using EdTech.Interfaces;

namespace EdTech.Utils
{
    public class BCryptHelper : Interfaces.IBCryptHelper

    {
        string IBCryptHelper.HashPassword(string password)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            return hashedPassword;
        }
        bool IBCryptHelper.CheckPassword(string password, string hashedPassword)
        {
            bool ValidPassword = BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            return ValidPassword;
        }

      
    }
}
