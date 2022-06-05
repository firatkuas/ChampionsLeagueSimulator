using DataAccess.Abstract;
using Entities.Concrete;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Entities.DTOs;
using System.Linq;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfTeamDal:EfEntityRepositoryBase<Team,ChampionsLeagueDBContext>,ITeamDal
    {
        public List<TeamDetailDTO> GetTeamDetails()
        {
            using (ChampionsLeagueDBContext context = new ChampionsLeagueDBContext())
            {
                var result = from t in context.Teams
                             join c in context.Countries
                             on t.CountryId equals c.Id
                             select new TeamDetailDTO
                             {
                                 Id = t.Id,
                                 TeamName = t.TeamName,
                                 CountryName = c.CountryName,
                                 CountryCode = c.CountryCode,
                                 GroupId = t.GroupId,
                                 PocketId = t.PocketId
                             };
                return result.ToList();
            }
        }
    }
}
