﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DoAnCoSo.Models.EF
{
    [Table("table_OrderDetail")]
    public class OrderDetail 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int orderid { get; set; }


        public int? productid { get; set; }
        public int? tableid { get; set; }

        public decimal price { get; set; }
        public int quantity { get; set; }
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
        public virtual Table Table { get; set; }
        public virtual Space Space { get; set; }

    }
}