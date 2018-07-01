using Market.Data.Infrastructure;
using Market.Model.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Market.Data.Repositories
{
    public class GameRepository : RepositoryBase<Game>, IGameRepository
    {
        public GameRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public Game GetGameByValue(string name)
        {
            return DbContext.Games.FirstOrDefault(g => g.Value == name);
        }

        public Game GetGameByValue(string name, params Expression<Func<Game, object>>[] includes)
        {
            var query = DbContext.Games.Where(g => g.Value == name);
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query.FirstOrDefault();
        }

        public Game GetGameByValueAsNoTracking(string name, params Expression<Func<Game, object>>[] includes)
        {
            var query = DbContext.Games.AsNoTracking().Where(g => g.Value == name);
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query.FirstOrDefault();
        }
    }

    public interface IGameRepository : IRepository<Game>
    {
        Game GetGameByValue(string userName);
        Game GetGameByValue(string name, params Expression<Func<Game, object>>[] includes);
        Game GetGameByValueAsNoTracking(string name, params Expression<Func<Game, object>>[] includes);
    }
}
