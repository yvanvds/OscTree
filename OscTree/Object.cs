using System;
using System.Collections.Generic;
using System.Text;

namespace OscTree
{
	public interface IOscObject
	{
		Object OscObject { get; }
	}

	public class Object : IOscNode
	{
		private Address address;
		public Address Address => address;

		private Endpoints endpoints;
		public Endpoints Endpoints => endpoints;

		public NodeCollection Children => null;

		public Routes Targets = new Routes();

		public Object(Address address)
		{
			this.address = address;
			endpoints = new Endpoints(this);
		}

		~Object()
		{
			DetachFromParent();
		}

		public void DetachFromParent()
		{
			if (Address.parent != null)
			{
				if (Address.parent.Children.List.ContainsKey(Address.ID))
				{
					Address.parent.Children.List.Remove(Address.ID);
				}
				Address.parent = null;
			}
		}

		public void Send(object argument)
		{
			Send(new object[] { argument });
		}

		public void Send(object[] arguments)
		{
			if(Address.parent != null)
			{
				foreach(var target in Targets)
				{
					Address.parent.Send(target, arguments);
				}
			}
		}

		public void Send(Route route, object[] arguments) { } // not implemented

		public bool Deliver(Route route, object[] arguments)
		{
			if (route.Type == Route.RouteType.ID)
			{
				if (route.CurrentPart().Equals(address.ID))
				{
					route.CurrentStep++;
					if (Endpoints.Deliver(route, arguments)) return true;
				}
			}
			else
			{
				if (route.CurrentPart().Equals(address.Name))
				{
					route.CurrentStep++;
					if (Endpoints.Deliver(route, arguments)) return true;
				}
			}
			return false;
		}

		public string GetRouteString(Route.RouteType type)
		{
			string route = string.Empty;
			if (Address.parent != null)
			{
				route = Address.parent.GetRouteString(type);
			}
			route += "/";
			if (type == Route.RouteType.ID)
			{
				route += Address.ID;
			}
			else
			{
				route += Address.Name;
			}
			return route;
		}

		public string GetNameOfRoute(Route route)
		{
			if (route.Type == Route.RouteType.NAME) return route.OriginalName;

			if (route.Steps.Count > route.CurrentStep)
			{
				if (Address.ID.Equals(route.Steps[route.CurrentStep]))
				{
					if (route.Steps.Count > route.CurrentStep + 1)
					{
						string next = route.Steps[route.CurrentStep + 1];
						if (Endpoints.List.ContainsKey(next))
						{
							string result = GetRouteString(Route.RouteType.NAME);
							result += "/" + next;
							return result;
						}
					}
				}
			}

			return string.Empty;
		}

		public void UpdateID(string oldID, string newID)
		{
			
		}
	}
}
