using System;

namespace Push.Models
{
    public abstract class EntityBase
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
