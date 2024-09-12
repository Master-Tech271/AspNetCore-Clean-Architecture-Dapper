namespace Api.Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TableNameAttribute : Attribute
    {
        public string Name { get; } = string.Empty;

        public TableNameAttribute(string name)
        {
            Name = name;
        }
    }
}
