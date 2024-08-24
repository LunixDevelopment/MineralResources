using FluentNHibernate.Mapping;

namespace MineralResources.Models
{
    public class SurfaceMap : ClassMap<Surface>
    {
        public SurfaceMap()
        {
            Table("surfaces");

            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Name).Nullable();
            Map(x => x.Hash).Not.Nullable();

            HasManyToMany(x => x.Items)
                .Cascade.All()
                .Table("surface_items")
                .ParentKeyColumn("surface_id")
                .ChildKeyColumn("item_id");
        }
    }
}
