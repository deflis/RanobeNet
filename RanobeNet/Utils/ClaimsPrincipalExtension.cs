using System.Security.Claims;

namespace RanobeNet.Utils
{
    internal static class ClaimsPrincipalExtension
    {
        public static string GetFirebaseUid(this ClaimsPrincipal user)
        {
            var claim = user.Claims.FirstOrDefault(claim => claim.Type == "user_id");
            return claim?.Value ?? "";
        }
    }
}
