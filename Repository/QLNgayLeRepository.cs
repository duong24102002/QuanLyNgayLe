using Hinet.Model.Entities;
using System.Data.Entity;
using System.Linq;

namespace Hinet.Repository.QLNgayLeRepository
{
    public class QLNgayLeRepository : GenericRepository<QLNgayLe>, IQLNgayLeRepository
    {
        public QLNgayLeRepository(DbContext context)
            : base(context)
        {

        }
        public QLNgayLe GetById(long id)
        {
            return FindBy(x => x.Id == id).FirstOrDefault();
        }

    }
}
