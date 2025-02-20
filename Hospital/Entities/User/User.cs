using Hospital.Enums;
using Hospital.PeopleCategories.PersonClass;
using Hospital.PeopleCategories.WardClass;
using Hospital.Utilities.UserInterface;

namespace Hospital.PeopleCategories.UserClass
{
    public class User : Person
    {
        public virtual int Id { get; set; }
        public virtual string Login { get; }
        public virtual string Password { get; set; }
        public virtual Rank Rank { get; set; }
        public virtual IList<Ward> AssignedWards { get; set; }

        protected User() { }

        public User(
            string name,
            string surname,
            Gender gender,
            DateTime birthday,
            string login,
            string password,
            Rank rank,
            IList<Ward> assignedWards)
            : base(name,
                  surname,
                  gender,
                  birthday)
        {
            Login = login;
            Password = password;
            Rank = rank;
            AssignedWards = assignedWards;
            IntroduceString = string.Format(
                UiMessages.UserObjectMessages.Introduce, login);
        }
    }
}