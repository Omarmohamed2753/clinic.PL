using clinic.BLL.Service.abstraction;
using Microsoft.AspNetCore.Mvc;

namespace clinic.PL.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly IDoctorService _doctorService;

        public DoctorsController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        public async Task<IActionResult> Index()
        {
            var doctors = await _doctorService.GetAllAsync();
            return View(doctors.doctors); 
        }
    }
}
