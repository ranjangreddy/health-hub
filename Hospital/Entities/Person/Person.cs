using Hospital.Entities.Interfaces;
using Hospital.Enums;

namespace Hospital.PeopleCategories.PersonClass
{
    public abstract class Person : IIntroduceString, IIdentifier, IIsDeleted
    {
        public virtual int Id { get; set; }
        public virtual string IntroduceString { get; set; }
        public virtual string Name { get; set; }
        public virtual string Surname { get; set; }
        public virtual Gender Gender { get; set; }
        public virtual DateTime Birthday { get; set; }
        public virtual bool IsDeleted { get; set; }

        protected Person() { }

        protected Person(
            string name,
            string surname,
            Gender gender,
            DateTime birthday)
        {
            Name = name;
            Surname = surname;
            Gender = gender;
            Birthday = birthday;
            IsDeleted = false;
        }
    }
}