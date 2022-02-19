using System.Security.Claims;

namespace RanobeNet.Utils
{
    internal static class ClaimsPrincipalExtension
    {
        public static string GetFirebaseUid(this HttpContext context)
        {
            var claim = context.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
            return claim?.Value ?? "";
        }
    public static string GetFirebaseName(this HttpContext context)
    {
        var claim = context.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name);
        return claim?.Value ?? "";
    }
}
}
