using FilmCatalogy.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FilmCatalogy.Infrastructure.DbContexts.Configs
{
	public class FilmCategoryConfig : IEntityTypeConfiguration<FilmCategory>
	{
		public void Configure(EntityTypeBuilder<FilmCategory> builder)
		{
			builder.ToTable("film_categories");
			builder.HasKey(x => x.Id);

			builder
				.HasOne(x => x.Film)
				.WithMany(x => x.Categories)
				.HasForeignKey(x => x.FilmId);

			builder
				.HasOne(x => x.Category)
				.WithMany(x => x.FilmCategories)
				.HasForeignKey(x => x.CategoryId);

			builder.Property(f => f.FilmId).HasColumnName("film_id");

			builder.Property(f => f.CategoryId).HasColumnName("category_id");

            builder.HasData(
					new FilmCategory { Id = 1, FilmId = 1, CategoryId = 1 },
					new FilmCategory { Id = 2, FilmId = 1, CategoryId = 2 },
					new FilmCategory { Id = 3, FilmId = 2, CategoryId = 2 },
					new FilmCategory { Id = 4, FilmId = 3, CategoryId = 3 }
				);
        }
	}
}
