using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public interface IMatchDal:IEntityRepository<Match>
    {
        List<MatchDetailDto> GetMatchDetails();
    }
}
