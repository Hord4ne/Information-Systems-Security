namespace OAuth2.Models
{
    public static class Constants
    {
        public const string GoogleApiUrl = @"https://accounts.google.com/o/oauth2/v2/auth";
        public const string GoogleApiPostUrl = @"https://accounts.google.com/o/oauth2/token";
        public const string YtScope = @"https://www.googleapis.com/auth/youtube.readonly";
        public const string GmailScope = @"https://www.googleapis.com/auth/gmail.readonly";
        public const string Scopes = @"https://www.googleapis.com/auth/gmail.readonly https://www.googleapis.com/auth/youtube.readonly https://www.googleapis.com/auth/blogger https://www.googleapis.com/auth/gmail.settings.basic https://www.googleapis.com/auth/youtube.upload";
        public const string BaseAddress = @"http://localhost:56546";
        public static string YoutubeRedirect => $@"{BaseAddress}/Home/YoutubeSubs";
        public static string GmailRedirect => $@"{BaseAddress}/Home/Gmail";
        public static string ScopesRedirect => $@"{BaseAddress}/Home/Scopes";
    }
}