namespace FilmCatalogy.Domain.Entities
{
	public class Film
	{
		public int Id {  get; set; }

		public string Name { get; set; }

		public string Director { get; set; }

		public DateTime Release {  get; set; }

		public List<FilmCategory> Categories { get; set; }
	}
}
