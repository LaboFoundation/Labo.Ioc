namespace Labo.Common.Ioc.Tests.Performance.Domain
{
    public class ErrorHandler : IErrorHandler
    {
        private readonly ILogger m_Logger;
        private readonly ISettings m_Settings;

        public ErrorHandler(ILogger logger, ISettings settings)
        {
            this.m_Logger = logger;
            this.m_Settings = settings;
        }

        public ILogger Logger { get { return this.m_Logger; } }
    }
}