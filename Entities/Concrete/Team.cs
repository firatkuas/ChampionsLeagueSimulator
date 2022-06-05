using Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    public class Team : IEntity
    {
        public byte Id { get; set; }
        public string TeamName { get; set; }
        public byte CountryId { get; set; }
        public byte PocketId { get; set; }
        public char GroupId { get; set; }
    }
}
