using FilmCatalogy.Application.Interfaces;
using FilmCatalogy.Application.Services;
using FilmCatalogy.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace FilmCatalogy.WebUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var conStr = builder.Configuration.GetConnectionString("DefaultConnection");
            // Add services to the container.
            builder.Services.AddDbContext<FilmCatalogyDbContext>(options =>
            {
                options.UseSqlServer(conStr);
            });
            builder.Services.AddScoped<IFilmService, FilmService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpContextAccessor();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<FilmCatalogyDbContext>();

                dbContext.Database.EnsureCreated();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
