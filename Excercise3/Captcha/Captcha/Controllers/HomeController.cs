using hbehr.recaptcha;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;

namespace Captcha.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Forma()
        {

            return View();

        }
        [HttpPost]
        public ActionResult Forma(string myinput1)
        {
            var r = HttpContext.Request.Params["g-recaptcha-response"];
            bool validCaptcha = ReCaptcha.ValidateCaptcha(r);
            if (validCaptcha)
                return Content($"Hi there HUMAN");
            else
                return Content($"ROBOT ALERT");
        }
    }
}