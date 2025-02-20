using Hospital.PeopleCategories.UserClass;
using Hospital.Utilities.Interfaces;
using Hospital.Utilities.ListManagement.Interfaces;

namespace Hospital.Utilities
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IListsStorage _listsStorage;

        public AuthenticationService(
            IListsStorage listsStorage)
        {
            _listsStorage = listsStorage;
        }

        public bool Authenticate(string userPassword, string inputPassword)
        {
            return userPassword == inputPassword;
        }

        public User? GetUserByLogin(string login)
        {
            return _listsStorage.Users.FirstOrDefault(u => u.Login == login);
        }
    }
}