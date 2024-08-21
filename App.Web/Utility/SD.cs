namespace App.Web.Utility
{
    public class SD
    {
        public static string APIBaseUrl { get; set; }
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
