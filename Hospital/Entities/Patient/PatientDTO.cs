using Hospital.PeopleCategories.PersonClass;
using Hospital.PeopleCategories.WardClass;

namespace Hospital.PeopleCategories.PatientClass
{
    public class PatientDTO : PersonDTO
    {
        public PatientDTO(PersonDTO person)
        {
            Name = person.Name;
            Surname = person.Surname;
            Gender = person.Gender;
            Birthday = person.Birthday;
        }

        public string PersonalIdNumber { get; set; }
        public Ward AssignedWard { get; set; }
    }
}