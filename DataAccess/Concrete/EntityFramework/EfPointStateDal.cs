using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfPointStateDal : EfEntityRepositoryBase<PointState, ChampionsLeagueDBContext>, IPointStateDal
    {
        public List<PointStateDetailDto> GetPointStateDetails()
        {
            using (ChampionsLeagueDBContext context = new ChampionsLeagueDBContext())
            {
                var result = from p in context.Points
                             join t in context.Teams
                             on p.TeamId equals t.Id
                             select new PointStateDetailDto
                             {
                                 Id = p.Id,
                                 GroupId = t.GroupId,
                                 Drawn = p.Drawn,
                                 GoalAgainst = p.GoalAgainst,
                                 GoalDifference = p.GoalDifference,
                                 GoalFor = p.GoalFor,
                                 Lost = p.Lost,
                                 Played = p.Played,
                                 Points = p.Points,
                                 TeamName = t.TeamName,
                                 Won = p.Won
                             };


                return result.ToList(); 
            }
        }
    }
}
