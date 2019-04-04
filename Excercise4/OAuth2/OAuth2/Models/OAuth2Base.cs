using System.Net;

namespace OAuth2.Models
{
    public abstract class OAuth2Base
    {
        protected string _token;
        public OAuth2Base(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new System.ArgumentException();
            _token = token;
        }
        public string GetBaseReq(string req)
        {
            string result = "";
            using (WebClient client = new WebClient())
            {
                client.Headers.Add("Authorization", $"Bearer {_token}");
                result = client.DownloadString(req);
            }
            return result;
        }
    }
}