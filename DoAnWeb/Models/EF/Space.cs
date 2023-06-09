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
    public class Space : CommonAbstracts
    {
        public Space()
        {
            this.Tables = new HashSet<Table>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        [StringLength(150)]
        public string title { get; set; }
        [Required]
        [StringLength(150)]
        public string alias { get; set; }
        [StringLength(250)]
        public string description { get; set; }
        [StringLength(250)]
        public string seotitle { get; set; }
        [StringLength(500)]
        public string seodescription { get; set; }
        [StringLength(250)]
        public string seokeywords { get; set; }

        public string icon { get; set; }
        public ICollection<Table> Tables { get; set; }

    }
}