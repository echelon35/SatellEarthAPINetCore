using SatellEarthAPI.Application.Common.Interfaces;

namespace SatellEarthAPI.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}