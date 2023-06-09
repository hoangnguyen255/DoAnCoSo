using DoAnCoSo.Models.EF;
using DoAnCoSo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnCoSo.Models.EF
{
    [Table("table_Space")]
    public class Space 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        [StringLength(250)]
        public string title { get; set; }
        public string alias { get; set; }
 
        public string description { get; set; }
        [AllowHtml]
        public string detail { get; set; }
        [StringLength(250)]
        public string image { get; set; }
 
    }
}