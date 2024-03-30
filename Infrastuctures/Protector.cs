using Microsoft.AspNetCore.DataProtection;

namespace mygallery.Infrastuctures
{
    public class Protector
    {
        private readonly IDataProtector protector;
        public Protector(IDataProtectionProvider dataProtectionProvider, string SecretKey)
        {
             protector = dataProtectionProvider.CreateProtector(SecretKey);
        }

        public string Encrypt(string data)
        {
            return protector.Protect(data);
        }

        public string Decrypt(string data)
        {
            return protector.Unprotect(data);
        }
    }
}