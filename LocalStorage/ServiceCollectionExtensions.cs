using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using MC.LocalStorage.JsonConverters;
using MC.LocalStorage.Serialization;
using MC.LocalStorage.StorageOptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace MC.LocalStorage
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMcLocalStorage(this IServiceCollection services)
            => AddMcLocalStorage(services, null);

        public static IServiceCollection AddMcLocalStorage(this IServiceCollection services, Action<LocalStorageOptions>? configure)
        {
            services.TryAddScoped<IStorageProvider, BrowserStorageProvider>();
            AddServices(services, configure);
            return services;
        }

        public static IServiceCollection AddMcLocalStorageStreaming(this IServiceCollection services)
            => AddMcLocalStorageStreaming(services, null);

        public static IServiceCollection AddMcLocalStorageStreaming(this IServiceCollection services, Action<LocalStorageOptions>? configure)
        {
            services.TryAddScoped<IStorageProvider, BrowserStreamingStorageProvider>();
            AddServices(services, configure);
            return services;
        }

        private static void AddServices(IServiceCollection services, Action<LocalStorageOptions>? configure)
        {
            services.TryAddScoped<IJsonSerializer, SystemTextJsonSerializer>();
            services.TryAddScoped<ILocalStorageService, LocalStorageService>();
            services.TryAddScoped<ISyncLocalStorageService, LocalStorageService>();
            if (services.All(serviceDescriptor => serviceDescriptor.ServiceType != typeof(IConfigureOptions<LocalStorageOptions>)))
            {
                services.Configure<LocalStorageOptions>(configureOptions =>
                {
                    configure?.Invoke(configureOptions);
                    configureOptions.JsonSerializerOptions.Converters.Add(new TimespanJsonConverter());
                });
            }
        }

        /// <summary>
        /// Registers the Mc LocalStorage services as singletons. This should only be used in Blazor WebAssembly applications.
        /// Using this in Blazor Server applications will cause unexpected and potentially dangerous behaviour. 
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection AddMcLocalStorageAsSingleton(this IServiceCollection services)
            => AddMcLocalStorageAsSingleton(services, null);

        /// <summary>
        /// Registers the Mc LocalStorage services as singletons. This should only be used in Blazor WebAssembly applications.
        /// Using this in Blazor Server applications will cause unexpected and potentially dangerous behaviour. 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IServiceCollection AddMcLocalStorageAsSingleton(this IServiceCollection services, Action<LocalStorageOptions>? configure)
        {
            services.TryAddSingleton<IJsonSerializer, SystemTextJsonSerializer>();
            services.TryAddSingleton<IStorageProvider, BrowserStorageProvider>();
            services.TryAddSingleton<ILocalStorageService, LocalStorageService>();
            services.TryAddSingleton<ISyncLocalStorageService, LocalStorageService>();
            if (services.All(serviceDescriptor => serviceDescriptor.ServiceType != typeof(IConfigureOptions<LocalStorageOptions>)))
            {
                services.Configure<LocalStorageOptions>(configureOptions =>
                {
                    configure?.Invoke(configureOptions);
                    configureOptions.JsonSerializerOptions.Converters.Add(new TimespanJsonConverter());
                });
            }
            return services;
        }
    }
}
