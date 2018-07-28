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

		bool Deliver(Route route, object[] arguments);

		string GetRouteString(Route.RouteType type);
	}
}
