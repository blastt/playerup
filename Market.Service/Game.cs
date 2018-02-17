using Market.Data.Infrastructure;
using Market.Data.Repositories;
using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Service
{
    public interface IGameService
    {
        IEnumerable<Game> GetGames();
        Game GetGame(int id);
        Game GetGameByValue(string name);
        void CreateGame(Game message);
        void SaveGame();
    }

    public class GameService : IGameService
    {
        private readonly IGameRepository gamesRepository;
        private readonly IUnitOfWork unitOfWork;

        public GameService(IGameRepository gamesRepository, IUnitOfWork unitOfWork)
        {
            this.gamesRepository = gamesRepository;
            this.unitOfWork = unitOfWork;
        }

        #region IGameService Members

        public IEnumerable<Game> GetGames()
        {
            var games = gamesRepository.GetAll();
            return games;
        }


        public Game GetGame(int id)
        {
            var game = gamesRepository.GetById(id);
            return game;
        }


        public void CreateGame(Game game)
        {
            gamesRepository.Add(game);
        }

        public void SaveGame()
        {
            unitOfWork.Commit();
        }

        public Game GetGameByValue(string name)
        {
            return gamesRepository.GetGameByValue(name);
        }

        #endregion

    }
}
