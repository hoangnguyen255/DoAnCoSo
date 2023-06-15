using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnCoSo.Models.EF
{
    public class Employee : CommonAbstracts
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required(ErrorMessage = "Bạn không thể trống tên nhân viên")]
        [StringLength(150)]
        public string name { get; set; }
        public string image { get; set; }
        public string gioitinh { get; set; }
        public string diachi { get; set; }
        public DateTime ngaysinh { get; set; }
        public string chucvu { get; set; }
 
    }
}