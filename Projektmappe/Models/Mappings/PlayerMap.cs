using FluentNHibernate.Mapping;

namespace MineralResources.Models
{
    public class PlayerMap : ClassMap<Player>
    {
        public PlayerMap()
        {
            Table("players");

            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Name);
            HasMany(x => x.PlayerItems)
                .Inverse()
                .Cascade.All();
        }
    }
}
