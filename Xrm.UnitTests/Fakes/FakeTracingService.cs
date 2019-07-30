using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;

namespace Xrm.UnitTests.Fakes
{
    public class FakeTracingService : ITracingService
    {
        public List<string> TracedTexts { get; private set; } = new List<string>();

        public void Trace(string format, params object[] args)
        {
            TracedTexts.Add(String.Format(format, args));
        }
    }
}
