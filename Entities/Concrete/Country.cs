using Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    public class Country : IEntity
    {
        public byte Id { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }

    }
}
