using Market.Data.Infrastructure;
using Market.Data.Repositories;
using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Market.Service
{
    public interface IGameService
    {
        IEnumerable<Game> GetGames();
        IQueryable<Game> GetGamesAsNoTracking();
        IQueryable<Game> GetGames(Expression<Func<Game, bool>> where, params Expression<Func<Game, object>>[] includes);
        IQueryable<Game> GetGamesAsNoTracking(params Expression<Func<Game, object>>[] includes);
        IQueryable<Game> GetGamesAsNoTracking(Expression<Func<Game, bool>> where, params Expression<Func<Game, object>>[] includes);

        Game GetGame(int id);
        Game GetGameByValue(string name);
        void Delete(Game game);
        Game GetGameByValue(string name, params Expression<Func<Game, object>>[] includes);
        Game GetGameByValueAsNoTracking(string name, params Expression<Func<Game, object>>[] includes);
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

        

        public IQueryable<Game> GetGamesAsNoTracking()
        {
            var query = gamesRepository.GetAllAsNoTracking();
            return query;
        }

        public IQueryable<Game> GetGamesAsNoTracking(params Expression<Func<Game, object>>[] includes)
        {
            var games = gamesRepository.GetAllAsNoTracking(includes);
            return games;
        }

        public IQueryable<Game> GetGames(Expression<Func<Game, bool>> where, params Expression<Func<Game, object>>[] includes)
        {
            var query = gamesRepository.GetMany(where, includes);
            return query;
        }

        public IQueryable<Game> GetGamesAsNoTracking(Expression<Func<Game, bool>> where, params Expression<Func<Game, object>>[] includes)
        {
            var query = gamesRepository.GetManyAsNoTracking(where, includes);
            return query;
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

        public void Delete(Game game)
        {
            gamesRepository.Delete(game);
        }

        public void SaveGame()
        {
            unitOfWork.Commit();
        }

        public Game GetGameByValue(string name)
        {
            return gamesRepository.GetGameByValue(name);
        }

        public Game GetGameByValue(string name, params Expression<Func<Game, object>>[] includes)
        {
            var query = gamesRepository.GetGameByValue(name, includes);
            return query;
        }

        public Game GetGameByValueAsNoTracking(string name, params Expression<Func<Game, object>>[] includes)
        {
            var query = gamesRepository.GetGameByValueAsNoTracking(name, includes);
            return query;
        }

        #endregion

    }
}
