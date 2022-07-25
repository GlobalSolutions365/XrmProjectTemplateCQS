using Xrm.Domain.Configuration;

namespace Xrm.Application.Interfaces
{
    public interface IConfigurationReader
    {
        string GetSetting(Settings.Keys key);

        string GetSettingOrDefault(Settings.Keys key);
    }
}
