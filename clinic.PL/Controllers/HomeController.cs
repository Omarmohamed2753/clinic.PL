using clinic.BLL.Service.abstraction;
using Microsoft.AspNetCore.Mvc;

namespace clinic.PL.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDoctorService _doctorService;

        public HomeController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        public IActionResult Index()
        {
            var doctors = _doctorService.GetAllAsync(); // ??? ????? ?? ????????
            return View(doctors.Result.doctors); // ??? ?? ?????? ???? ???? Compatible ?? ??? View
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
