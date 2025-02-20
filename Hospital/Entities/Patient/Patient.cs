using Hospital.Entities.Employee;
using Hospital.Enums;
using Hospital.PeopleCategories.PersonClass;
using Hospital.PeopleCategories.WardClass;
using Hospital.Utilities.UserInterface;

namespace Hospital.PeopleCategories.PatientClass
{
    public class Patient : Person
    {
        public virtual int Id { get; set; }
        public virtual string PersonalIdNumber { get; set; }
        public virtual Health? HealthStatus { get; set; }
        public virtual Ward AssignedWard { get; set; }
        public virtual Employee? AssignedDoctor { get; set; }

        protected Patient() { }

        public Patient(
            string name,
            string surname,
            Gender gender,
            DateTime birthday,
            string personalIdNumber,
            Ward assignedWard)
            : base(name,
                  surname,
                  gender,
                  birthday)
        {
            PersonalIdNumber = personalIdNumber;
            AssignedWard = assignedWard;
            IntroduceString = string.Format(
                UiMessages.PatientObjectMessages.Introduce, name, surname, personalIdNumber, AssignedWard.Name);
        }
    }
}