namespace FilmCatalogy.Application.Interfaces
{
	public interface IService<TEntityDTO>
		where TEntityDTO : class
	{
		Task<bool> AddAsync(TEntityDTO dTO, CancellationToken cancellation = default);

		Task<List<TEntityDTO>> GetAllAsync(int pageNumber = 1, int pageSize = 10, string attribute = "", string order = "asc", CancellationToken cancellation = default);

		Task<TEntityDTO> GetAsync(int id, CancellationToken cancellation = default);

		Task UpdateAsync(int id, TEntityDTO dto, CancellationToken cancellation = default);

		Task<bool> DeleteAsync(int id, CancellationToken cancellation = default);
	}
}
