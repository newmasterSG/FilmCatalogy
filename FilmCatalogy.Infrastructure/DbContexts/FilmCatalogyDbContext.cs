using FilmCatalogy.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace FilmCatalogy.Infrastructure.DbContexts
{

	/// <summary>
	/// DbContext it's like view of our db, similary if I write CREATE DATABASE FilmCatalogy , I will get created db
	/// </summary>
	/// <param name="options"></param>
	public class FilmCatalogyDbContext(DbContextOptions<FilmCatalogyDbContext> options) : DbContext(options)
	{
		public DbSet<Film> Films { get; set; }

		public DbSet<Category> Categories { get; set; }

		public DbSet<FilmCategory> FilmCategories { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		}
	}
}
