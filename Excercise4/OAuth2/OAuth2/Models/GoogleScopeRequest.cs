using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Web.Configuration;

namespace OAuth2.Models
{
    public static class GoogleScopeRequest
    {
        public static string GetRequestCodeUrl(string scope, string redirectUri)
        {
            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);

            var clientId = WebConfigurationManager.AppSettings["clientId"];

            queryString["redirect_uri"] = redirectUri;
            queryString["response_type"] = "code";
            queryString["client_id"] = clientId;
            queryString["scope"] = scope;
            queryString["access_type"] = "offline";

            return $"{Constants.GoogleApiUrl}?{queryString}";
        }

        public static string GetToken(string code, string redirectUri)
        {
            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);

            var clientId = WebConfigurationManager.AppSettings["clientId"];
            var clientSecret = WebConfigurationManager.AppSettings["secret"];

            queryString["code"] = code;
            queryString["redirect_uri"] = redirectUri;
            queryString["client_id"] = clientId;
            queryString["client_secret"] = clientSecret;
            queryString["scope"] = "";
            queryString["grant_type"] = "authorization_code";

            //return $"{Constants.GoogleApiPostUrl}?{queryString}";
            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                try
                {
                    var response = client.UploadString(Constants.GoogleApiPostUrl, "POST",queryString.ToString());
                    return response;
                }
                catch (WebException e)
                {
                    Console.WriteLine("This program is expected to throw WebException on successful run." +
                                        "\n\nException Message :" + e.Message);
                    if (e.Status == WebExceptionStatus.ProtocolError)
                    {
                        Console.WriteLine("Status Code : {0}", ((HttpWebResponse)e.Response).StatusCode);
                        Console.WriteLine("Status Description : {0}", ((HttpWebResponse)e.Response).StatusDescription);
                    }
                }
            }
            return "";
        }
    }
}