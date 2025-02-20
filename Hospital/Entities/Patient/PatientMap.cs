using FluentNHibernate.Mapping;

namespace Hospital.PeopleCategories.PatientClass
{
    internal class PatientMap : ClassMap<Patient>
    {
        public PatientMap()
        {
            Id(x => x.Id).Not.Nullable();
            Map(x => x.Name).Not.Nullable();
            Map(x => x.Surname).Not.Nullable();
            Map(x => x.Gender).Not.Nullable();
            Map(x => x.Birthday).CustomType<DateTime>().Not.Nullable();
            Map(x => x.IsDeleted).CustomType<bool>().Not.Nullable();
            Map(x => x.PersonalIdNumber).Not.Nullable();
            Map(x => x.IntroduceString).Not.Nullable();
            Map(x => x.HealthStatus);

            References(x => x.AssignedWard);

            References(x => x.AssignedDoctor);
        }
    }
}