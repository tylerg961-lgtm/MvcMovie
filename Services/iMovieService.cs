using System.Collections.Generic;  
using System.Threading.Tasks;
using MvcMovie.Models;

namespace MvcMovie.Services
{
    public interface iMovieService
    {
        Task<IEnumerable<Movie>> GetAllAsync();
        Task<Movie?> GetByIdAsync(int id);
        Task AddAsync(Movie movie);
        Task UpdateAsync(Movie movie);
        Task DeleteAsync(int id);
    }
}
