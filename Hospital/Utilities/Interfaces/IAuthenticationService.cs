using Hospital.PeopleCategories.UserClass;

namespace Hospital.Utilities.Interfaces
{
    public interface IAuthenticationService
    {
        bool Authenticate(string userPassword, string inputPassword);
        User? GetUserByLogin(string login);
    }
}