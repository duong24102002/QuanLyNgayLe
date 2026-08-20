using Hinet.Model.Entities;
using Hinet.Service.Common;
using Hinet.Service.QLNgayLeService.Dto;
using System.Collections.Generic;

namespace Hinet.Service.QLNgayLeService
{
    public interface IQLNgayLeService : IEntityService<QLNgayLe>
    {
        PageListResultBO<QLNgayLeDto> GetDaTaByPage(QLNgayLeSearchDto searchModel, int pageIndex = 1, int pageSize = 20);
        QLNgayLe GetById(long id);
        List<QLNgayLeDto> GetDaTaListNgayLe(long id);
        List<QLNgayLe> GetNgayLeCuaThang(int month, int year);
    }
}
