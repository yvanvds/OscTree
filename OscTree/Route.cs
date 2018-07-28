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
}
