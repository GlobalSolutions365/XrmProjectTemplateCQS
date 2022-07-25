using System;
using Xrm.Application.Interfaces;

namespace Xrm.UnitTests.Fakes
{
    public class FakeDateProvider : IDateProvider
    {
        public FakeDateProvider(DateTime utcNow)
        {
            UtcNow = utcNow;
            UtcToday = utcNow.Date;
        }

        public void SetDate(DateTime utcNow)
        {
            UtcNow = utcNow;
            UtcToday = utcNow.Date;
        }

        public DateTime UtcNow { get; private set; }
        public DateTime UtcToday { get; private set; }
    }
}
