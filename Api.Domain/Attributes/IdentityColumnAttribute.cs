namespace Api.Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IdentityColumnAttribute : Attribute
    {
        public string Name { get; } = default!;

        public IdentityColumnAttribute(string name = "Id")
        {
            Name = name;
        }
    }
}
