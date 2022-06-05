using Business.Abstract;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Business.Concrete
{
    public class TeamManager : ITeamService
    {
        ITeamDal _teamDal;
        public TeamManager(ITeamDal teamDal)
        {
            _teamDal = teamDal;
        }
        public List<Team> GetAll()
        {
           return _teamDal.GetAll();
        }

        public List<TeamDetailDTO> GetTeamDetails()
        {
            return _teamDal.GetTeamDetails();
        }

        public void Update(Team team)
        {
            _teamDal.Update(team);
        }

        public bool AreTeamsGrouped()
        {

            if(_teamDal.GetAll().Where(t => t.GroupId == ' ').Count() == 0)
            {
                return true;
            }
            else return false;
        }
    }
}
