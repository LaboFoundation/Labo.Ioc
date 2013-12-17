namespace Labo.Common.Ioc.Tests.Performance.Domain
{
    public class Settings : ISettings
    {
        private readonly IConfigurationManager m_ConfigurationManager;

        public Settings(IConfigurationManager configurationManager)
        {
            m_ConfigurationManager = configurationManager;
        }
    }
}