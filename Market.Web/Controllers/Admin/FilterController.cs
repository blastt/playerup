using Market.Service;
using System.Web.Mvc;

namespace Market.Web.Controllers
{
    public class FilterController : Controller
    {
        private readonly IGameService _gameService;
        private readonly IFilterService _filterService;

        public FilterController(IGameService gameService, IFilterService filterService)
        {
            _gameService = gameService;
            _filterService = filterService;
        }
                

        public ActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(int? id)
        {
            if (id != null)
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