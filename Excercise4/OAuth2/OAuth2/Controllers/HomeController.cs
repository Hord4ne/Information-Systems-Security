using Newtonsoft.Json;
using OAuth2.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OAuth2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendAuth(string scope)
        {
            var x = GoogleScopeRequest.GetRequestCodeUrl(scope, "http://localhost:56546/Home/YoutubeSubs");

            return Redirect(x);
        }

        public ActionResult YoutubeSubs(string code)
        {
            var jsonToken = GoogleScopeRequest.GetToken(code, "http://localhost:56546/Home/YoutubeSubs");
            var obj = JsonConvert.DeserializeObject<TokenResponse>(jsonToken);
            var yt = new YoutubeRO(obj.Access_Token);
            var subs = yt.GetSubscriptions();
            var subsDeserialized = JsonConvert.DeserializeObject<SubsResponse>(subs);
            return View(subsDeserialized);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}