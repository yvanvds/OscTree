using System;
using System.Collections.Generic;
using System.Text;

namespace OscTree
{
	public class Address
	{
		public string Name { get; set; }
		public string ID { get; set; }

		internal IOscNode parent = null;
		private IOscNode obj = null;

		public Address(string name) 
			: this(name, Utils.GenerateID())
		{}

		public Address(string name, string id)
		{
			Name = name;
			ID = id;
		}
	}
}
