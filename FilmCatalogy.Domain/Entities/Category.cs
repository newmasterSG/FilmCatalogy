namespace FilmCatalogy.Domain.Entities
{
	public class Category
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public int? ParentCategoryId { get; set; }

		public Category ParentCategory { get; set; }

		public List<Category> ChildCategories { get; set; }

		public List<FilmCategory> FilmCategories { get; set; }
	}
}
