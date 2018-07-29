﻿using System;
using System.Collections.Generic;
using System.Text;

namespace OscTree
{
	public class NodeCollection
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
	}
}
