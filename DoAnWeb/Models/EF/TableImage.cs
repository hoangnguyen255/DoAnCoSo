using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DoAnCoSo.Models.EF
{
    [Table("table_TableImage")]
    public class TableImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int tableid { get; set; }
        public string image { get; set; }
        public bool isdefault { get; set; }
        public virtual Table Table { get; set; }

    }
}