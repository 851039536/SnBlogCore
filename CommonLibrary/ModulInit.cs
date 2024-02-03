using Microsoft.Extensions.DependencyInjection;

namespace CommonLibrary
{
    public class ModulInit : IModuleInit
    {
        public void Init(IServiceCollection services)
        {
            //所有需要DI的在此处注入
            services.AddScoped<Class1>();
            //services.AddScoped<Class1,Class1>();
        }
    }
}
