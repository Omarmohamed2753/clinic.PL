using clinic.BLL.Service.abstraction;
using Microsoft.AspNetCore.Mvc;
using clinic.BLL.ModelVM.Appointment;

using System.Threading.Tasks;         
using System.Linq;
namespace clinic.PL.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        public async Task<IActionResult> Create()
        {
            var viewModel = await _appointmentService.PrepareAppointmentViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppointmentVM viewModel)
        {
            if (ModelState.IsValid)
            {
                await _appointmentService.CreateAppointmentAsync(viewModel);
                return RedirectToAction("Success");
            }

            var repopulatedViewModel = await _appointmentService.PrepareAppointmentViewModel();
            viewModel.Departments = repopulatedViewModel.Departments;
            return View(viewModel);
        }

        [HttpGet]
        public async Task<JsonResult> GetDoctorsByDepartment(int departmentId)
        {
            var doctors = await _appointmentService.GetDoctorsByDepartmentAsync(departmentId);
            var result = doctors.Select(d => new { id = d.Id, name = d.Person.FirstName + " " + d.Person.LastName });
            return Json(result);
        }

        public IActionResult Success()
        {
            return View();
        }
    
}
}
