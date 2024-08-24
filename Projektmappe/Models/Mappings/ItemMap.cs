using FluentNHibernate.Mapping;

namespace MineralResources.Models
{
    public class ItemMap : ClassMap<Item>
    {
        public ItemMap()
        {
            Table("items");

            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Name).Not.Nullable();

            HasManyToMany(x => x.Surfaces)
                .Cascade.All()
                .Table("surface_items")
                .ParentKeyColumn("item_id")
                .ChildKeyColumn("surface_id");

            HasMany(x => x.PlayerItems)
                .Cascade.All()
                .Inverse()
                .KeyColumn("ItemId");
        }
    }
}
