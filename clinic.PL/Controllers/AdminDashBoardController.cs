using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using clinic.BLL.Service.abstraction;
using clinic.BLL.ModelVM.AdminDashBoard;
using clinic.BLL.ModelVM.Appointment;
using clinic.BLL.ModelVM.Doctor;
using clinic.BLL.ModelVM.Patient;
using clinic.DAL.Entities;
using clinic.BLL.ModelVM.PatientDashBoard;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using clinic.DAL.DataBase;
using clinic.BLL.Service.implementation;
using clinic.BLL.ModelVM.User;

namespace clinic.PL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminDashboardController : Controller
    {
        private readonly IDoctorService _doctorService;
        private readonly IPatientServicecs _patientService;
        private readonly IAppointmentService _appointmentService;
        private readonly IDepartmentService _departmentService;

        public AdminDashboardController(
            IDoctorService doctorService,
            IPatientServicecs patientService,
            IAppointmentService appointmentService,
            IDepartmentService departmentService)
        {
            _doctorService = doctorService;
            _patientService = patientService;
            _appointmentService = appointmentService;
            _departmentService = departmentService;
        }

        public async Task<IActionResult> Index()
        {
            // Get data from services
            var doctorResult = await _doctorService.GetAllAsync();
            var patientResult = await _patientService.GetAllWithPersonAndRecordAsync();
            var appointmentResult = await _appointmentService.GetAllAppointmentsAsync();

            var doctors = doctorResult.doctors ?? new List<GetAllDoctorVM>();
            var patients = patientResult ?? new List<Patient>();
            var appointments = appointmentResult ?? new List<Appointment>();

            // Fill Dashboard ViewModel
            var model = new AdminDashBoardVM
            {
                DoctorCount = doctors.Count(),
                PatientCount = patients.Count(),
                AppointmentCount = appointments.Count(),

                // Upcoming Appointments
                UpcomingAppointments = appointments.Take(5).Select(a => new AppointmentDashboardVM
                {
                    Id = a.ID,
                    PatientName = a.Patient?.Person.FirstName + " " + a.Patient?.Person.LastName,
                    PatientLocation = a.Patient?.Person.Address,
                    DoctorName = a.Doctor?.Person.FirstName + " " + a.Doctor?.Person.LastName,
                    AppointmentDate = a.AppointmentDate
                }).ToList(),

                // Doctors
                Doctors = doctors.Take(5).ToList(),

                // New Patients (من Person Navigation)
                NewPatients = patients
                .OrderByDescending(p => p.Id)
                .Take(5)
                .Select(p => new PatientDashBoardVM
                {
                    FullName = p.Person.FirstName + " " + p.Person.LastName,
                    Email = p.Person.Email,
                    Phone = p.Person.Phone,
                    Disease = p.Appointments
                                .Select(a => a.MedicalRecord.Diagnosis)
                                .FirstOrDefault()
                }).ToList(),

                OPDPercent = 16,
                NewPatientPercent = 71,
                LabTestPercent = 82,
                TreatmentPercent = 67,
                DischargePercent = 30
            };

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var departments = await _departmentService.GetAllAsync(); // جلب الأقسام
            ViewBag.Departments = new SelectList(departments.Item3, "Id", "DepartmentName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDoctorVM model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                .ToList();
                return Json(errors);
            }


            var (success, errorMessage) = await _doctorService.CreateAsync(model);

            if (!success)
            {
                ModelState.AddModelError("", errorMessage ?? "Something went wrong while creating doctor");
                return View(model);
            }

            TempData["SuccessMessage"] = "Doctor created successfully!";
            return RedirectToAction("Doctors");
        }


        public async Task<IActionResult> Doctors()
        {
            var doctors = await _doctorService.GetAllAsync();
            return View(doctors.doctors);
        }

        public async Task<IActionResult> Patients()
        {
            var patients = await _patientService.GetAllAsync();
            return View(patients.patients);
        }

        public async Task<IActionResult> Appointments()
        {
            var appointments = await _appointmentService.GetAllAppointmentsAsync();

            var mapped = appointments.Select(a => new AppointmentDashboardVM
            {
                Id = a.ID,
                PatientName = a.Patient?.Person.FirstName + " " + a.Patient?.Person.LastName,
                PatientLocation = a.Patient?.Person.Address,
                DoctorName = a.Doctor?.Person.FirstName + " " + a.Doctor?.Person.LastName,
                AppointmentDate = a.AppointmentDate
            }).ToList();

            return View(mapped);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var (isError, errorMessage, doctor) = await _doctorService.GetDoctorForEditAsync(id);
            if (isError || doctor == null)
            {
                TempData["ErrorMessage"] = errorMessage ?? "Doctor not found";
                return RedirectToAction("Doctors");
            }

            return View(doctor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditDoctorVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var (success, errorMessage) = await _doctorService.EditAsync(model);

            if (!success)
            {
                ModelState.AddModelError("", errorMessage ?? "Something went wrong while updating doctor");
                return View(model);
            }

            TempData["SuccessMessage"] = "Doctor updated successfully!";
            return RedirectToAction("Doctors");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _doctorService.DeleteAsync(id);

            if (!result.success)
            {
                TempData["ErrorMessage"] = result.errorMessage ?? "Failed to delete doctor.";
                return RedirectToAction("Index");
            }

            TempData["SuccessMessage"] = "Doctor deleted successfully.";
            return RedirectToAction("Index"); 
        }


    }
}
