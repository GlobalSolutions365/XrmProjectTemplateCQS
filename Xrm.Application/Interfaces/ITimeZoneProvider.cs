using Xrm.Domain.DTO;

namespace Xrm.Application.Interfaces
{
    public interface ITimeZoneProvider
    {
        TimeZoneNames GetTimeZoneFromLatLng(double lat, double lng);
    }
}
