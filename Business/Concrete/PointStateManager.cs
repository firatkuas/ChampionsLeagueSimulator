using Business.Abstract;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Business.Concrete
{
    public class PointStateManager : IPointStateService
    {
        IPointStateDal _pointStateDal;

        public PointStateManager(IPointStateDal pointStateDal)
        {
            _pointStateDal = pointStateDal;
        }

        public void Add(PointState pointState)
        {
            _pointStateDal.Add(pointState);
        }

        public void DeleteAll()
        {
            var all = _pointStateDal.GetAll();
            foreach (var point in all)
            {
                _pointStateDal.Delete(point);
            }
        }

        public List<PointState> GetAll()
        {
            return _pointStateDal.GetAll();
        }

        public List<PointState> GetAll(Expression<Func<PointState, bool>> filter = null)
        {
            return _pointStateDal.GetAll(filter);
        }

        public List<PointStateDetailDto> GetPointStateDetails()
        {
            return _pointStateDal.GetPointStateDetails();
        }

        public void Update(PointState pointState)
        {
            _pointStateDal.Update(pointState);
        }
    }
}
