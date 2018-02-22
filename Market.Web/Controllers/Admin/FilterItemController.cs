using AutoMapper;
using Market.Model.Models;
using Market.Service;
using Market.Web.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        public ActionResult Create()
        {
            CreateFilterItemViewModel model = new CreateFilterItemViewModel();
            IList<SelectListItem> gameList = new List<SelectListItem>();
            foreach (var game in _gameService.GetGames())
            {
                gameList.Add(new SelectListItem
                {
                    Value = game.Value,
                    Text = game.Name
                });
            }
            model.Games = gameList;
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CreateFilterItemViewModel model)
        {
            var game = _gameService.GetGameByValue(model.Game.Value);
            if (game != null)
            {
                model.Game = game;
            }
            
            var filter = game.Filters.FirstOrDefault(m => m.Value == model.Filter.Value);
            if(filter != null)
            {
                model.Filter = filter;
            }
            var filterItem = Mapper.Map<CreateFilterItemViewModel, FilterItem>(model);

            if (game != null && filter != null && filterItem != null)
            {
                var existingFilterItem = _filterItemService.GetFilterItems().Where(m => m.Value == model.Value).FirstOrDefault();
                if(existingFilterItem != null)
                {
                    filter.FilterItems.Add(existingFilterItem);
                }
                else
                {
                    filter.FilterItems.Add(filterItem);
                    _filterItemService.CreateFilterItem(filterItem);
                }                
                _filterService.SaveFilter();
                return RedirectToAction("Details","FilterItem");
            }
            return HttpNotFound();
        }


        public ActionResult Details()
        {
            DetailsFilterItemViewModel model = new DetailsFilterItemViewModel
            {
                Games = _gameService.GetGames()
            };
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
        public string GetFiltersForGameJson(string game)
        {
            //Dictionary<string, string> ranks = new Dictionary<string, string>();
            //[{\"Text\":\"2x2\",\"Value\":\"2x2\",\"FilterItems\":[]},{\"Text\":\"5x5\",\"Value\":\"5x5\",\"FilterItems\":[]}]
            // Сделать проверку на налл
            Game g = _gameService.GetGameByValue(game);
            var ranks = g.Filters.Select(m => new
            {
                Text = m.Text,
                Value = m.Value,
                FilterItems = m.FilterItems,
                GameValue = m.Game.Value
            });
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            var str = JsonConvert.SerializeObject(ranks, jsonSerializerSettings);
            //check if any of the UserName matches the UserName specified in the Parameter using the ANY extension method.  
            return str;
        }
    }
}