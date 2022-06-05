using Business.Concrete;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfMatchDal : EfEntityRepositoryBase<Match, ChampionsLeagueDBContext>, IMatchDal
    {
        public List<MatchDetailDto> GetMatchDetails()
        {
            using (ChampionsLeagueDBContext context = new ChampionsLeagueDBContext())
            {
                //select t.TeamName,m.HomeGoals,m.AwayGoals, t2.TeamName from Matches m
                //join Teams t on t.Id = m.HomeId join Teams t2 on m.AwayId = t2.Id
                var result = from m in context.Matches
                             join t in context.Teams
                             on m.HomeId equals t.Id
                             join t2 in context.Teams
                             on m.AwayId equals t2.Id
                             select new MatchDetailDto
                             {
                                 Id = m.Id,
                                 HomeGoals = m.HomeGoals,
                                 AwayGoals = m.AwayGoals,
                                 HomeTeamName = t.TeamName,
                                 AwayTeamName = t2.TeamName,
                                 Played = m.Played
                             };

                return result.ToList();
            }
        }
    }
}
