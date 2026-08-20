using Hinet.Model.Entities;

namespace Hinet.Repository.QLNgayLeRepository
{
    public interface IQLNgayLeRepository : IGenericRepository<QLNgayLe>
    {
        QLNgayLe GetById(long id);

    }

}
