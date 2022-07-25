using System;
using System.Collections.Generic;
using Xrm.Application.Interfaces;
using Xrm.Domain.Configuration;
using Xrm.Domain.DTO;

namespace PluginSettingsConfigurationReader
{
    public class ConfigurationReader : IConfigurationReader
    {
        private readonly Dictionary<string, string> settings = new Dictionary<string, string>();

        public ConfigurationReader(IJsonHelper jsonHelper, string serializedConfig)
        {
            jsonHelper = jsonHelper ?? throw new ArgumentNullException(nameof(jsonHelper));

            if (!string.IsNullOrWhiteSpace(serializedConfig))
            {
                ConfigurationSetting[] settingsArray = jsonHelper.Deserialize<ConfigurationSetting[]>(serializedConfig);

                foreach (var setting in settingsArray)
                {
                    settings.Add(setting.Key, setting.Value);
                }
            }
        }

        public string GetSetting(Settings.Keys key)
        {
            string keyName = key.ToString();

            if (!settings.ContainsKey(keyName))
            {
                throw new KeyNotFoundException($"Could not find key = \"{keyName}\" in settings. Available keys: {String.Join(", ", settings.Keys)}.");
            }

            return settings[keyName];
        }

        public string GetSettingOrDefault(Settings.Keys key)
        {
            string keyName = key.ToString();

            if (!settings.ContainsKey(keyName))
            {
                return "";
            }

            return settings[keyName];
        }
    }
}
