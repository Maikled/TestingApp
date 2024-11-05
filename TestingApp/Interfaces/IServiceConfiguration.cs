namespace TestingApp.Interfaces
{
    public interface IServiceConfiguration
    {
        public void ConfigureService(IServiceCollection services);
        public void ConfigureApp(WebApplication app);
    }
}
