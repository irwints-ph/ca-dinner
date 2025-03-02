using Apps.Application.Common.Interfaces.Services;

namespace Apps.Infrastructure.Services;
public class DateTimeProvider : IDateTimeProvider
{

    public DateTime UtcNow => DateTime.UtcNow;
}
