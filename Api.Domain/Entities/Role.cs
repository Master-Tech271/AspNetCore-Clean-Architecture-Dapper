using Api.Domain.Attributes;
using Api.Domain.Common;

namespace Api.Domain.Entities
{
    public class Role : Entity
    {
        [ColumnName]
        public string RoleName { get; set; } = default!;
    }
}
