using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IMatchService
    {
        List<Match> GetAll();
        void Add(Match match);
        void Delete(Match match);
        void Update(Match match);
        List<MatchDetailDto> GetMatchDetails();
    }
}
