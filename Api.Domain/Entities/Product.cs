using Api.Domain.Attributes;
using Api.Domain.Common;

namespace Api.Domain.Entities
{
    //
    [TableName("Products")]
    public class Product : Entity
    {
        [ColumnName("Name")]
        public string Name { get; set; } = string.Empty;
        [ColumnName]
        public decimal Price { get; set; }
        [ColumnName]
        public int Stock { get; set; }
    }
}
