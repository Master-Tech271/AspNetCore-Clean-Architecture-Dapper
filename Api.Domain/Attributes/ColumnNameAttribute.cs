namespace Api.Domain.Attributes
{
    public class ColumnNameAttribute : Attribute
    {
        public string? Name { get; }

        public ColumnNameAttribute(string? name = null)
        {
            Name = name;
        }
    }
}
