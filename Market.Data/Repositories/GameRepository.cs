using Market.Data.Infrastructure;
using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }

    public interface IGameRepository : IRepository<Game>
    {
        Game GetGameByValue(string userName);
    }
}
