namespace MineralResources.Models
{
    public class Player
    {
        public virtual uint Id { get; set; }
        public virtual string Name { get; set; }
        public virtual IList<PlayerItem> PlayerItems { get; set; }

        public Player()
        {
            PlayerItems = new List<PlayerItem>();
        }
    }
}
