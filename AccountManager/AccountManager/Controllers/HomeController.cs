using AccountManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccountManager.Controllers
{
    public class HomeController : Controller
    {
        PlayerContext db = new PlayerContext();

        [Authorize]
        public ActionResult Index()
        {
            Player player = db.Players.Include("Character").Where(u => u.Username == User.Identity.Name).FirstOrDefault();
            ViewBag.Player = player;
            player.Character.Head = db.Equipments.Where(u => u.ItemId == player.Character.HeadId).FirstOrDefault();
            player.Character.Body = db.Equipments.Where(u => u.ItemId == player.Character.BodyId).FirstOrDefault();
            player.Character.Leg = db.Equipments.Where(u => u.ItemId == player.Character.LegId).FirstOrDefault();
            player.Character.Sword = db.Swords.Where(u => u.SwordId == player.Character.SwordId).FirstOrDefault();
            player.Character.Bow = db.Bows.Where(u => u.BowId == player.Character.BowId).FirstOrDefault();
            ViewBag.Character = player.Character;

            Clan clan = db.PlayerClans.Where(u => u.PlayerId == player.Id).Select(i => i.ClanId).FirstOrDefault();
            //ViewBag.Clan = db.Clans.Where(u => u.ClanId == clanId).FirstOrDefault();
            foreach (Player v in db.Players) { }
            foreach (PlayerClan v in db.PlayerClans) { }
            ViewBag.Clan = clan;
            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult EditHead()
        {
            var heads = db.Equipments.Include("BodyPart");
            //ViewBag.Head = heads;
            Player player = db.Players.Include("Character").Where(u => u.Username == User.Identity.Name).FirstOrDefault();
            ViewBag.Player = player;
            return View(heads);
        }

        [Authorize]
        [HttpGet]
        public ActionResult EditHeadNow(int id)
        {
            Player player = db.Players.Include("Character").Where(u => u.Username == User.Identity.Name).FirstOrDefault();
            player.Character.HeadId = id;
            db.Entry(player).State = EntityState.Modified;
            db.SaveChanges();
            return Redirect("Index");
        }

        [Authorize]
        [HttpGet]
        public ActionResult EditBody()
        {
            var bodies = db.Equipments.Include("BodyPart");
            //ViewBag.Head = heads;
            Player player = db.Players.Include("Character").Where(u => u.Username == User.Identity.Name).FirstOrDefault();
            ViewBag.Player = player;
            return View(bodies);
        }

        [Authorize]
        [HttpGet]
        public ActionResult EditBodyNow(int id)
        {
            Player player = db.Players.Include("Character").Where(u => u.Username == User.Identity.Name).FirstOrDefault();
            player.Character.BodyId = id;
            db.Entry(player).State = EntityState.Modified;
            db.SaveChanges();
            return Redirect("Index");
        }

        [Authorize]
        [HttpGet]
        public ActionResult EditLeg()
        {
            var legs = db.Equipments.Include("BodyPart");
            //ViewBag.Head = heads;
            Player player = db.Players.Include("Character").Where(u => u.Username == User.Identity.Name).FirstOrDefault();
            ViewBag.Player = player;
            return View(legs);
        }

        [Authorize]
        [HttpGet]
        public ActionResult EditLegNow(int id)
        {
            Player player = db.Players.Include("Character").Where(u => u.Username == User.Identity.Name).FirstOrDefault();
            player.Character.LegId = id;
            db.Entry(player).State = EntityState.Modified;
            db.SaveChanges();
            return Redirect("Index");
        }

        [Authorize]
        [HttpGet]
        public ActionResult EditSword()
        {
            var swords = db.Swords;
            //ViewBag.Head = heads;
            Player player = db.Players.Include("Character").Where(u => u.Username == User.Identity.Name).FirstOrDefault();
            ViewBag.Player = player;
            return View(swords);
        }

        [Authorize]
        [HttpGet]
        public ActionResult EditSwordNow(int id)
        {
            Player player = db.Players.Include("Character").Where(u => u.Username == User.Identity.Name).FirstOrDefault();
            player.Character.SwordId = id;
            db.Entry(player).State = EntityState.Modified;
            db.SaveChanges();
            return Redirect("Index");
        }

        [Authorize]
        [HttpGet]
        public ActionResult EditBow()
        {
            var bows = db.Bows;
            //ViewBag.Head = heads;
            Player player = db.Players.Include("Character").Where(u => u.Username == User.Identity.Name).FirstOrDefault();
            ViewBag.Player = player;
            return View(bows);
        }

        [Authorize]
        [HttpGet]
        public ActionResult EditBowNow(int id)
        {
            Player player = db.Players.Include("Character").Where(u => u.Username == User.Identity.Name).FirstOrDefault();
            player.Character.BowId = id;
            db.Entry(player).State = EntityState.Modified;
            db.SaveChanges();
            return Redirect("Index");
        }
    }
}