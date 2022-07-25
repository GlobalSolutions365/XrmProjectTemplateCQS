using System.Collections.Generic;
using Xrm.Application.Interfaces;
using Xrm.Domain.Configuration;

namespace Xrm.UnitTests.Fakes
{
    public class FakeConfigurationReader : IConfigurationReader
    {
        private Dictionary<Settings.Keys, string> settings = new Dictionary<Settings.Keys, string>();

        public void AddSetting(Settings.Keys key, string value)
        {
            settings.Add(key, value);
        }

        public string GetSetting(Settings.Keys key)
        {
            return settings[key];
        }

        public string GetSettingOrDefault(Settings.Keys key)
        {
            if (settings.ContainsKey(key))
            {
                return settings[key];
            }
            else
            {
                return "";
            }
        }
    }
}
