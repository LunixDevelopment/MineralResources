namespace MineralResources.Models
{
    public class Item
    {
        public virtual uint Id { get; set; }
        public virtual string Name { get; set; }
        public virtual IList<PlayerItem> PlayerItems { get; set; }
        public virtual IList<Surface> Surfaces { get; set; }

        public Item()
        {
            PlayerItems = new List<PlayerItem>();
            Surfaces = new List<Surface>();
        }
    }
}
