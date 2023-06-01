using Microsoft.AspNetCore.Mvc;

namespace SP.EmployeeManagement.Api.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
