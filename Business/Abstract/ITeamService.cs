using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface ITeamService
    {
        List<Team> GetAll();
        void Update(Team team);
        List<TeamDetailDTO> GetTeamDetails();


    }
}
