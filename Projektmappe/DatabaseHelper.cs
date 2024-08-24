using AltV.Net;
using AltV.Net.Elements.Entities;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using MineralResources.Models;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using Player = MineralResources.Models.Player;

namespace MineralResources
{
    public static class DatabaseHelper
    {
        private static readonly ISessionFactory _sessionFactory;

        #region ctor
        static DatabaseHelper()
        {
            try
            {
                _sessionFactory = Fluently.Configure()
                    .Database(MySQLConfiguration.Standard.ConnectionString("Server=localhost;Database=altvtest;User Id=root;Password=;"))
                    .Mappings(m =>
                    {
                        m.FluentMappings.AddFromAssemblyOf<ItemMap>();
                        m.FluentMappings.AddFromAssemblyOf<SurfaceMap>();
                        m.FluentMappings.AddFromAssemblyOf<PlayerMap>();
                        m.FluentMappings.AddFromAssemblyOf<PlayerItemMap>();
                    })
                    .ExposeConfiguration(cfg =>
                    {
                        var schemaUpdate = new SchemaUpdate(cfg);
                        schemaUpdate.Execute(true, true);
                    })
                    .BuildSessionFactory();
            }
            catch (Exception ex)
            {
                Alt.Log(ex.Message);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gibt ein zufälliges Item aus dem jeweiligen Untergrund/Material zurück
        /// </summary>
        /// <param name="materialHash">Hashwert (Dezimal)</param>
        /// <remarks>TODO: Evtl. Wahrscheinlichkeit hinzufügen, um nicht immer etwas zu finden?</remarks>
        /// <returns><paramref name="Item"/> aus Material <paramref name="materialHash"/></returns>
        public static Item? GetRandomItemForSurface(uint materialHash)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                var materialHashHex = $"0x{materialHash:X}";
                var material = (MaterialHash)materialHash;
                var surface = session.Query<Surface>().FirstOrDefault(s => s.Hash == materialHashHex);

                // Schreibe Material in die Datenbank, falls nicht vorhanden
                // --> Tabelle "Surface" baut sich so selbst auf
                if (surface == null)
                {
                    surface = new Surface { Hash = materialHashHex, Name = material.ToString() };
                    session.Save(surface);
                }

                var items = session.Query<Item>().Where(i => i.Surfaces.Contains(surface)).ToList();

                if (!items.Any())
                    return null;

                var random = new Random();
                var randomIndex = random.Next(1, items.Count);

                return items[randomIndex];
            }
        }

        /// <summary>
        /// Schreibe Item für den jeweiligen Spieler in die Datenbank.
        /// Neuer Eintrag, wenn keiner vorhanden. Andernfalls update.
        /// </summary>
        /// <param name="player">Spieler, der das Item erhalten soll</param>
        /// <param name="item">Das Item, das der Spieler erhalten soll</param>
        /// <param name="quantity">Anzahl der zu erhaltenen Items</param>
        /// <returns><c>true</c>, wenn erfolgreich. Ansonst <c>false</c></returns>
        public static bool GivePlayerItem(IPlayer player, Item? item, int quantity = 1)
        {
            try
            {
                if (item == null)
                    return false;

                using (var session = _sessionFactory.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        var playerFromDb = session.Get<Player>(player.Id);

                        if (playerFromDb == null)
                        {
                            Alt.Log($"Player {player.Id} not found");
                            return false;
                        }

                        var itemFromDb = session.Get<Item>(item.Id);

                        if (itemFromDb == null)
                        {
                            Alt.Log($"Item {item.Name} ({item.Id}) not found");
                            return false;
                        }

                        var playerItem = session.Query<PlayerItem>().FirstOrDefault(p => p.Item.Id == itemFromDb.Id && player.Id == playerFromDb.Id);

                        // Wenn vorhanden, nur Quanitity addieren
                        if (playerItem != null)
                        {
                            playerItem.Quantity += quantity;
                        }
                        else
                        {
                            playerItem = new PlayerItem
                            {
                                Player = playerFromDb,
                                Item = item,
                                Quantity = quantity
                            };
                        }

                        session.Save(playerItem);
                        transaction.Commit();

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Alt.Log(ex.Message);
                return false;
            }
        }
        #endregion

        #region Mock
        public static void AddPlayer(Player player)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                // Prüfen, ob schon in DB
                if (session.Query<Player>().FirstOrDefault(p => p.Name == player.Name) != null)
                    return;

                using (var transaction = session.BeginTransaction())
                {
                    session.Save(player);
                    transaction.Commit();
                }
            }
        }
        #endregion
    }
}