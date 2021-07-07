using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AccountManager.Models
{
    public class Bow
    {
        [Key]
        public int BowId { get; set; }
        public string BowName { get; set; }
        public int BowDamage { get; set; }
        public int Arrows { get; set; }
    }
}