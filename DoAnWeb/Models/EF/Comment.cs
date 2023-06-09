using DoAnCoSo.Models.EF;
using DoAnCoSo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DoAnCoSo.Models.EF
{
    [Table("table_Comment")]
    public class Comment : CommonAbstracts
    {
        public Comment()
        {
            this.Products = new HashSet<Product>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required(ErrorMessage = "Tên  không được để trống")]
        [StringLength(150)]
        public string name { get; set; }
        [Required(ErrorMessage = "Email không được để trống")]
        public string Email { get; set; }
        public string productid { get; set; }
        public string productname { get; set; }
        public int star { get; set; }
        public string message { get; set; }
        public bool isactive { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}