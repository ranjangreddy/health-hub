using Hospital.Enums;
using Hospital.PeopleCategories.PersonClass;
using Hospital.PeopleCategories.WardClass;

namespace Hospital.PeopleCategories.UserClass
{
    public class UserDTO : PersonDTO
    {
        public UserDTO(PersonDTO person)
        {
            Name = person.Name;
            Surname = person.Surname;
            Gender = person.Gender;
            Birthday = person.Birthday;
        }

        public string Login { get; set; }

        public string Password { get; set; }

        public Rank Rank { get; set; }

        public IList<Ward> AssignedWards { get; set; }
    }
}