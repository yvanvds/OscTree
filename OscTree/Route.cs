﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OscTree
{
	public class Route : MarshalByRefObject
	{
		public enum RouteType
		{
			ID,
			NAME,
		}
		public RouteType Type;

		public string ScreenName { get; set; } = string.Empty;

		private string originalName;
		public string OriginalName => originalName;
		public List<string> Steps;
		public int CurrentStep;

		public Dictionary<int, string> Replacements = null;
		public string ValueOverrideMethodName { get; set; } = string.Empty;

		public Route(string route, RouteType type)
		{
			Type = type;
			Steps = route.Split('/').Where(s => !string.IsNullOrEmpty(s)).ToList();
			originalName = route;
			CurrentStep = 0;
		}

		public Route(JObject obj)
		{
			if(obj.ContainsKey("Type")) {
				if(((string)obj["Type"]).Equals("NAME",StringComparison.CurrentCultureIgnoreCase)) {
					Type = RouteType.NAME;
				} else
				{
					Type = RouteType.ID;
				}
			}

			if(obj.ContainsKey("Name"))
			{
				originalName = (string)obj["Name"];
				Steps = originalName.Split('/').Where(s => !string.IsNullOrEmpty(s)).ToList();
				CurrentStep = 0;
			}

			if(obj.ContainsKey("Method"))
			{
				ValueOverrideMethodName = (string)obj["Method"];
			}

			if(obj.ContainsKey("Replacements"))
			{
				var replacements = obj["Replacements"] as JObject;
				if(replacements.Count > 0)
				{
					Replacements = new Dictionary<int, string>();
					foreach(var replacement in replacements)
					{
						Replacements[Convert.ToInt32(replacement.Key)] = (string)replacement.Value;
					}
				}
			}
		}

		public string CurrentPart()
		{
			if (CurrentStep < Steps.Count)
			{
				if(Replacements != null && Replacements.ContainsKey(CurrentStep))
				{
					return Replacements[CurrentStep];
				}
				return Steps[CurrentStep];
			}
			else return string.Empty;
		}

		// route with replacements
		public string GetActualRoute()
		{
			string result = string.Empty;
			for(int i = 0; i < Steps.Count; i++)
			{
				result += "/";
				if (Replacements != null && Replacements.ContainsKey(i)) result += Replacements[i];
				else result += Steps[i];
			}
			return result;
		}

		public JObject ToJSON()
		{
			var result = new JObject();
			result["Name"] = OriginalName;
			result["Type"] = Type.ToString();
			result["Method"] = ValueOverrideMethodName;
			if(Replacements != null)
			{
				var replacements = new JObject();
				foreach(var replacement in Replacements)
				{
					replacements[replacement.Key.ToString()] = replacement.Value;
				}
				result["Replacements"] = replacements;
			}

			return result;
		}


	}

	public class Routes : List<Route> {
		public void UpdateScreenNames(Tree root)
		{
			foreach(var elm in this)
			{
				if(elm.Replacements == null)
				{
					elm.CurrentStep = 0;
					elm.ScreenName = root.GetNameOfRoute(elm);
				} else
				{
					elm.CurrentStep = 0;
					OscTree.Route route = new OscTree.Route(root.GetNameOfRoute(elm), OscTree.Route.RouteType.NAME);
					route.Replacements = new System.Collections.Generic.Dictionary<int, string>();
					foreach(var replacement in elm.Replacements)
					{
						route.Replacements[replacement.Key] = replacement.Value;
					}
					elm.ScreenName = route.GetActualRoute();
				}
				
			}
		}
	}
}
