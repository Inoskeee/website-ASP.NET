using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AccountManager.Models
{
    public class BodyPart
    {
        [Key]
        public int BodyPartId { get; set; }
        public string BodyPartName { get; set; }
    }
}