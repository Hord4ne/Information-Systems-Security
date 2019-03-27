using hbehr.recaptcha;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Captcha
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
       //         < add key = "recaptcha-secret-key" value = "6LdxS5oUAAAAAHr167UZm48MSP7GpAZclQqpxlgK" />
   
       //< add key = "recaptcha-public-key" value = "6LdxS5oUAAAAAJw5GMqEsbec55ZcVzgw1kSiZ1qZ" />
                  AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            string publicKey = "6LfjTJoUAAAAAOVruGxPn4U9K_mNEsmkw7uxw0_9";
            string secretKey = "6LfjTJoUAAAAABJehsQyP5xDWktDFKanoQWcreiE";
            ReCaptcha.Configure(publicKey, secretKey);
        }
    }
}
