using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AccountManager.Models
{
    public class PlayerContext:DbContext
    {
        public PlayerContext():base("DefaultConnection") { }

        public DbSet<Player> Players { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<Sword> Swords { get; set; }
        public DbSet<Bow> Bows { get; set; }
        public DbSet<BodyPart> BodyParts { get; set; }
        public DbSet<PlayerClan> PlayerClans { get; set; }
        public DbSet<Clan> Clans { get; set; }
    }

    public class PlayerDbInitializer : DropCreateDatabaseAlways<PlayerContext>
    {
        protected override void Seed(PlayerContext db)
        {
            db.BodyParts.Add(new BodyPart { BodyPartName = "head" });
            db.BodyParts.Add(new BodyPart { BodyPartName = "body" });
            db.BodyParts.Add(new BodyPart { BodyPartName = "leg" });

            db.Equipments.Add(new Equipment { BodyPartId = 1, ItemName = "Стальной шлем", Armor = 5});
            db.Equipments.Add(new Equipment { BodyPartId = 2, ItemName = "Стальная броня", Armor = 10});
            db.Equipments.Add(new Equipment { BodyPartId = 3, ItemName = "Стальные ботинки", Armor = 5});
            db.Equipments.Add(new Equipment { BodyPartId = 1, ItemName = "Самурайский шлем", Armor = 8});
            db.Equipments.Add(new Equipment { BodyPartId = 2, ItemName = "Латный нагрудник", Armor = 15});
            db.Equipments.Add(new Equipment { BodyPartId = 3, ItemName = "Латные ботинки", Armor = 8});

            db.Swords.Add(new Sword { SwordName = "Стальной меч", Damage = 25 });
            db.Swords.Add(new Sword { SwordName = "Стальной топор", Damage = 20 });

            db.Bows.Add(new Bow { BowName = "Стандартный лук", BowDamage = 25, Arrows = 50 });
            db.Bows.Add(new Bow { BowName = "Улучшенный лук", BowDamage = 35, Arrows = 100 });

            base.Seed(db);
        }
    }
}