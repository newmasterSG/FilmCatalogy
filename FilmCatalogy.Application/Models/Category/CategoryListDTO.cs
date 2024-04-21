namespace FilmCatalogy.Application.Models.Category
{
    public class CategoryListDTO
    {
        public string Name { get; set; }
        public int NumberOfFilms { get; set; }
        public int NestingLevel { get; set; }
    }
}
