using System;
using System.Collections.Generic;
using System.Text;

namespace OscTree
{
	public interface IOscNode
	{
		Address Address { get; }
		Endpoints Endpoints { get; }
		NodeCollection Children { get; }

		void Send(Route route, object[] arguments);
		bool Deliver(Route route, object[] arguments);

		string GetRouteString(Route.RouteType type);
		string GetNameOfRoute(Route route);

		Endpoint GetEndpoint(Route route);

		void UpdateID(string oldID, string newID);
	}
}
