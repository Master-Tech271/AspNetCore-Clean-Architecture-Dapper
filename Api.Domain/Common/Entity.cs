using Api.Domain.Attributes;
using Api.Domain.Interfaces;

namespace Api.Domain.Common
{
    public class Entity : IEntity
    {
        [IdentityColumnAttribute]
        [ColumnName]
        public int Id { get; set; }
        [ColumnName]
        public DateTime CreatedAt { get; set; }
        [ColumnName]
        public DateTime UpdatedAt { get; set; }
    }
}
