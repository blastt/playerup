using AutoMapper;
using Market.Model.Models;
using Market.Service;
using Market.Web.ViewModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Market.Web.Controllers.Admin
{
    public class FilterItemController : Controller
    {
        private readonly IGameService _gameService;
        private readonly IFilterService _filterService;
        private readonly IFilterItemService _filterItemService;

        public FilterItemController(IGameService gameService, IFilterService filterService, IFilterItemService filterItemService)
        {
            _gameService = gameService;
            _filterService = filterService;
            _filterItemService = filterItemService;
        }

        


        public ActionResult Details()
        {
            DetailsFilterItemViewModel model = new DetailsFilterItemViewModel
            {
                Games = _gameService.GetGames()
            };
            foreach (var game in model.Games)
            {
                foreach (var filter in game.Filters)
                {
                    filter.FilterItems = filter.FilterItems.OrderBy(f => f.Rank).ToList();
                }
            }
            return View(model);
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

        [HttpPost]
        public JsonResult GetFiltersForGameJson(string game)
        {
            //Dictionary<string, string> ranks = new Dictionary<string, string>();
            //[{\"Text\":\"2x2\",\"Value\":\"2x2\",\"FilterItems\":[]},{\"Text\":\"5x5\",\"Value\":\"5x5\",\"FilterItems\":[]}]
            // Сделать проверку на налл
            Game g = _gameService.GetGameByValue(game, i => i.Filters, i => i.Filters.Select(fi => fi.FilterItems));

            IList<FilterViewModel> filters = new List<FilterViewModel>();
            if (g != null)
            {
                foreach (var filter in g.Filters)
                {
                    filter.FilterItems = filter.FilterItems.OrderBy(f => f.Rank).ToList();
                    filters.Add(Mapper.Map<Model.Models.Filter, FilterViewModel>(filter));
                }

            }
            
            var s = new JavaScriptSerializer();
            var jsonSerializerSettings = new JsonSerializerSettings();

           
            var str = s.Serialize(filters);
            //check if any of the UserName matches the UserName specified in the Parameter using the ANY extension method.  

            return Json(filters);
        }
        
    }
}