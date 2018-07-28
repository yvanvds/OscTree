using System;
using System.Collections.Generic;
using System.Text;

namespace OscTree
{
	public class Endpoint
	{
		public Endpoint(string name, Action<object[]> action)
		{
			this.name = name;
			this.action = action;
		}

		public Route GetRoute(Route.RouteType type)
		{
			string route = string.Empty;
			if(parent != null)
			{
				route = parent.GetRouteString(type);
			}
			route += "/" + name;
			return new Route(route, type);
		}

		private string name;
		public string Name => name;

		private Action<object[]> action;
		public Action<object[]> Action => action;

		internal IOscNode parent = null;
	}
}
