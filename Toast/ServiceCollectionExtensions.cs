using MC.Toast.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MC.Toast;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMcToast(this IServiceCollection services)
    {
        return services.AddScoped<IToastService, ToastService>();
    }
}
