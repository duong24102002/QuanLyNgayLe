using System.ComponentModel;

namespace Hinet.Service.QLNgayLeService.Dto
{
    public class QLNgayLeExportDto
    {
        [DisplayName("Ngày Lễ")]
        public string TenNgayLe { get; set; }
        [DisplayName("Thời Gian Lễ")]
        public string NgayLetxt { get; set; }

    }
}