using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DoAnCoSo.Models.EF
{
    [Table("table_SystemSetting")]
    public class Systemsetting
    {
        [Key]
        [StringLength(100)]
        public string settingkey { get; set; }
        [StringLength(1000)]
        public string settingvalue { get; set; }
        public string settingdescription { get; set; }
    }
}