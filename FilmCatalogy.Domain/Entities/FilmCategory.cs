namespace FilmCatalogy.Domain.Entities
{
	public class FilmCategory
	{
		public int Id { get; set; }
		public int FilmId { get; set; }
		public int CategoryId { get; set; }

		public Film Film { get; set; }
		public Category Category { get; set; }
	}
}
