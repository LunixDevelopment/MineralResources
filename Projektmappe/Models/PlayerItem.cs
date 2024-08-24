namespace MineralResources.Models
{
    public class PlayerItem
    {
        public virtual int Id { get; set; }
        public virtual Player Player { get; set; }
        public virtual Item Item { get; set; }
        public virtual int Quantity { get; set; }
    }
} 