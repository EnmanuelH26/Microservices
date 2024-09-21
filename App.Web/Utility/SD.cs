namespace App.Web.Utility
{
    public class SD
    {
        public static string APIBaseUrl { get; set; }
        public static string AuthAPIBase { get; set; }
        public static string ProductAPIBase { get; set; }

        public const string RoleAdmin = "Admin";
        public const string RoleCustomer = "Customer";
        public const string TokenCookie = "JWToken";
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
