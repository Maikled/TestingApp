using TestingApp.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TestingApp.Core.Models.Tests.DataProviders;
using TestingApp.Core.Models.Tests;
using TestingApp.Core.Processing.CSharp;
using TestingApp.Database;

namespace TestingApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DatabaseConnection")));

            var assignableType = typeof(IServiceConfiguration);
            var servicesConfigurationTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => assignableType.IsAssignableFrom(type) && assignableType != type);
            foreach (var serviceType in servicesConfigurationTypes)
            {
                var serviceConfiguration = Activator.CreateInstance(serviceType) as IServiceConfiguration;
                serviceConfiguration?.ConfigureService(builder.Services);
            }

            var app = builder.Build();

            foreach (var serviceType in servicesConfigurationTypes)
            {
                var serviceConfiguration = app.Services.GetService(serviceType) as IServiceConfiguration;
                serviceConfiguration?.ConfigureApp(app);
            }

            using (var scope = app.Services.CreateScope())
            {
                var databaseContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                databaseContext?.Database.Migrate();
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

            var test = new TextDataProvider().GetTestData();

            TestResult result1;
            var task1 = Task.Run(() =>
            {
                var compiler = new CSharpCompiller(test);
                result1 = compiler.Run();
            });

            TestResult result2;
            var task2 = Task.Run(() =>
            {
                var compiler = new CSharpCompiller(test);
                result2 = compiler.Run();
            });

            TestResult result3;
            var task3 = Task.Run(() =>
            {
                var compiler = new CSharpCompiller(test);
                result3 = compiler.Run();
            });

            Task.WaitAll(task1, task2, task3);

            app.Run();
        }
    }
}
