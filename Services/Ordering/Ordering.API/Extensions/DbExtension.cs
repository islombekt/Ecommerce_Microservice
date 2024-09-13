using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace Ordering.API.Extensions
{
    public static class DbExtension
    {
        public static IHost MigrateDatabase<TContext>(this IHost host,Action<TContext, IServiceProvider> seeder) where TContext : DbContext
        {
            using (var scope = host.Services.CreateScope()) 
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var context = services.GetRequiredService<TContext>();
                try
                {
                    logger.LogInformation($"--> Started Db Migration: {typeof(TContext).Name}");
                    var retry = Policy.Handle<SqlException>().WaitAndRetry(
                        retryCount: 5, sleepDurationProvider: retrAttempt => TimeSpan.FromSeconds(Math.Pow(retrAttempt, 2)),
                        onRetry: (exception, span, count) => { 
                            logger.LogError($"--> Retrying because of {exception} {span}");
                        }
                        );
                    retry.Execute(() => CallSeeder(seeder,context,services));
                    logger.LogInformation($"--> Migration Completed: {typeof(TContext).Name}"); 
                }
                catch (Exception ex) {
                    logger.LogError($"--> Migration Failed: {typeof(TContext).Name}, error {ex.InnerException}");
                }
            }
            return host;
        }

        private static void CallSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider services) where TContext : DbContext
        {
            context.Database.Migrate();
            seeder(context, services);
        }
    }
}
