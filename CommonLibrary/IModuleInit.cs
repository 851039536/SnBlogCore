using Microsoft.Extensions.DependencyInjection;

namespace CommonLibrary
{
    /// <summary>
    /// DI公共接口
    /// </summary>
    public interface IModuleInit
    {
        public void Init(IServiceCollection services);
    }
}
