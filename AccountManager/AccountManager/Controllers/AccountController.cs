using AccountManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AccountManager.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                // поиск пользователя в бд
                Player player = null;
                using (PlayerContext db = new PlayerContext())
                {
                    var md5 = MD5.Create();
                    var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(model.Password));
                    string hashedPassword = Convert.ToBase64String(hash);
                    player = db.Players.FirstOrDefault(u => u.Username == model.Username && u.PasswordHash == hashedPassword);

                }
                if (player != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Username, true);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Игрока с таким ником не существует или пароль введен неверно.");
                }
            }

            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                Player player = null;
                using (PlayerContext db = new PlayerContext())
                {
                    player = db.Players.FirstOrDefault(u => u.Username == model.Username);
                }
                if (player == null)
                {
                    string cond = @"(\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)";
                    string email = model.Email;

                    if (Regex.IsMatch(email, cond))
                    {
                        if(model.Password.Length >= 6 && model.Password.Length <= 15)
                        {
                            // создаем нового пользователя
                            using (PlayerContext db = new PlayerContext())
                            {
                                var md5 = MD5.Create();
                                var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(model.Password));
                                string hashedPassword = Convert.ToBase64String(hash);


                                Player ourPlayer = new Player { Username = model.Username, PasswordHash = hashedPassword, Email = model.Email };
                                Character ourCharacter = new Character { Score = 0, HeadId = 1, BodyId = 2, LegId = 3, SwordId = 1, BowId = 1 };
                                
                                ourPlayer.Character = ourCharacter;
                                db.Players.Add(ourPlayer);
                                db.SaveChanges();

                                //db.Players.Where(u => u.Username == model.Username && u.PasswordHash == hashedPassword).FirstOrDefault().CharacterId = db.Players.Where(u => u.Username == model.Username && u.PasswordHash == hashedPassword).FirstOrDefault().Id;
                                //db.SaveChanges();

                                player = db.Players.Where(u => u.Username == model.Username && u.PasswordHash == hashedPassword).FirstOrDefault();
                            }
                            // если пользователь удачно добавлен в бд
                            if (player != null)
                            {
                                FormsAuthentication.SetAuthCookie(model.Username, true);
                                return RedirectToAction("Index", "Home");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Пароль должен быть от 6 до 15 символов.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Email введен некорректно, повторите ввод. Пример: temp@gmail.com");
                    }
                    
                }
                else
                {
                    ModelState.AddModelError("", "Игрок с таким логином уже существует");
                }
            }

            return View(model);
        }
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
        public ActionResult EditName()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditName(ChangeNameModel model)
        {
            if (ModelState.IsValid)
            {
                Player player = null;
                using (PlayerContext db = new PlayerContext())
                {
                    player = db.Players.FirstOrDefault(u => u.Username == model.NewUsername);
                }
                if (player == null)
                {
                    using (PlayerContext db = new PlayerContext())
                    {
                        player = db.Players.FirstOrDefault(u => u.Username == User.Identity.Name);
                        player.Username = model.NewUsername;
                        db.Entry(player).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    // если пользователь удачно добавлен в бд
                    if (player != null)
                    {
                        FormsAuthentication.SetAuthCookie(model.NewUsername, true);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Игрок с таким логином уже существует");
                }
            }

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public ActionResult EditEmail()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEmail(ChangeEmailModel model)
        {
            if (ModelState.IsValid)
            {
                string cond = @"(\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)";
                string email = model.NewEmail;

                if (Regex.IsMatch(email, cond))
                {
                    Player player = null;
                    using (PlayerContext db = new PlayerContext())
                    {
                        player = db.Players.FirstOrDefault(u => u.Username == User.Identity.Name);
                        player.Email = model.NewEmail;
                        db.Entry(player).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    if (player != null)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Еmail введен некорректно, повторите ввод. Пример: temp@gmail.com");
                }
            }
            return View(model);
        }

        [Authorize]
        [HttpGet]
        public ActionResult EditPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                Player player = null;
                using (PlayerContext db = new PlayerContext())
                {
                    player = db.Players.FirstOrDefault(u => u.Username == User.Identity.Name);
                    var md5 = MD5.Create();
                    var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(model.OldPassword));
                    string hashedPassword = Convert.ToBase64String(hash);
                    if (hashedPassword == player.PasswordHash)
                    {
                        if(model.NewPassword.Length >=6 && model.NewPassword.Length <= 15)
                        {
                            if (model.NewPassword == model.RetryPassword)
                            {
                                hash = md5.ComputeHash(Encoding.UTF8.GetBytes(model.NewPassword));
                                hashedPassword = Convert.ToBase64String(hash);
                                player.PasswordHash = hashedPassword;
                                db.Entry(player).State = EntityState.Modified;
                                db.SaveChanges();
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                ModelState.AddModelError("", "Пароли не совпадают, повторите попытку!");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Пароль должен быть от 6 до 15 символов.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Старый пароль введен неверно, повторите попытку!");
                    }
                }
            }

            return View(model);
        }
    }
}