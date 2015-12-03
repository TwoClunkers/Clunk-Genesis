using System;
namespace DataObjects
{
	public class StatBlock
	{
		public float stability; //health what it would take to destroy us now
		public float energy; //currently stored energy
		public float fluid; //physical allocation for action
		
		//base stats
		public float durability; //a measure of the physical stresses that can be tolerated 1 - n
		public float capacitance; //the ability to store power 0 - n
		public float volume; //physical storage 0 - n
		public float aptitude; //ability to do / fitness for task 0-100%
		public float precision; //measurement of fidelity 0-100%

		public StatBlock ()
		{
		}
	}
}

