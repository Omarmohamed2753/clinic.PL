using Microsoft.AspNetCore.Mvc;

namespace clinic.PL.Controllers
{
    public class DoctorDashBoardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
