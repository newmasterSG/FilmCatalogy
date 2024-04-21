using FilmCatalogy.Application.Models.Category;

namespace FilmCatalogy.Application.Models.Films
{
    public class FilmDTO
    {
		public int Id { get; set; }

		public string Name { get; set; }

		public string Director { get; set; }
		public DateTime Release { get; set; }

		public List<CategoryDTO>? Categories { get; set; }
	}
}
