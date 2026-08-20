using Hinet.Service.Common;
using System;

namespace Hinet.Service.QLNgayLeService.Dto
{
    public class QLNgayLeSearchDto : SearchBase
    {
        public string TenNgayLeFilter { get; set; }
        public DateTime? NgayLeFilter { get; set; }
        public DateTime? NgayLeFilterTo { get; set; }


    }
}