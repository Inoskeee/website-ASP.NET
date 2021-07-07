using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AccountManager.Models
{
    public class Clan
    {
        [Key]
        public int ClanId { get; set; }
        public string ClanName { get; set; }
        public int ClanRaiting { get; set; }
        public Player ClanLeader { get; set; }

        public List<PlayerClan> Followers { get; set; }
    }
}