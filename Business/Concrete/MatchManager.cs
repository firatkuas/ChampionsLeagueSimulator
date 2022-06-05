using Business.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class MatchManager:IMatchService
    {
        IMatchDal _matchDal;

        public MatchManager(IMatchDal matchDal)
        {
            _matchDal = matchDal;
        }

        public void Add(Match match)
        {
            _matchDal.Add(match);
        }

        public void Delete(Match match)
        {
            _matchDal.Delete(match);
        }

        public List<Match> GetAll()
        {
            return _matchDal.GetAll();
        }

        public List<MatchDetailDto> GetMatchDetails()
        {
            return _matchDal.GetMatchDetails();
        }

        public void Update(Match match)
        {
            _matchDal.Update(match);
        }
    }
}
