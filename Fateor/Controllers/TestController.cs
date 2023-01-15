using Microsoft.AspNetCore.Mvc;

namespace Fateor.Controllers
{
    public class TestController : Controller
    {
        [Route("test1")]
        public IActionResult Test1()
        {
            if (HttpContext.Request.Method=="POST")
            {
                TempData["messages"] = new List<string>() { "New Message" };
                return RedirectToAction("test2");
            }
            else
                return View();
        }

        [Route("test2")]
        public IActionResult Test2()
        {
            return View();
        }
    }
}
