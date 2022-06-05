using Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DTOs
{
    public class MatchDetailDto:IEntity
    {
        public int Id { get; set; }
        public string HomeTeamName { get; set; }
        public string AwayTeamName { get; set; }
        public byte HomeGoals { get; set; }
        public byte AwayGoals { get; set; }
        public bool Played { get; set; }
    }
}
