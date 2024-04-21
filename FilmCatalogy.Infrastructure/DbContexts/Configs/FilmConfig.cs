using FilmCatalogy.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FilmCatalogy.Infrastructure.DbContexts.Configs
{
	/// <summary>
	/// SQL Script for this table
	/// CREATE TABLE films(
	///		Id INT IDENTITY(1,1) PRIMARY KEY,
	///		name VARCHAR(200),
	///		director VARCHAR(200),
	///		release DATETIME
	/// );
	/// </summary>
	public class FilmConfig : IEntityTypeConfiguration<Film>
	{
		public void Configure(EntityTypeBuilder<Film> builder)
		{
			builder.ToTable("films");
			builder.HasKey(x => x.Id);

			builder.Property(f => f.Name).HasColumnName("name");
			builder.Property(f => f.Director).HasColumnName("director");
			builder.Property(f => f.Release).HasColumnName("release");

            builder.HasData(
						new Film { Id = 1, Name = "Фильм 1", Director = "Режиссер 1", Release = new DateTime(2020, 1, 1) },
						new Film { Id = 2, Name = "Фильм 2", Director = "Режиссер 2", Release = new DateTime(2019, 12, 15) },
						new Film { Id = 3, Name = "Фильм 3", Director = "Режиссер 3", Release = new DateTime(2021, 5, 20) }
			);
        }
	}
}
