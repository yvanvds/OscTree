using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OscTree
{
	public class Route
	{
		public enum RouteType
		{
			ID,
			NAME,
		}
		public RouteType Type;

		public string ScreenName { get; set; } = string.Empty;

		private string originalName;
		public string OriginalName => originalName;
		public List<string> Steps;
		public int CurrentStep;

		public Route(string route, RouteType type)
		{
			Type = type;
			Steps = route.Split('/').Where(s => !string.IsNullOrEmpty(s)).ToList();
			originalName = route;
			CurrentStep = 0;
		}

		public string CurrentPart()
		{
			if (CurrentStep < Steps.Count) return Steps[CurrentStep];
			else return string.Empty;
		}
	}

	public class Routes : List<Route> {
		public void UpdateScreenNames(Tree root)
		{
			foreach(var elm in this)
			{
				elm.CurrentStep = 0;
				elm.ScreenName = root.GetNameOfRoute(elm);
			}
		}
	}
}
