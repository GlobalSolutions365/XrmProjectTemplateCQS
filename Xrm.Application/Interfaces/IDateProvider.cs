using System;

namespace Xrm.Application.Interfaces
{
    public interface IDateProvider
    {
        DateTime UtcNow { get; }
        DateTime UtcToday { get; }
    }
}
