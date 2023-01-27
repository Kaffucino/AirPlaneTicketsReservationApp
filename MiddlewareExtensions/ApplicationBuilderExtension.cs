using AvioApp.SubscribeTableDependencies;

namespace AvioApp.MiddlewareExtensions
{
    public static class ApplicationBuilderExtension
    {
        public static void UseReservationsTableDependency(this IApplicationBuilder appBuilder) 
        {
            var service_provider = appBuilder.ApplicationServices;
            var service=service_provider.GetService<SubscribeReservationsTableDependencies>();
            service.subscribeTableDependency();
        
        }
    }
}
