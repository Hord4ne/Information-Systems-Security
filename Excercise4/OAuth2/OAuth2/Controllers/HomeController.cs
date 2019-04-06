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
        public ActionResult SendAuth(string scope, string redirect)
        {
            var x = GoogleScopeRequest.GetRequestCodeUrl(scope, redirect);

            return Redirect(x);
        }

        public ActionResult Scopes(string code)
        {
            var jsonToken = GoogleScopeRequest.GetToken(code, Constants.ScopesRedirect);
            var tokenInfo = JsonConvert.DeserializeObject<TokenResponse>(jsonToken);

            return Content($"<html>Your token is {tokenInfo.Access_Token} :)</html>");
        }

        public ActionResult Gmail(string code)
        {
            var jsonToken = GoogleScopeRequest.GetToken(code, Constants.GmailRedirect);
            var tokenInfo = JsonConvert.DeserializeObject<TokenResponse>(jsonToken);
            var gmail = new GmailRO(tokenInfo.Access_Token);
            var messageInfo = gmail.GetMessages();
            var messagedDeserialized = JsonConvert.DeserializeObject<GmailMessagesResponse>(messageInfo);

            return View(messagedDeserialized);
        }

        public ActionResult YoutubeSubs(string code)
        {
            var jsonToken = GoogleScopeRequest.GetToken(code, Constants.YoutubeRedirect);
            var tokenInfo = JsonConvert.DeserializeObject<TokenResponse>(jsonToken);
            var yt = new YoutubeRO(tokenInfo.Access_Token);
            var subs = yt.GetSubscriptions();
            var subsDeserialized = JsonConvert.DeserializeObject<SubsResponse>(subs);
            return View(subsDeserialized);
        }
    }
}