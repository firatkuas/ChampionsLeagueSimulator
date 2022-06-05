using Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    public partial class PointState:IEntity
    {
        public int Id { get; set; }
        public byte TeamId { get; set; }
        public byte Played { get; set; }
        public byte Won { get; set; }
        public byte Drawn { get; set; }
        public byte Lost { get; set; }
        public byte GoalFor { get; set; }
        public byte GoalAgainst { get; set; }
        public sbyte GoalDifference { get; set; }
        public byte Points { get; set; }
    }
}
