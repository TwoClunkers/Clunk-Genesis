using System;
namespace DataObjects
{
	[Serializable]
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

		public StatBlock getCopy()
		{
			StatBlock statCopy = new StatBlock ();

			statCopy.stability = stability;
			statCopy.energy = energy;
			statCopy.fluid = fluid;
			statCopy.durability = durability;
			statCopy.capacitance = capacitance;
			statCopy.volume = volume;
			statCopy.aptitude = aptitude;
			statCopy.precision = precision;

			return statCopy;
		}
	}
}

