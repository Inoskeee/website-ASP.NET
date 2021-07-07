using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AccountManager.Models
{
    public class Sword
    {
        [Key]
        public int SwordId { get; set; }
        public string SwordName { get; set; }
        public int Damage { get; set; }
    }
}