using AutoMapper;
using Market.Model.Models;
using Market.Service;
using Market.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
        
        public ActionResult Create()
        {
            CreateFilterViewModel model = new CreateFilterViewModel();
            IList<SelectListItem> list = new List<SelectListItem>();
            foreach (var game in _gameService.GetGames())
            {
                list.Add(new SelectListItem
                {
                    Value = game.Value,
                    Text = game.Name
                });
            }
            model.Games = list;
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CreateFilterViewModel model)
        {
            var game = _gameService.GetGameByValue(model.Game.Value);
            if(game != null)
            {
                model.Game = game;
            }
            var filter = Mapper.Map<CreateFilterViewModel, Model.Models.Filter>(model);
            if (game != null && filter != null)
            {
                game.Filters.Add(filter);
                _filterService.CreateFilter(filter);
                _gameService.SaveGame();
                return RedirectToAction("Details", "FilterItem");
            }
            return HttpNotFound();
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