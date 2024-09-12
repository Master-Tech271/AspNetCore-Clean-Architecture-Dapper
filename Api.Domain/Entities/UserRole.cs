using Api.Domain.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Api.Domain.Entities
{
    public class UserRole
    {
        [Key]
        [ColumnName]
        public int UserId { get; set; }
        [Key]
        [ColumnName]
        public int RoleId { get; set; }

    }
}
