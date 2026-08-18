using System.Collections.Generic;
using System.Linq.Expressions; // Adicione este using
using System.Threading.Tasks;

namespace Franquias.Api.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> ObterTodosAsync();
        Task<T?> ObterPorIdAsync(int id);
        
        // NOVO MÉTODO PARA FILTRAGEM
        Task<IEnumerable<T>> BuscarAsync(Expression<Func<T, bool>> predicate);
        
        Task AdicionarAsync(T entity);
        Task AtualizarAsync(T entity);
        Task RemoverAsync(T entity);
    }
}