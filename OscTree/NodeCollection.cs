using System;
using System.Collections.Generic;
using System.Text;

namespace OscTree
{
	public class NodeCollection
	{
		internal Dictionary<string, IOscNode> list = new Dictionary<string, IOscNode>();

		
		public bool Deliver(Route route, object[] arguments)
		{
			if(route.Type == Route.RouteType.ID)
			{
				if(list.ContainsKey(route.CurrentPart())) {
					route.CurrentStep++;
					return list[route.CurrentPart()].Deliver(route, arguments);
				}
			} else
			{
				foreach(var item in list.Values)
				{
					if(item.Address.Name.Equals(route.CurrentPart(), StringComparison.CurrentCultureIgnoreCase))
					{
						route.CurrentStep++;
						return item.Deliver(route, arguments);
					}
				}
			}
			return false;
		}
	}
}
