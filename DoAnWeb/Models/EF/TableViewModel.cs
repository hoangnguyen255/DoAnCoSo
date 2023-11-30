using DoAnCoSo.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAnCoSo.Models.EF
{
    public class TableViewModel
    {
        public Table table { get; set; }
        public bool isActive { get; set; } = true;
    }
}