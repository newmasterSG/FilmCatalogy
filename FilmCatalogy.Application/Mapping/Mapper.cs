using FilmCatalogy.Application.Models.Category;
using FilmCatalogy.Application.Models.Films;
using FilmCatalogy.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmCatalogy.Application.Mapping
{
	public static class Mapper
	{
        public static FilmDTO FilmToDTO(Film film) => new FilmDTO
        {
            Id = film.Id,
            Name = film.Name,
            Director = film.Director,
            Release = film.Release,
            Categories = film.Categories.Select(c => CategoryToDTO(c.Category)).ToList()
        };

        public static Film DTOToFilm(FilmDTO filmDTO) => new Film
        {
            Id = filmDTO.Id,
            Name = filmDTO.Name,
            Director = filmDTO.Director,
            Release = filmDTO.Release,
            Categories = filmDTO.Categories.Select(c => new FilmCategory { CategoryId = c.Id }).ToList()
        };

        public static CategoryDTO CategoryToDTO(Category category) => new CategoryDTO
        {
            Id = category.Id,
            Name = category.Name,
            ParentCategoryId = category.ParentCategoryId,
            ChildCategory = category.ChildCategories?.Select(c => CategoryToDTO(c)).ToList()
        };

        public static Category DTOToCategory(CategoryDTO categoryDTO) => new Category
        {
            Id = categoryDTO.Id,
            Name = categoryDTO.Name,
            ParentCategoryId = categoryDTO.ParentCategoryId,
            ChildCategories = categoryDTO.ChildCategory?.Select(c => DTOToCategory(c)).ToList()
        };
    }
}
