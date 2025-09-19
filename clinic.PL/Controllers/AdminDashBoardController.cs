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
using clinic.BLL.ModelVM.Department;
using clinic.DAL.Enum;

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
            var doctorResult = await _doctorService.GetAllAsync();
            var patientResult = await _patientService.GetAllWithPersonAndRecordAsync();
            var appointmentResult = await _appointmentService.GetAllAppointmentsAsync();

            var doctors = doctorResult.doctors ?? new List<GetAllDoctorVM>();
            var patients = patientResult ?? new List<Patient>();
            var appointments = appointmentResult ?? new List<Appointment>();

            var model = new AdminDashBoardVM
            {
                DoctorCount = doctors.Count(),
                PatientCount = patients.Count(),
                AppointmentCount = appointments.Count(),

                UpcomingAppointments = appointments
                    .OrderBy(a => a.AppointmentDate)
                    .Take(5)
                    .Select(a => new AppointmentDashboardVM
                    {
                        Id = a.ID,
                        PatientName = a.Patient != null && a.Patient.Person != null
                                        ? a.Patient.Person.FirstName + " " + a.Patient.Person.LastName
                                        : "N/A",
                        PatientLocation = a.Patient?.Person?.Address ?? "N/A",
                        DoctorName = a.Doctor != null && a.Doctor.Person != null
                                        ? a.Doctor.Person.FirstName + " " + a.Doctor.Person.LastName
                                        : "N/A",
                        AppointmentDate = a.AppointmentDate
                    }).ToList(),

                Doctors = doctors.Take(5).ToList(),
                NewPatients = patients
                    .OrderByDescending(p => p.Id)
                    .Take(5)
                    .Select(p => new PatientDashBoardVM
                    {
                        FullName = p.Person != null
                                    ? p.Person.FirstName + " " + p.Person.LastName
                                    : "N/A",
                        Phone = p.Person?.Phone ?? "N/A",
                        Disease = p.Appointments?
                                    .Select(a => a.MedicalRecord?.Diagnosis)
                                    .FirstOrDefault() ?? "N/A"
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
            var departments = await _departmentService.GetAllAsync();
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
        public async Task<IActionResult> Departments()
        {
            var department = await _departmentService.GetAllAsync();
            return View(department.Item3);
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
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                .ToList();
                return Json(errors);
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
        [HttpGet]
        public async Task<IActionResult> CreatePatient()
        {
            var departments = await _departmentService.GetAllAsync(); // جلب الأقسام
            ViewBag.Departments = new SelectList(departments.Item3, "Id", "DepartmentName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePatient(CreatePatientVM model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                .ToList();
                return Json(errors);
            }


            var (success, errorMessage) = await _patientService.CreateAsync(model);

            if (!success)
            {
                ModelState.AddModelError("", errorMessage ?? "Something went wrong while creating Patient");
                return View(model);
            }

            TempData["SuccessMessage"] = "Patient created successfully!";
            return RedirectToAction("Patients");
        }

        [HttpGet]
        public async Task<IActionResult> EditPatient(int id)
        {
            var (isError, errorMessage, patient) = await _patientService.GetPatientForEditAsync(id);
            if (isError || patient == null)
            {
                TempData["ErrorMessage"] = errorMessage ?? "Patient not found";
                return RedirectToAction("Patients");
            }

            return View(patient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPatient(EditPatientVM model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                return Json(new { success = false, errors });
            }

            var (success, errorMessage) = await _patientService.EditAsync(model);

            if (!success)
            {
                return Json(new { success = false, error = errorMessage ?? "Something went wrong while updating patient" });
            }

            TempData["SuccessMessage"] = "Patient updated successfully!";
            return RedirectToAction("Patients", "AdminDashBoard");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var result = await _patientService.DeleteAsync(id);

            if (!result.success)
            {
                TempData["ErrorMessage"] = result.errorMessage ?? "Failed to delete patient.";
                return RedirectToAction("Index");
            }

            TempData["SuccessMessage"] = "Patient deleted successfully.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult CreateDepartment()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDepartment(CreateDepartmentVM model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(ms => ms.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                return Json(new { success = false, errors });
            }

            var (success, errorMessage) = await _departmentService.CreateAsync(model);

            if (!success)
            {
                return Json(new { success = false, errors = new { General = new[] { errorMessage ?? "Something went wrong while creating department" } } });
            }

            TempData["SuccessMessage"] = "Department created successfully!";
            return RedirectToAction("Departments");
        }

        [HttpGet]
        public async Task<IActionResult> EditDepartment(int id)
        {
            var (isError, errorMessage, dept) = await _departmentService.GetDepartmentByIdAsync(id);

            if (isError || dept == null)
            {
                return NotFound(errorMessage ?? "Department not found");
            }

            var model = new EditDepartmentVM
            {
                Id = dept.Id,
                DepartmentName = dept.Name,
                Description = dept.Description
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDepartment(EditDepartmentVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var (success, errorMessage) = await _departmentService.EditAsync(model);

            if (!success)
            {
                ModelState.AddModelError("", errorMessage ?? "Something went wrong while updating department");
                return View(model);
            }

            TempData["SuccessMessage"] = "Department updated successfully!";
            return RedirectToAction("Departments");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var result = await  _departmentService.DeleteAsync(id);

            if (!result.success)
            {
                TempData["ErrorMessage"] = result.errorMessage ?? "Failed to delete department.";
                return RedirectToAction("Index");
            }

            TempData["SuccessMessage"] = "department deleted successfully.";
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> AllAppointments()
        {
            var appointments = await _appointmentService.GetAllAppointmentsAsync();

            var model = appointments.Select(a => new GetAllAppointmentVM
            {
                Id = a.ID,
                PatientName = a.Patient?.Person != null
                                ? $"{a.Patient.Person.FirstName} {a.Patient.Person.LastName}"
                                : "Unknown",
                Age = a.Patient?.Person.Age,

                DoctorName = a.Doctor?.Person != null
                                ? $"Dr. {a.Doctor.Person.FirstName} {a.Doctor.Person.LastName}"
                                : "Unknown",
                Department = a.Doctor?.Department?.DepartmentName,
                AppointmentDate = a.AppointmentDate,

                Status = a.Status switch
                {
                    AppointmentStatus.PendingPayment => "Pending Payment",
                    AppointmentStatus.Scheduled => "Scheduled",
                    AppointmentStatus.Completed => "Completed",
                    AppointmentStatus.Cancelled => "Cancelled",
                    _=> "Unknown"
                },


                PaymentStatus = string.IsNullOrEmpty(a.PaymentIntentId) ? "Pending" : "Paid",

                PatientImg = a.Patient?.imgPath ?? "~/assets/img/user.jpg"

            }).ToList();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CreateAppointment()
        {
            var departments = await _appointmentService.GetAllDepartmentsForDropdownAsync();
            var patients = await _appointmentService.GetAllPatientsForDropdownAsync();
            var doctors = await _appointmentService.GetAllDoctorsForDropdownAsync();

            var vm = new CreateAppointmentVM
            {
                Departments = departments.Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                }).ToList(),

                Patients = patients.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToList(),

                Doctors = doctors.Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                }).ToList(),
            };

            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAppointment(CreateAppointmentVM vm)
        {
            if (!ModelState.IsValid)
            {
                var departments = await _appointmentService.GetAllDepartmentsForDropdownAsync();
                var patients = await _appointmentService.GetAllPatientsForDropdownAsync();
                var doctors = await _appointmentService.GetAllDoctorsForDropdownAsync();

                vm.Departments = departments.Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                }).ToList();

                vm.Patients = patients.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToList();

                vm.Doctors = doctors.Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                }).ToList();

                return View(vm);
            }

            await _appointmentService.AddAppointmentFromAdminAsync(vm);
            return RedirectToAction("AllAppointments", "AdminDashBoard");
        }
        [HttpGet]
        public async Task<IActionResult> GetDoctorsByDepartment(int departmentId)
        {
            var doctors = await _appointmentService.GetDoctorsByDepartmentAsync(departmentId);
            var result = doctors.Select(d => new
            {
                id = d.Id,
                name = "Dr. " + d.Person.FirstName + " " + d.Person.LastName
            });
            return Json(result);
        }

    }

}

