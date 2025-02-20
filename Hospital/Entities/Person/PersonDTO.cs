using Hospital.Enums;

namespace Hospital.PeopleCategories.PersonClass
{
    public class PersonDTO
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public Gender Gender { get; set; }
        public DateTime Birthday { get; set; }
    }
}