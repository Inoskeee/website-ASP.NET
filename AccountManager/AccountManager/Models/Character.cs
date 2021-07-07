using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AccountManager.Models
{
    public class Character
    {
        [Key]
        public int CharacterId { get; set; }
        public int Score { get; set; }
        
        public int HeadId { get; set; }
        public Equipment Head { get; set; }
        public int BodyId { get; set; }
        public Equipment Body { get; set; }
        public int LegId { get; set; }
        public Equipment Leg { get; set; }
        public int SwordId { get; set; }
        public Sword Sword { get; set; }
        public int BowId { get; set; }
        public Bow Bow { get; set; }
    }
}