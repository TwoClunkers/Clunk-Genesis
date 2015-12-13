
using System;
namespace DataObjects
{
	public class PartTag
	{
		public int index; //the index of this part in the schematic
		public int level; //how many steps from the parent (0 = we are parent)
		public int parent; //the index of the part we are connected to
		public int node; //which attachment we are on


		
		public PartTag ()
		{
		}
		
		public void set (int newIndex, int newLevel, int newConnect, int newNodeIndex)
		{
			index = newIndex;
			level = newLevel;
			parent = newConnect;
			node = newNodeIndex;
		}


	}
}

