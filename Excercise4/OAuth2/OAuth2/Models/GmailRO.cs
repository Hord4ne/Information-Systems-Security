namespace OAuth2.Models
{
    public class GmailRO: OAuth2Base
    {
        public static string Messages => @"https://www.googleapis.com/gmail/v1/users/me/messages";

        public GmailRO(string token) : base(token) { }

        public string GetMessages()
        {
            return GetBaseReq(Messages);
        }
    }
}