using AltV.Net;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;

namespace MineralResources
{
    public class Events : IScript
    {
        #region ScriptEvents
        [ScriptEvent(ScriptEventType.PlayerConnect)]
        public void OnPlayerConnect(IPlayer player, string reason)
        {
            player.Spawn(new AltV.Net.Data.Position(0, 0, 70), 50);
            player.Model = (uint)PedModel.Abigail;

            DatabaseHelper.AddPlayer(new Models.Player
            {
                Id = player.Id,
                Name = player.Name
            });
        }
        #endregion

        #region ClientEvents
        [ClientEvent("MineralResources.Event.PickUp")]
        public void OnPickUp(IPlayer player, uint materialHash)
        {
            PlayAnimation(player);

            var item = DatabaseHelper.GetRandomItemForSurface(materialHash);

            if (!DatabaseHelper.GivePlayerItem(player, item))
            {
                player.Emit("sendNotification", $"Du hast leider nichts gefunden!");
                return;
            }

            player.Emit("sendNotification", $"Du hast 1 {item.Name} gefunden!");
        }
        #endregion

        #region Methods
        private void PlayAnimation(IPlayer player)
        {
            var animDict = "amb@world_human_gardener_plant@male@base";
            var animName = "base";
            var blendInSpeed = 1.0f;
            var blendOutSpeed = 1.0f;
            var duration = 5 * 1000;
            var flags = 2;
            var playbackRate = 1.0f;
            var lockX = true;
            var lockY = true;
            var lockZ = true;

            player.PlayAnimation(animDict, animName, blendInSpeed, blendOutSpeed, duration, flags, playbackRate, lockX, lockY, lockZ);
        }
        #endregion
    }
}
