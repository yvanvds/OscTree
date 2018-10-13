using System;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using NUnitLite;

namespace OscTest
{
	[TestFixture]
	public class UnitTest1
	{
		[Test]
		public void TestRouteName()
		{
			OscTree.Route route = new OscTree.Route("/root/test/one", OscTree.Route.RouteType.NAME);
			Assert.That(route.OriginalName, Is.EqualTo("/root/test/one"));
		}

		[Test]
		public void TestRouteJsonSave()
		{
			OscTree.Route route = new OscTree.Route("/root/test/one", OscTree.Route.RouteType.NAME);
			JObject obj = route.ToJSON();
			Assert.That(obj, Is.Not.Null);

			route.Replacements = new System.Collections.Generic.Dictionary<int, string>();
			route.Replacements[1] = "client";
			obj = route.ToJSON();
			Assert.That(obj, Is.Not.Null);

			OscTree.Route check = new OscTree.Route(obj);
			Assert.That(check.OriginalName, Is.EqualTo("/root/test/one"));
			Assert.That(check.Replacements, Is.Not.Null);
			Assert.That(check.Replacements.ContainsKey(1), Is.True);
			Assert.That(check.Replacements[1], Is.EqualTo("client"));
		}
	}
}
