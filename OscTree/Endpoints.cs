using System;
using System.Collections.Generic;
using System.Text;

namespace OscTree
{
	public class Endpoints
	{
		private Dictionary<string, Endpoint> list = new Dictionary<string, Endpoint>();
		public Dictionary<string, Endpoint> List => list;

		private IOscNode parent;

		public Endpoints(IOscNode parent)
		{
			this.parent = parent;
		}

		public void Add(Endpoint point)
		{
			if(!list.ContainsKey(point.Name))
			{
				point.parent = parent;
				list.Add(point.Name, point);
			}
		}

		public bool Deliver(Route route, object[] arguments)
		{
			if (list.ContainsKey(route.CurrentPart()))
			{
				list[route.CurrentPart()].Action(arguments);
				return true;
			}
			return false;
		}
	}
}
