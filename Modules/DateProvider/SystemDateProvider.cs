using System;
using Xrm.Application.Interfaces;

namespace DateProvider
{
    public class SystemDateProvider : IDateProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;

        public DateTime UtcToday => DateTime.UtcNow.ToUniversalTime();
    }
}
