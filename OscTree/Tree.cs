using System;
using System.Collections.Generic;
using System.Text;

namespace OscTree
{
	public delegate void ErrorHandler(string message);
	public delegate void ReRoute(Route route, object[] arguments);
	public delegate object ValueOverrideHandler(string methodName, object[] arguments);

	public class Tree : MarshalByRefObject, IOscNode
	{
		private Address address;
		public Address Address => address;

		public Endpoints endpoints;
		public Endpoints Endpoints => endpoints;

		private NodeCollection children = new NodeCollection();
		public NodeCollection Children => children;

		public ErrorHandler ErrorHandler = null;
		public ReRoute ReRoute = null;
		public ValueOverrideHandler ValueOverrideHandler = null;

		private bool ignoreInGui = false;
		public bool IgnoreInGui { get => ignoreInGui; set => ignoreInGui = value; }

		public Tree(Address address)
		{
			endpoints = new Endpoints(this);
			this.address = address;
			//this.address.obj = this;
		}

		~Tree()
		{
			DetachFromParent();
		}

		public void Add(IOscNode node)
		{
			node.Address.parent = this;
			Children.List.Add(node.Address.ID, node);
		}

		public void Remove(IOscNode node)
		{
			if (Children.List.ContainsKey(node.Address.ID))
			{
				Children.List.Remove(node.Address.ID);
			}
		}

		public void DetachFromParent()
		{
			if(Address.parent != null)
			{
				if(Address.parent.Children.List.ContainsKey(Address.ID))
				{
					Address.parent.Children.List.Remove(Address.ID);
				}
				Address.parent = null;
			}
		}

		public void Send(Route route, object[] arguments)
		{
			if(Address.parent != null)
			{
				Address.parent.Send(route, arguments);
			} else
			{
				if (route.ValueOverrideMethodName != null && route.ValueOverrideMethodName != string.Empty && ValueOverrideHandler != null)
				{
					var result = ValueOverrideHandler(route.ValueOverrideMethodName, arguments);
					arguments = new object[] { result };
				}

				route.CurrentStep = 0;
				Deliver(route, arguments);
			}
		}

		public bool Deliver(Route route, object[] arguments)
		{
			if(ReRoute != null)
			{
				ReRoute.Invoke(route, arguments);
				return true;
			}

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

			if(ErrorHandler != null)
			{
				ErrorHandler.Invoke("Tree " + Address.Name + "Cannot deliver to route " + route.OriginalName);
			}

			return false;
		}

		public Endpoint GetEndpoint(Route route)
		{
			if(route.Type == Route.RouteType.ID)
			{
				if(route.CurrentPart().Equals(address.ID))
				{
					route.CurrentStep++;
					Endpoint endpoint = Endpoints.GetEndpoint(route);
					if (endpoint != null) return endpoint;

					endpoint = Children.GetEndpoint(route);
					if (endpoint != null) return endpoint;
				}
			} else
			{
				if(route.CurrentPart().Equals(address.Name))
				{
					route.CurrentStep++;
					Endpoint endpoint = Endpoints.GetEndpoint(route);
					if (endpoint != null) return endpoint;

					endpoint = Children.GetEndpoint(route);
					if (endpoint != null) return endpoint;
				}
			}
			return null;
		}

		public bool Contains(IOscNode node)
		{
			return Children.Contains(node);
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
				route += Address.ID;
			} else
			{
				route += Address.Name;
			}
			return route;
		}

		public string GetNameOfRoute(Route route)
		{
			if (route.Type == Route.RouteType.NAME) return route.OriginalName;

			if(route.Steps.Count > route.CurrentStep)
			{
				if(Address.ID.Equals(route.Steps[route.CurrentStep]))
				{
					if (route.Steps.Count > route.CurrentStep + 1)
					{
						string next = route.Steps[route.CurrentStep + 1];
						if (Endpoints.List.ContainsKey(next))
						{
							string result = GetRouteString(Route.RouteType.NAME);
							result += "/" + next;
							return result;
						} else if (Children.List.ContainsKey(next)) 
						{
							route.CurrentStep++;
							return Children.List[next].GetNameOfRoute(route);
						}
					}
				}
			}

			return string.Empty;
		}

		public void UpdateID(string oldID, string newID)
		{
			Children.UpdateID(oldID, newID);
		}
	}
}
