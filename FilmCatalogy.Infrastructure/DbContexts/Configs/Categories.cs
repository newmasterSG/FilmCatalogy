using FilmCatalogy.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FilmCatalogy.Infrastructure.DbContexts.Configs
{
	public class Categories : IEntityTypeConfiguration<Category>
	{
		public void Configure(EntityTypeBuilder<Category> builder)
		{
			builder.ToTable("categories");
			builder.HasKey(x => x.Id);

			builder.Property(f => f.Name).HasColumnName("name");

			builder.HasOne(p => p.ParentCategory)
					.WithMany(c => c.ChildCategories) // Each category can have multiple child categories
					.HasForeignKey(p => p.ParentCategoryId) // Foreign key for parent category
					.IsRequired(false); // Allow the foreign key to be nullable 

            builder.HasData(
					new Category { Id = 1, Name = "Категория 1" },
					new Category { Id = 2, Name = "Категория 2", ParentCategoryId = 1 },
					new Category { Id = 3, Name = "Категория 3", ParentCategoryId = 1 }
				);
        }
	}
}
