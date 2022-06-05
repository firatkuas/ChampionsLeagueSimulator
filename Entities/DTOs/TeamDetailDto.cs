using Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DTOs
{
    public class TeamDetailDTO:IEntity
    {
        public byte Id { get; set; }
        public string TeamName { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public char GroupId { get; set; }
        public byte PocketId { get; set; }


    }
}
