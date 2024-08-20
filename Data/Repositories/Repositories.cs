using BetsApi.Models;
using System.Linq.Expressions;

namespace BetsApi.Repositories
{
    //General CRUD Operations
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetByIdAsync(long id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
    }
    //User Specific
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByNameAsync(string name);
        Task<IEnumerable<User>> GetUsersWithBetsAsync();
    }

    // Bet Specific
    public interface IBetRepository : IRepository<Bet>
    {
        Task<IEnumerable<Bet>> GetBetsBetweenUsersAsync(long receiverUserId, long senderUserId);
        Task<IEnumerable<Bet>> GetPendingBetsAsync();
    }

}