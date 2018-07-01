using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace Market.Web.Helpers
{
    public static class ActiveHelpers
    {
        public static string IsSelected(this HtmlHelper html, string controllers = "", string actions = "", string cssClass = "active")
        {
            
            ViewContext viewContext = html.ViewContext;
            bool isChildAction = viewContext.Controller.ControllerContext.IsChildAction;

            if (isChildAction)
                viewContext = html.ViewContext.ParentActionViewContext;

            RouteValueDictionary routeValues = viewContext.RouteData.Values;
            string currentAction = routeValues["action"].ToString();
            string currentController = routeValues["controller"].ToString();

            if (String.IsNullOrEmpty(actions))
                actions = currentAction;

            if (String.IsNullOrEmpty(controllers))
                controllers = currentController;

            string[] acceptedActions = actions.ToLower().Trim().Split(',').Distinct().ToArray();
            string[] acceptedControllers = controllers.ToLower().Trim().Split(',').Distinct().ToArray();

            return acceptedActions.Contains(currentAction.ToLower()) && acceptedControllers.Contains(currentController.ToLower()) ?
                cssClass : String.Empty;
        }

        public static string IsSelectedGame(this HtmlHelper html, string controllers = "", string actions = "", string games = "", string cssClass = "active")
        {
            ViewContext viewContext = html.ViewContext;
            bool isChildAction = viewContext.Controller.ControllerContext.IsChildAction;

            if (isChildAction)
                viewContext = html.ViewContext.ParentActionViewContext;

            RouteValueDictionary routeValues = viewContext.RouteData.Values;
            string currentGame = routeValues["game"] == null ? "csgo" : routeValues["game"].ToString();
            string currentAction = routeValues["action"].ToString();
            string currentController = routeValues["controller"].ToString();

            if (String.IsNullOrEmpty(games))
                games = currentAction;

            if (String.IsNullOrEmpty(actions))
                actions = currentAction;

            if (String.IsNullOrEmpty(controllers))
                controllers = currentController;

            string[] acceptedGames = games.ToLower().Trim().Split(',').Distinct().ToArray();
            string[] acceptedActions = actions.ToLower().Trim().Split(',').Distinct().ToArray();
            string[] acceptedControllers = controllers.ToLower().Trim().Split(',').Distinct().ToArray();
            string str = acceptedActions.Contains(currentAction.ToLower()) && acceptedGames.Contains(currentGame.ToLower()) && acceptedControllers.Contains(currentController.ToLower()) ?
                cssClass : String.Empty;
            return str;
        }
    }
}