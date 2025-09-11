using clinic.DAL.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using clinic.BLL.ModelVM.Appointment;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace clinic.BLL.Service.implementation
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepo _appointmentRepo;

        public AppointmentService(IAppointmentRepo appointmentRepo)
        {
            _appointmentRepo = appointmentRepo;
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

        public async Task CreateAppointmentAsync(AppointmentVM viewModel)
        {
            var person = await _appointmentRepo.GetPersonByEmailAsync(viewModel.Email);
            Patient patientRecord;

            if (person == null)
            {
                person = new Person
                {
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    Email = viewModel.Email,
                    Phone = viewModel.Phone,
                    CreatedOn = DateTime.UtcNow
                };
                patientRecord = new Patient { Person = person };
            }
            else
            {
                patientRecord = await _appointmentRepo.GetPatientByPersonIdAsync(person.Id);
            }

            var appointment = new Appointment(0, viewModel.AppointmentDate, AppointmentStatus.Scheduled)
            {
                Patient = patientRecord,
                DoctorID = viewModel.DoctorID
            };

            await _appointmentRepo.AddAsync(appointment);
            await _appointmentRepo.SaveChangesAsync();
        }

        public async Task<IEnumerable<Doctor>> GetDoctorsByDepartmentAsync(int departmentId)
        {
            return await _appointmentRepo.GetDoctorsByDepartmentAsync(departmentId);
        }
        public async Task<List<Appointment>> GetAllAppointmentsAsync()
        {
            return await _appointmentRepo.GetAllAsync();
        }
    }

}
