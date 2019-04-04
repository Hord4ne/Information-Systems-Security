namespace OAuth2.Models
{
    public class YoutubeRO: OAuth2Base
    {
        public static string Subscribers => @"https://www.googleapis.com/youtube/v3/subscriptions?part=snippet%2CcontentDetails&mine=true";

        public YoutubeRO(string token) : base(token) { }

        public string GetSubscriptions()
        {
            return GetBaseReq(Subscribers);
        }
    }
}