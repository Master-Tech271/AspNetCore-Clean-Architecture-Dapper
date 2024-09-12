using Api.Domain.Attributes;
using Api.Domain.Common;

namespace Api.Domain.Entities
{
    public class User : Entity
    {
        [ColumnName]
        public string Username { get; set; } = default!;
        [ColumnName]
        public string PasswordHash { get; set; } = default!;
        [ColumnName]
        public string Email { get; set; } = default!;
    }
}
