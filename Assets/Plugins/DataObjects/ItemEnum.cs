
using System;
namespace DataObjects
{
	public enum ItemTypes {
		place_fore,
		place_back,
		equip_body,
		equip_head,
		equip_drive,
		equip_arm,
		equip_arm_buff,
		equip_drive_buff,
		equip_head_buff,
		equip_body_buff,
		equip_buff,
		equip_internal,
		consumable,
		material,
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
}

