using System.Collections.Generic;

namespace OAuth2.Models

{
    public class GmailMessagesResponse
    {
        public string nextPageToken { get; set; }
        public List<GmailMessage> Messages { get; set; }
    }
}