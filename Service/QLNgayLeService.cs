using AutoMapper;
using Hinet.Model.Entities;
using Hinet.Repository;
using Hinet.Repository.QLNgayLeRepository;
using Hinet.Service.Common;
using Hinet.Service.QLNgayLeService.Dto;
using log4net;
using PagedList;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace Hinet.Service.QLNgayLeService
{
    public class QLNgayLeService : EntityService<QLNgayLe>, IQLNgayLeService
    {
        IUnitOfWork _unitOfWork;
        IQLNgayLeRepository _QLNgayLeRepository;
        ILog _loger;
        IMapper _mapper;



        public QLNgayLeService(IUnitOfWork unitOfWork,
            IQLNgayLeRepository QLNgayLeRepository,
            ILog loger,
            IMapper mapper
            )
            : base(unitOfWork, QLNgayLeRepository)
        {
            _unitOfWork = unitOfWork;
            _QLNgayLeRepository = QLNgayLeRepository;
            _loger = loger;
            _mapper = mapper;



        }

        public PageListResultBO<QLNgayLeDto> GetDaTaByPage(QLNgayLeSearchDto searchModel, int pageIndex = 1, int pageSize = 20)
        {
            var query = from QLNgayLetbl in _QLNgayLeRepository.GetAllAsQueryable()
                        select new QLNgayLeDto
                        {
                            TenNgayLe = QLNgayLetbl.TenNgayLe,
                            NgayLe = QLNgayLetbl.NgayLe,
                            CreatedBy = QLNgayLetbl.CreatedBy,
                            UpdatedBy = QLNgayLetbl.UpdatedBy,
                            CreatedDate = QLNgayLetbl.CreatedDate,
                            UpdatedDate = QLNgayLetbl.UpdatedDate,
                            DeleteTime = QLNgayLetbl.DeleteTime,
                            IsDelete = QLNgayLetbl.IsDelete,
                            Id = QLNgayLetbl.Id,
                            CreatedID = QLNgayLetbl.CreatedID,
                            UpdatedID = QLNgayLetbl.UpdatedID,
                            DeleteId = QLNgayLetbl.DeleteId

                        };

            if (searchModel != null)
            {
                if (!string.IsNullOrEmpty(searchModel.TenNgayLeFilter))
                {
                    query = query.Where(x => x.TenNgayLe.Contains(searchModel.TenNgayLeFilter));
                }
                if (searchModel.NgayLeFilter != null)
                {
                    query = query.Where(x => x.NgayLe >= searchModel.NgayLeFilter);
                }
                if (searchModel.NgayLeFilterTo != null)
                {
                    query = query.Where(x => x.NgayLe <= searchModel.NgayLeFilterTo);
                }


                if (!string.IsNullOrEmpty(searchModel.sortQuery))
                {
                    query = query.OrderBy(searchModel.sortQuery);
                }
                else
                {
                    query = query.OrderByDescending(x => x.Id);
                }
            }
            else
            {
                query = query.OrderByDescending(x => x.Id);
            }
            var resultmodel = new PageListResultBO<QLNgayLeDto>();
            if (pageSize == -1)
            {
                var dataPageList = query.ToList();
                resultmodel.Count = dataPageList.Count;
                resultmodel.TotalPage = 1;
                resultmodel.ListItem = dataPageList;
            }
            else
            {
                var dataPageList = query.ToPagedList(pageIndex, pageSize);
                resultmodel.Count = dataPageList.TotalItemCount;
                resultmodel.TotalPage = dataPageList.PageCount;
                resultmodel.ListItem = dataPageList.ToList();
            }
            foreach (var item in resultmodel.ListItem)
            {
                item.NgayLetxt = string.Format("{0:dd/MM/yyyy}", item.NgayLe);
            }
            return resultmodel;
        }

        public QLNgayLe GetById(long id)
        {
            return _QLNgayLeRepository.GetById(id);
        }

        public List<QLNgayLe> GetNgayLeCuaThang(int month, int year)
        {
            return _QLNgayLeRepository.GetAllAsQueryable().Where(x => x.NgayLe.Value.Month == month && x.NgayLe.Value.Year == year).ToList();
        }

        public List<QLNgayLeDto> GetDaTaListNgayLe(long id)
        {
            var query = (from QLNgayLetbl in _QLNgayLeRepository.GetAllAsQueryable()

                         select new QLNgayLeDto
                         {
                             TenNgayLe = QLNgayLetbl.TenNgayLe,
                             NgayLe = QLNgayLetbl.NgayLe,
                             CreatedBy = QLNgayLetbl.CreatedBy,
                             UpdatedBy = QLNgayLetbl.UpdatedBy,
                             CreatedDate = QLNgayLetbl.CreatedDate,
                             UpdatedDate = QLNgayLetbl.UpdatedDate,
                             DeleteTime = QLNgayLetbl.DeleteTime,
                             IsDelete = QLNgayLetbl.IsDelete,
                             Id = QLNgayLetbl.Id,
                             CreatedID = QLNgayLetbl.CreatedID,
                             UpdatedID = QLNgayLetbl.UpdatedID,
                             DeleteId = QLNgayLetbl.DeleteId

                         }).ToList();
            foreach (var item in query)
            {
                //if ()
                //{
                //}
            }
            return query;
        }
    }
}
