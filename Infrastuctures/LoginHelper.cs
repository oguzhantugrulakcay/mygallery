using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using mygallery.Context;
using mygallery.Data;

namespace mygallery.Infrastuctures
{
    public class LoginHelper
    {
        private readonly Protector protector;
        private readonly string cookieKey = "";
        private readonly IDataProtectionProvider dataProtectionProvider;
        private readonly int expireMinutes;
        private readonly string secretKey;
        private readonly string spName;
        private readonly AppConfig appConfig;
        private readonly MyGalleryContext dbContext;
        public LoginHelper(MyGalleryContext context, AppConfig config, IDataProtectionProvider DataProtectionProvider, string CookieKey, string SpName, int ExpireMinutes = 60)
        {
            dbContext = context;
            appConfig = config;
            dataProtectionProvider = DataProtectionProvider;
            secretKey = appConfig.SecretKey;
            cookieKey = CookieKey;
            spName = spName;
            expireMinutes = ExpireMinutes;
            protector = new Protector(dataProtectionProvider, secretKey);
        }

        public async Task<Result> LoginAsync(HttpContext httpContext, string LoginName, string LoginPassword, bool RememberMe)
        {

            if (string.IsNullOrWhiteSpace(LoginName) || string.IsNullOrWhiteSpace(LoginPassword))
                return new Result(false, "LOGIN_DATA_REQUIRED");
            var hashedPassword = Hash(LoginPassword, secretKey);
            var user = dbContext
                    .Users
                    .FirstOrDefault(u => u.LoginName == LoginName);
            if (user == null)
                return new Result(false, "Kullanıcı adı bulunamadı lütfen kontrol ediniz!");

            if (user.LoginPassword != hashedPassword)
                return new Result(false, "Şifre hatalı, lütfen kontrol ediniz!");

            var claimsIdentity = new ClaimsIdentity(new List<Claim>
            {
                new("UserData", JsonSerializer.Serialize(new{
                    user.UserId,
                    user.FirstName,
                    user.LastName
                }))
            }, "Cookies");

            var properties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(expireMinutes)
            };

            await httpContext.SignInAsync("Cookies", new ClaimsPrincipal(claimsIdentity), properties);

            if (RememberMe)
            {
                var str = protector.Encrypt(JsonSerializer.Serialize(new CookieData
                {
                    LoginName = LoginName,
                    LoginPassword = LoginPassword,
                    RememberMe = RememberMe
                }));
                httpContext.Response.Cookies.Append(cookieKey, str);
            }
            else if (httpContext.Request.Cookies.ContainsKey(cookieKey))
            {
                httpContext.Response.Cookies.Delete(cookieKey);
            }

            return new Result(true);
        }

        public CookieData GetCookie(HttpContext httpContext)
        {
            if (httpContext.Request.Cookies.ContainsKey(cookieKey))
                try
                {
                    var data = httpContext.Request.Cookies[cookieKey];
                    return JsonSerializer.Deserialize<CookieData>(protector.Decrypt(data));
                }
                catch (Exception)
                {
                    return new CookieData
                    {
                        RememberMe = false,
                        LoginName = "",
                        LoginPassword = ""
                    };
                }

            return new CookieData
            {
                RememberMe = false,
                LoginName = "",
                LoginPassword = ""
            };
        }
        public string SafeReturnUrl(string Host, string returnUrl, string index = "index")
        {
            return returnUrl == null ||
                   string.IsNullOrEmpty(returnUrl) || returnUrl == "/" ||
                   (!returnUrl.StartsWith("/") && !returnUrl.StartsWith(Host))
                ? index
                : returnUrl;
        }

        public async void LogoutAsync(HttpContext httpContext)
        {
            await httpContext.SignOutAsync("Cookies");
        }

        public static async void Logout(HttpContext httpContext)
        {
            await httpContext.SignOutAsync("Cookies");
        }

        public string Hash(string Password)
        {
            return Convert.ToBase64String(
                KeyDerivation.Pbkdf2(Password,
                    Encoding.ASCII.GetBytes(secretKey), KeyDerivationPrf.HMACSHA1, 10000, 32));
        }

        public static string Hash(string SecretKey, string Password)
        {
            return Convert.ToBase64String(
                KeyDerivation.Pbkdf2(Password,
                    Encoding.ASCII.GetBytes(SecretKey), KeyDerivationPrf.HMACSHA1, 10000, 32));
        }

    }
}