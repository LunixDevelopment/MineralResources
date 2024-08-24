namespace MineralResources.Models
{
    public class Surface
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Hash { get; set; }
        public virtual IList<Item> Items { get; set; }

        public Surface()
        {
            Items = new List<Item>();
        }
    }
}