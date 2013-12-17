namespace Labo.Common.Ioc.Tests.Performance.Domain
{
    public class Controller : IController
    {
        private readonly IErrorHandler m_ErrorHandler;

        public Controller(IErrorHandler errorHandler)
        {
            this.m_ErrorHandler = errorHandler;
        }
    }
}