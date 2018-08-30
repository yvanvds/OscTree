using System;
using System.Collections.Generic;
using System.Text;

namespace OscTree
{
	public class Endpoint : MarshalByRefObject
	{
		public Endpoint(string name, Action<object[]> action, params Type[] type)
		{
			this.name = name;
			this.action = action;

			if(type != null)
			foreach(var t in type)
			{
				validArgs.Add(t);
			}
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

		private readonly List<Type> validArgs = new List<Type>();
		public List<Type> ValidArgs => validArgs;

		internal IOscNode parent = null;
	}
}
