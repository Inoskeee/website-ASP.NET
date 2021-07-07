using AccountManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;

namespace AccountManager.Controllers
{
    public class ClanController : Controller
    {
        PlayerContext db = new PlayerContext();

        [Authorize]
        [HttpGet]
        public ActionResult ClanList()
        {
            List<Clan> clans = db.PlayerClans.Select(i => i.ClanId).ToList();

            foreach (Player v in db.Players) { }
            foreach (PlayerClan v in db.PlayerClans) { }

            for(int i = 0; i < clans.Count; i++)
            {
                Clan curClan = clans[i];
                List<Player> allPlayers = db.Players.Include("Character").ToList();
                List<Player> players = new List<Player>();
                int clanRaiting = 0;

                foreach (PlayerClan q in db.PlayerClans)
                {
                    if (q.ClanId == curClan)
                    {
                        players.Add(allPlayers.Where(u => u.Id == q.PlayerId).FirstOrDefault());
                        clanRaiting += allPlayers.Where(u => u.Id == q.PlayerId).FirstOrDefault().Character.Score;
                    }
                }

                if (clanRaiting != curClan.ClanRaiting)
                {
                    curClan.ClanRaiting = clanRaiting;
                }
                db.Entry(curClan).State = EntityState.Modified;
                db.SaveChanges();
                clans[i] = curClan;
            }

            return View(clans);
        }

        [Authorize]
        [HttpGet]
        public ActionResult ClanJoinNow(int id)
        {
            Player player = db.Players.Include("Character").Where(u => u.Username == User.Identity.Name).FirstOrDefault();
            Clan clan = db.Clans.Where(u => u.ClanId == id).FirstOrDefault();
            PlayerClan pc = new PlayerClan { ClanId = clan, PlayerId = player.Id };
            clan.ClanRaiting += player.Character.Score;
            db.Entry(clan).State = EntityState.Modified;
            db.PlayerClans.Add(pc);
            db.SaveChanges();
            return Redirect("../Home");
        }


        [Authorize]
        [HttpGet]
        public ActionResult ClanCreate()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ClanCreate(ClanCreateModel model)
        {
            if (ModelState.IsValid)
            {
                Player player = null;
                Clan clan = null;
                using (PlayerContext db = new PlayerContext())
                {
                    clan = db.Clans.FirstOrDefault(u => u.ClanName == model.ClanName);
                }
                if (clan == null)
                {
                    using (PlayerContext db = new PlayerContext())
                    {
                        player = db.Players.Include("Character").FirstOrDefault(u => u.Username == User.Identity.Name);
                        clan = new Clan 
                        { 
                            ClanName = model.ClanName, 
                            ClanLeader = player, 
                            ClanRaiting = player.Character.Score, 
                            Followers = new List<PlayerClan> { new PlayerClan { PlayerId = player.Id, ClanId = clan } } 
                        };
                        db.Clans.Add(clan);
                        db.SaveChanges();

                        /*int clanId = db.Clans.FirstOrDefault(u=>u.ClanName == model.ClanName).ClanId;
                        db.PlayerClans.Add(new PlayerClan { ClanId = clanId, PlayerId = player.Id });
                        db.SaveChanges();*/
                    }
                    // если пользователь удачно добавлен в бд
                    if (clan != null)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Клан с таким названием уже существует");
                }
            }

            return View(model);
        }


        [Authorize]
        [HttpGet]
        public ActionResult ClanDelete(int id)
        {
            Clan clan = db.Clans.Where(u => u.ClanId == id)
                .FirstOrDefault();
            foreach (PlayerClan v in db.PlayerClans)
            {
                if(clan == v.ClanId)
                {
                    db.Entry(v).State = EntityState.Deleted;
                }
            }
            db.Entry(clan).State = EntityState.Deleted;
            db.SaveChanges();
            return Redirect("../Home");
        }

        [Authorize]
        [HttpGet]
        public ActionResult ClanLeave()
        {
            Player player = db.Players.Include("Character").Where(u => u.Username == User.Identity.Name).FirstOrDefault();
            PlayerClan clanPlayer = db.PlayerClans.Where(u => u.PlayerId == player.Id).FirstOrDefault();

            foreach (Clan f in db.Clans) { }
            Clan clan = clanPlayer.ClanId;
            //Clan clan = db.Clans.Where(u => u.ClanId == id).FirstOrDefault();
            foreach (PlayerClan v in db.PlayerClans)
            {
                if (player.Id == v.PlayerId)
                {
                    clan.ClanRaiting -= player.Character.Score;
                    db.Entry(clan).State = EntityState.Modified;
                    db.Entry(v).State = EntityState.Deleted;
                }
            }
            db.SaveChanges();
            return Redirect("../Home");
        }

        [Authorize]
        [HttpGet]
        public ActionResult ClanInfo(int id)
        {
            Clan clans = db.Clans.Where(u => u.ClanId == id).FirstOrDefault();
            List<Player> allPlayers = db.Players.Include("Character").ToList();
            List<Player> players = new List<Player>();
            int clanRaiting = 0;

            foreach (PlayerClan v in db.PlayerClans)
            {
                if(v.ClanId == clans)
                {
                    players.Add(allPlayers.Where(u => u.Id == v.PlayerId).FirstOrDefault());
                    clanRaiting += allPlayers.Where(u => u.Id == v.PlayerId).FirstOrDefault().Character.Score;
                }
            }

            if(clans.ClanLeader.Username == User.Identity.Name)
            {
                ViewBag.isLeader = true;
            }
            else
            {
                ViewBag.isLeader = false;
            }

            if(clanRaiting != clans.ClanRaiting)
            {
                clans.ClanRaiting = clanRaiting;
            }

            ViewBag.Clan = clans;

            return View(players);
        }
        [Authorize]
        [HttpGet]
        public ActionResult ClanExpel(int id, int clanId)
        {
            foreach (PlayerClan v in db.PlayerClans)
            {
                if (id == v.PlayerId)
                {
                    db.Entry(v).State = EntityState.Deleted;
                }
            }

            db.SaveChanges();
            return Redirect($"../Clan/ClanInfo?id={clanId}");
        }

        [Authorize]
        [HttpGet]
        public ActionResult ClanInvite()
        {
            ViewBag.Message = null;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ClanInvite(ClanInviteModel model)
        {
            if (ModelState.IsValid)
            {
                List<Player> allPlayers = db.Players.Include("Character").ToList();
                Player player = db.Players.Where(u => u.Username == model.Username).FirstOrDefault();
                if(player != null)
                {
                    Clan clan = db.Clans.Where(u => u.ClanLeader.Username == User.Identity.Name).FirstOrDefault();
                    MailMessage message = new MailMessage();
                    message.Body = $"Здравствуйте {player.Username}! Вы получили приглашение в клан {clan.ClanName}. Ознакомьтесь с информацией о клане и примите решение:";
                    message.Body += $"\nЛидер клана: {clan.ClanLeader.Username}.\nРейтинг клана: {clan.ClanRaiting}.\nУчастники клана:";
                    foreach (PlayerClan v in db.PlayerClans)
                    {
                        if (clan == v.ClanId)
                        {
                            message.Body += $"\n{allPlayers.Where(u => u.Id == v.PlayerId).FirstOrDefault().Username}";
                        }
                    }
                    message.From = new MailAddress("nvpopov.me@gmail.com");
                    message.To.Add(player.Email);
                    message.BodyEncoding = System.Text.Encoding.UTF8;

                    SmtpClient client = new SmtpClient();
                    client.Host = "smtp.gmail.com";
                    client.Port = 587;
                    client.Credentials = new NetworkCredential(message.From.Address, "bmpkfpzbgfjymrxt");
                    client.EnableSsl = true;
                    ServicePointManager.ServerCertificateValidationCallback =
                        delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                        { return true; };
                    client.Send(message);
                    ViewBag.Message = "Приглашение успешно отправлено!";
                }
                else
                {
                    ModelState.AddModelError("", "Такой игрок не существует");
                }
            }
            return View(model);
        }

    }
}