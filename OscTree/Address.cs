using System;
using System.Collections.Generic;
using System.Text;

namespace OscTree
{
	public class Address
	{
		public string Name { get; set; }

		private string id = string.Empty;
		public string ID
		{
			get => id;
			set
			{
				if(id != string.Empty && parent != null)
				{
					parent.UpdateID(id, value);
				}
				id = value;
			}
		}

		internal IOscNode parent = null;
		public IOscNode obj = null;

		public Address(string name) 
			: this(name, Utils.GenerateID())
		{}

		public Address(string name, string id)
		{
			Name = name;
			ID = id;
		}

		public int TreeLevel()
		{
			if (parent == null) return 0;
			return parent.Address.TreeLevel() + 1;
		}

	}
}
