namespace Api.Application.Common
{
    public interface IEnumExtensions
    {
        (string Name, int Value) GetEnumInfo(Enum value);
    }
}
