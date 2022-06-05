using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Business.Abstract
{
    public interface IPointStateService
    {
        List<PointState> GetAll();
        List<PointState> GetAll(Expression<Func<PointState, bool>> filter = null);
        List<PointStateDetailDto> GetPointStateDetails();
        void Add(PointState pointState);
        void Update(PointState pointState);

        void DeleteAll();
    }
}
