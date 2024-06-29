using System.Security.Claims;

namespace HaveFun.Common
{
    public static class UserExtension
    {
        public static int GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            var claims = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);

            if(claims == null)
            {
                return 0;
            }
            return Convert.ToInt32(claims.Value);
        }
    }
}
