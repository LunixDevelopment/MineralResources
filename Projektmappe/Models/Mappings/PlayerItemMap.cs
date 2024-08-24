using FluentNHibernate.Mapping;

namespace MineralResources.Models
{
    public class PlayerItemMap : ClassMap<PlayerItem>
    {
        public PlayerItemMap()
        {
            Table("player_items");

            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.Player)
                .Column("PlayerId")
                .Not.Nullable();

            References(x => x.Item)
                .Column("ItemId")
                .Not.Nullable();

            Map(x => x.Quantity).Not.Nullable();
        }
    }
}
