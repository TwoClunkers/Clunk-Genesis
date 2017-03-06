
using System;
namespace DataObjects
{
	public enum ItemGroup {
		unused,
		assortment,
		substance,
		component,
		part,
		assembly,
		unit
	}
	public enum ItemTypes {
		unused,
		bot_core,
		bot_drive,
		bot_chassis,
		bot_apex
	}
	public enum ProcessTypes {
		mine,
		combine,
		fabricate,
		assemble,
		extract,
		smelt,
		forge
	}
	public enum MathTypes {
		add,
		subtract,
		multiply,
		divide,
		max,
		min
	}
	public enum Mode { 
		voxel, 
		plane, 
		articles
	}
	public enum Shape {	
		sphere, 
		cube, 
		voxel	
	}
	public enum App { 
		subtractive, 
		additive, 
		blend
	}

}

