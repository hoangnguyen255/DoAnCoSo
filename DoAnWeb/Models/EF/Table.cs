using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnCoSo.Models.EF
{
    [Table("table_Table")]
    public class Table : CommonAbstracts
    {
        public Table()
        {
            this.TableImages = new HashSet<TableImage>();
            this.OrderDetails = new HashSet<OrderDetail>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        [StringLength(250)]
        public string title { get; set; }
        public string alias { get; set; }
        public string tablecode { get; set; }
        public string description { get; set; }
        [AllowHtml]
        public string detail { get; set; }
        [StringLength(250)]
        public string image { get; set; }
        public int quantity { get; set; }
        public int spaceid { get; set; }
        [StringLength(250)]
        public string seotitle { get; set; }
        [StringLength(500)]
        public string seodescription { get; set; }
        [StringLength(250)]
        public string seokeywords { get; set; }
        public virtual Space Space { get; set; }
        public virtual ICollection<TableImage> TableImages { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}