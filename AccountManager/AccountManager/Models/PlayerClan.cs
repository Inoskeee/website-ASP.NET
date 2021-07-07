using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AccountManager.Models
{
    public class PlayerClan
    {
        [Key]
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public Clan ClanId { get; set; }
    }
}