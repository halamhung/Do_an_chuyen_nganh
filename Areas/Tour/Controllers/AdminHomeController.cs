using HKQTravellingAuthenication.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HKQTravellingAuthenication.Areas.Tour.Controllers
{
    [Area("Tour")]
    [Route("admin-manager")]
    [Authorize(Roles = RoleName.Administrator + "," + RoleName.Editor)]
    public class AdminHomeController : Controller
    {
        [Route("index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
