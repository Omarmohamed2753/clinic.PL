using Microsoft.Extensions.Configuration;
using clinic.BLL.ModelVM.Appointment;
using clinic.DAL.Entities;
using clinic.DAL.Enum;
using clinic.DAL.Repo.abstraction;
using Microsoft.AspNetCore.Mvc.Rendering;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using clinic.BLL.ModelVM.Common;
using Microsoft.EntityFrameworkCore;

namespace clinic.BLL.Service.implementation
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepo _appointmentRepo;
        private readonly string _stripeSecretKey;

        public AppointmentService(IAppointmentRepo appointmentRepo, IConfiguration config)
        {
            _appointmentRepo = appointmentRepo;
            _stripeSecretKey = config["StripeSettings:SecretKey"];
        }

        public async Task<AppointmentVM> PrepareAppointmentViewModel()
        {
            var departments = await _appointmentRepo.GetAllDepartmentsAsync();
            return new AppointmentVM
            {
                Departments = departments.Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.DepartmentName
                }).ToList()
            };
        }

        public async Task<Session> CreateCheckoutSessionAsync(AppointmentVM viewModel, string successUrl, string cancelUrl, string userId)
        {
            var doctor = await _appointmentRepo.GetDoctorByIdAsync(viewModel.DoctorID);
            if (doctor == null || doctor.Person == null)
            {
                throw new Exception("Doctor not found or doctor details are incomplete.");
            }

            var patientRecord = await _appointmentRepo.GetPatientByUserIdAsync(userId);

            if (patientRecord == null)
            {
                var person = new clinic.DAL.Entities.Person
                {
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    Phone = viewModel.Phone,
                    Address = viewModel.Address,
                    CreatedOn = DateTime.UtcNow,
                    Age = viewModel.Age,
                };

                patientRecord = new Patient
                {
                    Person = person,
                    UserId = userId
                };

                patientRecord.UpdatePatient(
                    viewModel.BloodType,
                    viewModel.Allergies,
                    viewModel.EmergencyContactPhone,
                    viewModel.MedicalHistory
                );
            }
            else
            {
                patientRecord.UpdatePatient(
                    viewModel.BloodType,
                    viewModel.Allergies,
                    viewModel.EmergencyContactPhone,
                    viewModel.MedicalHistory
                );
            }

            var appointment = new Appointment(0, viewModel.AppointmentDate, AppointmentStatus.PendingPayment)
            {
                Patient = patientRecord,
                DoctorID = viewModel.DoctorID,
            };

            await _appointmentRepo.AddAsync(appointment);
            await _appointmentRepo.SaveChangesAsync();

            StripeConfiguration.ApiKey = _stripeSecretKey;

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(doctor.ConsultationFee * 100),
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = $"Consultation with Dr. {doctor.Person.LastName}",
                                Description = $"Appointment on {viewModel.AppointmentDate.ToShortDateString()}",
                            },
                        },
                        Quantity = 1,
                    },
                },
                Mode = "payment",
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl,
            };

            var service = new SessionService();
            Session session = await service.CreateAsync(options);

            appointment.StripeSessionId = session.Id;
            await _appointmentRepo.SaveChangesAsync();

            return session;
        }

        public async Task<Appointment> FulfillAppointmentOrderAsync(string sessionId)
        {
            StripeConfiguration.ApiKey = _stripeSecretKey;

            var service = new SessionService();
            Session session = await service.GetAsync(sessionId);

            if (session.PaymentStatus == "paid")
            {
                var appointment = await _appointmentRepo.GetAppointmentBySessionIdAsync(sessionId);
                if (appointment != null)
                {
                    appointment.Status = AppointmentStatus.Scheduled;
                    appointment.PaymentIntentId = session.PaymentIntentId;
                    _appointmentRepo.UpdateAppointment(appointment);
                    await _appointmentRepo.SaveChangesAsync();
                    return appointment;
                }
            }
            return null;
        }

        public async Task<IEnumerable<Doctor>> GetDoctorsByDepartmentAsync(int departmentId)
        {
            return await _appointmentRepo.GetDoctorsByDepartmentAsync(departmentId);
        }

        public async Task<List<Appointment>> GetAllAppointmentsAsync()
        {
            return await _appointmentRepo.GetAllAsync();
        }
        public async Task AddAppointmentFromAdminAsync(CreateAppointmentVM vm)
        {
            AppointmentStatus statusToSave = vm.Status switch
            {
                AppointmentStatus.Scheduled => AppointmentStatus.Scheduled,
                AppointmentStatus.Completed => AppointmentStatus.Completed,
                AppointmentStatus.Cancelled => AppointmentStatus.Cancelled,
                AppointmentStatus.PendingPayment => AppointmentStatus.PendingPayment,
                _ => AppointmentStatus.PendingPayment 
            };

            var appointment = new Appointment(0, vm.AppointmentDate, statusToSave)
            {
                DoctorID = vm.DoctorID,
                PatientID = vm.PatientID
            };

            await _appointmentRepo.AddAsync(appointment);
            await _appointmentRepo.SaveChangesAsync();

        }
        public async Task<IEnumerable<DropdownVM>> GetAllPatientsForDropdownAsync()
        {
            var patients = await _appointmentRepo.GetAllPatientsAsync();
            return patients.Select(p => new DropdownVM
            {
                Id = p.Id,
                Name = p.Person.FirstName + " " + p.Person.LastName
            }).ToList();
        }

        public async Task<IEnumerable<DropdownVM>> GetAllDoctorsForDropdownAsync()
        {
            var doctors = await _appointmentRepo.GetAllDoctorsAsync();
            return doctors.Select(d => new DropdownVM
            {
                Id = d.Id,
                Name = "Dr. " + d.Person.FirstName + " " + d.Person.LastName
            }).ToList();
        }

        public async Task<IEnumerable<DropdownVM>> GetAllDepartmentsForDropdownAsync()
        {
            var departments = await _appointmentRepo.GetAllDepartmentsAsync();
            return departments.Select(dep => new DropdownVM
            {
                Id = dep.Id,
                Name = dep.DepartmentName,
            }).ToList();
        }

    }
}
