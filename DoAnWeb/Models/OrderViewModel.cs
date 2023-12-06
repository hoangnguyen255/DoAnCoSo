using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DoAnCoSo.Models
{
    public class OrderViewModel
    {
        [Required(ErrorMessage = "Tên khách hàng không được để trống")]
        public string customername { get; set; }
        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        public string phone { get; set; }
        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        public string address { get; set; }
        [Required(ErrorMessage = "Ngày đặt bàn không được để trống")]
        public DateTime datetime { get; set; } = DateTime.Now;
        public string email { get; set; }
        public int typepayment { get; set; }
        public int typepaymentvn { get; set; }

    }
}