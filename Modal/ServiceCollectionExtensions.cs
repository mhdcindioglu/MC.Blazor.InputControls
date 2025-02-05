using MC.Modal.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MC.Modal;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMcModal(this IServiceCollection services) 
        => services.AddScoped<IModalService, ModalService>();
}