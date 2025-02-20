using FluentNHibernate.Mapping;

namespace Hospital.PeopleCategories.WardClass
{
    internal class WardMap : ClassMap<Ward>
    {
        public WardMap()
        {
            Id(x => x.Id).GeneratedBy.Identity().Not.Nullable();
            Map(x => x.Name).Unique().Not.Nullable();
            Map(x => x.Capacity).Not.Nullable();
            Map(x => x.PatientsNumber).Not.Nullable();
            Map(x => x.IsDeleted).CustomType<bool>().Not.Nullable();
            Map(x => x.IntroduceString).Not.Nullable();

            HasMany(x => x.AssignedPatients)
                .Inverse();

            HasMany(x => x.AssignedEmployees)
                .Inverse()
                .KeyColumn("WardId");

            HasManyToMany(x => x.AssignedUsers)
                .Cascade.All()
                .Table("User_Ward");
        }
    }
}