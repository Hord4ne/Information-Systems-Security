using System.Collections.Generic;

namespace OAuth2.Models

{
    public class SubsResponse
    {
        public string nextPageToken { get; set; }
        public List<SubsItem> Items { get; set; }

    }
}