using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hinet.Model.Entities
{
    [Table("QLNgayLe")]
    public class QLNgayLe : AuditableEntity<long>
    {
        public string TenNgayLe { get; set; }
        public DateTime? NgayLe { get; set; }
    }
}
