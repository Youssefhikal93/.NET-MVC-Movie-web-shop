using Microsoft.EntityFrameworkCore;
using Lexiflix.Data.Db;
using Lexiflix.Services;
using Lexiflix.Utils;

namespace Lexiflix
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            
            

            var connectionString = builder.Configuration.GetConnectionString(
            "DefaultConnection") ?? throw new InvalidCastException("Default Connection not found");

            builder.Services.AddDbContext<MovieDbContext>(
                options =>
             options.UseSqlServer(connectionString)
                
                );

            builder.Services.AddScoped<IMovieServices, MovieServices>();
            builder.Services.AddHttpClient();
            builder.Services.AddTransient<MovieSeederService>();

            builder.Services.AddScoped<IOrderServices, OrderServices>();
            builder.Services.AddScoped<ICustomerServices, CustomerServices>();
            builder.Services.AddScoped<IHomeService, HomeService>();


            builder.Services.AddControllersWithViews();
            builder.Services.AddSession();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddDistributedMemoryCache();
            var app = builder.Build();

            
            
            using (var scope = app.Services.CreateScope())
            {
                var seeder = scope.ServiceProvider.GetRequiredService<MovieSeederService>();

                // Only seed if there is no movie data in the database
                var db = scope.ServiceProvider.GetRequiredService<MovieDbContext>();
                await seeder.SeedInitialMoviesAsync();
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
