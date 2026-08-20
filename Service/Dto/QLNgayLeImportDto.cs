using System;
using System.ComponentModel;
namespace Hinet.Service.QLNgayLeService.Dto
{
    public class QLSuKienImportDto
    {
        [DisplayName("Ngày Lễ")]
        public string TenNgayLe { get; set; }
        [DisplayName("Thời Gian Lễ")]
        public DateTime? NgayLe { get; set; }

    }
}