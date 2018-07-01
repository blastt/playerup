using AutoMapper;
using Market.Model.Models;
using Market.Service;
using Market.Web.ViewModels;
using System.Web.Mvc;

namespace Market.Web.Controllers
{
    public class GameController : Controller
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }
        // GET: Game
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateGameViewModel model)
        {
            Game game = Mapper.Map<CreateGameViewModel, Game>(model);
            _gameService.CreateGame(game);
            _gameService.SaveGame();
            return RedirectToAction("Details", "FilterItem");
        }

        public ActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(int? id)
        {
            if(id != null)
            {
                var game = _gameService.GetGame(id.Value);
                _gameService.CreateGame(game);
                _gameService.SaveGame();
                return View();
            }
            return HttpNotFound("There is no game with that id");
            
            
        }
    }
}