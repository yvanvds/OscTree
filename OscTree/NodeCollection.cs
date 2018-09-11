using System;
using System.Collections.Generic;
using System.Text;

namespace OscTree
{
	public class NodeCollection : MarshalByRefObject
	{
		private Dictionary<string, IOscNode> list = new Dictionary<string, IOscNode>();
		public Dictionary<string, IOscNode> List => list;
		
		public void UpdateID(string oldID, string newID)
		{
			if (List.ContainsKey(oldID))
			{
				var node = List[oldID];
				List.Add(newID, node);
				List.Remove(oldID);
			}
		}

		public bool Deliver(Route route, object[] arguments)
		{
			if(route.Type == Route.RouteType.ID)
			{
				string key = route.CurrentPart();
				if(List.ContainsKey(key)) {
					//route.CurrentStep++;
					return List[key].Deliver(route, arguments);
				}
			} else
			{
				foreach(var item in List.Values)
				{
					if(item.Address.Name.Equals(route.CurrentPart(), StringComparison.CurrentCultureIgnoreCase))
					{
						//route.CurrentStep++;
						return item.Deliver(route, arguments);
					}
				}
			}
			return false;
		}

		public Endpoint GetEndpoint(Route route)
		{
			if(route.Type == Route.RouteType.ID)
			{
				string key = route.CurrentPart();
				if(List.ContainsKey(key))
				{
					return List[key].GetEndpoint(route);
				} else
				{
					foreach(var item in List.Values)
					{
						if (item.Address.Name.Equals(route.CurrentPart(), StringComparison.CurrentCultureIgnoreCase))
						{
							return item.GetEndpoint(route);
						}
					}
				}
			}
			return null;
		}

		public bool Contains(IOscNode node)
		{
			foreach(var item in List.Values)
			{
				if (item == node) return true;
				if(item is Tree)
				{
					if ((item as Tree).Contains(node)) return true;
				}
			}
			return false;
		}
	}
}
