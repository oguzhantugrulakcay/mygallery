using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Json;
using System.Threading.Tasks;
using mygallery.Data;

namespace mygallery.Extensions
{
    public static class ClaimsExtension
    {
        public static UserData GetUser(this ClaimsPrincipal principal)
        {
            if (principal == null) throw new ArgumentNullException("principal");

            return JsonSerializer.Deserialize<UserData>(principal.Claims.Select(c => c.Value).ToList()[0]);
        }

        public static UserData GetUser(this IPrincipal principal)
        {
            if (principal == null) throw new ArgumentNullException("principal");

            var list = (principal as ClaimsPrincipal).Claims.Select(c => c.Value).ToList();

            return new UserData
            {
                UserId = int.Parse(list[0]),
                FirstName = list[1],
                LastName = list[2]
            };
        }

        public static T GetUser<T>(this ClaimsPrincipal principal) where T : class
        {
            if (principal == null) throw new ArgumentNullException("principal");

            var text = (from c in principal.Claims
                        where c.Type == "UserData"
                        select c.Value).FirstOrDefault();

            if (principal.Identity.IsAuthenticated == false)
            {
                return null;
            }
            if (text == null)
            {
                var text2 = string.Join(",", principal.Claims.Select(c => c.Type));
                throw new Exception("UserDataNotFoundInClaimTypes available claimTypes:" + text2);
            }

            return JsonSerializer.Deserialize<T>(text);
        }
    }
}