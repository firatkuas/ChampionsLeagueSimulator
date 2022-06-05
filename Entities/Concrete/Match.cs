using Entities.Abstract;

namespace Entities.Concrete
{
    public class Match:IEntity
    {
        public int Id { get; set; }
        public byte HomeId { get; set; }
        public byte AwayId { get; set; }
        public byte HomeGoals { get; set; }
        public byte AwayGoals { get; set; }
        public bool Played { get; set; }

    }
}
