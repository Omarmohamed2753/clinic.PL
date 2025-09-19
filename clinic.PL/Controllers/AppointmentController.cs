using clinic.BLL.Service.abstraction;
using Microsoft.AspNetCore.Mvc;
using clinic.BLL.ModelVM.Appointment;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using clinic.PL.Models;
using Microsoft.Extensions.Options;

namespace clinic.PL.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly StripeSettings _stripeSettings;

        public AppointmentController(IAppointmentService appointmentService, IOptions<StripeSettings> stripeOptions)
        {
            _appointmentService = appointmentService;
            _stripeSettings = stripeOptions.Value;
        }

        [HttpGet]
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
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var domain = $"{Request.Scheme}://{Request.Host}";
                var successUrl = $"{domain}/Appointment/PaymentSuccess?sessionId={{CHECKOUT_SESSION_ID}}";
                var cancelUrl = $"{domain}/Appointment/PaymentFailed";

                try
                {
                    var session = await _appointmentService.CreateCheckoutSessionAsync(viewModel, successUrl, cancelUrl, userId);
                    return Redirect(session.Url);
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);
                }
            }

            var repopulatedViewModel = await _appointmentService.PrepareAppointmentViewModel();
            viewModel.Departments = repopulatedViewModel.Departments;
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> PaymentSuccess(string sessionId)
        {
            var appointment = await _appointmentService.FulfillAppointmentOrderAsync(sessionId);

            if (appointment == null)
            {
                return RedirectToAction("PaymentFailed");
            }

            return View();
        }

        [HttpGet]
        public IActionResult PaymentFailed()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetDoctorsByDepartment(int departmentId)
        {
            var doctors = await _appointmentService.GetDoctorsByDepartmentAsync(departmentId);
            var result = doctors.Select(d => new { id = d.Id, name = d.Person.FirstName + " " + d.Person.LastName });
            return Json(result);
        }
    }
}
