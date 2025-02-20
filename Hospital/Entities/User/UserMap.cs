using FluentNHibernate.Mapping;

namespace Hospital.PeopleCategories.UserClass
{
    internal class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Id(x => x.Id).Not.Nullable();
            Map(x => x.Name).Not.Nullable();
            Map(x => x.Surname).Not.Nullable();
            Map(x => x.Gender).Not.Nullable();
            Map(x => x.Birthday).CustomType<DateTime>().Not.Nullable();
            Map(x => x.IsDeleted).CustomType<bool>().Not.Nullable();
            Map(x => x.Login).Not.Nullable();
            Map(x => x.Password).Not.Nullable();
            Map(x => x.Rank).Not.Nullable();
            Map(x => x.IntroduceString).Not.Nullable();

            HasManyToMany(x => x.AssignedWards)
                .Cascade.All()
                .Inverse()
                .Table("User_Ward");
        }
    }
}