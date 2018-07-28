using System;
using System.Collections.Generic;
using System.Text;

namespace OscTree
{
	public class Tree : IOscNode
	{
		private Address address;
		public Address Address => address;

		public Endpoints Endpoints => new Endpoints(this);

		public NodeCollection Children => new NodeCollection();

		public Tree(Address address)
		{
			this.address = address;
		}

		public void Add(IOscNode node)
		{
			node.Address.parent = this;
			Children.list.Add(node.Address.ID, node);
		}

		public void Remove(IOscNode node)
		{
			if (Children.list.ContainsKey(node.Address.ID))
			{
				Children.list.Remove(node.Address.ID);
			}
		}

		public bool Deliver(Route route, object[] arguments)
		{
			if(route.Type == Route.RouteType.ID)
			{
				if (route.CurrentPart().Equals(address.ID))
				{
					route.CurrentStep++;
					if (Endpoints.Deliver(route, arguments)) return true;
					if (Children.Deliver(route, arguments)) return true;
				}
			} else
			{
				if(route.CurrentPart().Equals(address.Name))
				{
					route.CurrentStep++;
					if (Endpoints.Deliver(route, arguments)) return true;
					if (Children.Deliver(route, arguments)) return true;
				}
			}
			return false;
		}

		public string GetRouteString(Route.RouteType type)
		{
			string route = string.Empty;
			if(Address.parent != null)
			{
				route = Address.parent.GetRouteString(type);
			}
			route += "/";
			if (type == Route.RouteType.ID)
			{
				route += Address.Name;
			} else
			{
				route += Address.ID;
			}
			return route;
		}
	}
}
