using System.Collections.Generic;
using System.Threading.Tasks;

namespace Franquias.Api.Repositories
{
    // O <T> significa que ela aceita qualquer classe (Usuario, Unidade, Venda, etc.)
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> ObterTodosAsync();
        Task<T?> ObterPorIdAsync(int id);
        Task AdicionarAsync(T entity);
        Task AtualizarAsync(T entity);
        Task RemoverAsync(T entity);
    }
}