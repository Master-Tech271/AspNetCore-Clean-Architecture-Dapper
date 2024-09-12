using Api.Application.Common;

namespace Api.Infrastructure.Extensions
{
    public class EnumExtensions : IEnumExtensions
    {
        public (string Name, int Value) GetEnumInfo(Enum value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return (value.ToString(), Convert.ToInt32(value)); // Returns both the name and the numeric value.
        }
    }

}
