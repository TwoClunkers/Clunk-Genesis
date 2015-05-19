
using System;
namespace DataObjects
{
	[Serializable]
	public class Split
	{
		public InventoryItem product;
		public InventoryItem[] components;
		public ProcessTypes process;

		public Split ()
		{
		}

	}
}

