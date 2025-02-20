using Hospital.Enums;
using Hospital.PeopleCategories.PatientClass;
using Hospital.PeopleCategories.PersonClass;
using Hospital.PeopleCategories.WardClass;
using Hospital.Utilities.UserInterface;

namespace Hospital.Entities.Employee
{
    public class Employee : Person
    {
        public virtual int Id { get; set; }
        public virtual Ward AssignedWard { get; set; }
        public virtual Position Position { get; }
        public virtual IList<Patient> AssignedPatients { get; set; }

        protected Employee() { }

        public Employee(
            string name,
            string surname,
            Gender gender,
            DateTime birthday,
            Ward ward,
            Position position,
            IList<Patient> assignedPatients)
            : base(name,
                  surname,
                  gender,
                  birthday)
        {
            AssignedWard = ward;
            Position = position;
            AssignedPatients = assignedPatients;
            IntroduceString = string.Format(
                UiMessages.DoctorObjectMessages.Introduce, name, surname, Position, AssignedWard.Name);
        }
    }
}