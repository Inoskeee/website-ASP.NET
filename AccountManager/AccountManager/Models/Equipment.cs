using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AccountManager.Models
{
    public class Equipment
    {
        [Key]
        public int ItemId { get; set; }
        public int BodyPartId { get; set; }
        public BodyPart BodyPart { get; set; }
        public string ItemName { get; set; }
        public int Armor { get; set; }
    }
}