using UnityEngine;
using System.Collections;
using DataObjects;

public static class Terra
{
	public static bool isCloser(Vector3 center, Vector3 pointOne, Vector3 pointTwo)
	{
		float distance1 = Vector3.SqrMagnitude (center - pointOne);
		float distance2 = Vector3.SqrMagnitude (center - pointTwo);
		if (distance1 < distance2)
			return true;
		else
			return false;
	}

	public static bool damageSphere(World world, Vector3 center, float radius, float power)
	{
		WorldPos current_pos;
		WorldPos start_pos;
		Block current_block;
		Vector3 V_pos;
		Vector3 V_distance;
		Vector3 O_pos;
		float distanceVtoC;
		float distanceOtoC;
		float push;
		Vector3 V_push = new Vector3();
		Vector3 V_offset;
		int landing;
		
		int size = ((int)radius + 2) * 2; //this is to capture a cube of voxels
		int half = size / 2;
		
		Block[,,] brush = new Block[size,size,size]; //this is where we will save our changes before final
		
		start_pos = GetBlockPos(new Vector3(center.x+half-1,center.y+half-1,center.z+half-1));
		
		for(int a = size-1; a > -1; a-=1) 
			for(int b = size-1; b > -1; b-=1) 
				for(int c = size-1; c > -1; c-=1)
			{
				brush[a,b,c] = new Block();
				current_pos = GetBlockPos(new Vector3(center.x+a-half,center.y+b-half,center.z+c-half));
				current_block = world.GetBlock(current_pos.x,current_pos.y,current_pos.z);
				
				V_pos = new Vector3(0.5f + current_pos.x, 0.5f + current_pos.y, 0.5f + current_pos.z);
				
				distanceVtoC = Vector3.Distance(V_pos, center);

				
				push = radius/distanceVtoC;
				V_distance = V_pos-center;
				V_push = (V_distance)*( Mathf.Pow(push, 3));
				V_pos = center+V_push;
				
				V_offset = new Vector3(V_pos.x - current_pos.x, V_pos.y - current_pos.y, V_pos.z - current_pos.z);
				
				landing = Terra.insideBound(V_offset, distanceVtoC, radius, radius+1.55f);
				
				brush[a,b,c].offx = Mathf.Clamp(V_offset.x,0.0f,1.0f);
				brush[a,b,c].offy = Mathf.Clamp(V_offset.y,0.0f,1.0f);
				brush[a,b,c].offz = Mathf.Clamp(V_offset.z,0.0f,1.0f);
				
				if(landing < 0) { //should be inside sphere
					brush[a,b,c].material = 1;
					landing = 0;
				}
				else { 
					if(landing > 0) {
						brush[a,b,c].material = -1; //should be outside, no change
					}
					else if(landing == 0)  //landed in voxel on edge
					{ 
						brush[a,b,c].offx = V_offset.x;
						brush[a,b,c].offy = V_offset.y;
						brush[a,b,c].offz = V_offset.z;
						
						brush[a,b,c].material = 1;
					}
					
				}
				
				if((landing&8)>0) {
					brush[a,b,c+1].material = 0;
					if((landing&4)>0) {
						brush[a,b+1,c+1].material = 0;
						if((landing&2)>0 ) brush[a+1,b+1,c+1].material = 0;
					}
					if((landing&2)>0)brush[a+1,b,c+1].material = 0;
				}
				else if((landing&4)>0) {
					brush[a,b+1,c].material = 0;
					if((landing&2) >0) brush[a+1,b+1,c].material = 0;
				}
				else if((landing&2)>0) brush[a+1,b,c].material = 0;
			}
		for(int a = size-1; a > -1; a-=1) 
			for(int b = size-1; b > -1; b-=1) 
				for(int c = size-1; c > -1; c-=1)
			{	
				current_pos = GetBlockPos(new Vector3(center.x+a-half,center.y+b-half,center.z+c-half));
				current_block = world.GetBlock(current_pos.x,current_pos.y,current_pos.z);
				O_pos = new Vector3(current_block.offx + current_pos.x, current_block.offx + current_pos.y, current_block.offx + current_pos.z);
				distanceOtoC = Vector3.Distance(O_pos, center);

				if(brush[a,b,c].material == 0) { //set offset, don't change block
					current_block.setoffset(brush[a,b,c].offx, brush[a,b,c].offy, brush[a,b,c].offz);
					SetBlock(world.GetChunk(current_pos.x,current_pos.y,current_pos.z), current_pos, current_block);
				}
				else{
					if(brush[a,b,c].material > 0) {
						current_block.setoffset(brush[a,b,c].offx, brush[a,b,c].offy, brush[a,b,c].offz);
						if(current_block.DamageBlock(current_pos, power*(radius-distanceOtoC)*500, V_push)) {
							Pickup newPickup = new Pickup();
							newPickup.reset(current_block.material, 1);
							newPickup.setPosition(new Vector3(current_pos.x+0.5f, current_pos.y+0.5f, 1.5f), Quaternion.identity);
							world.createPickUp(newPickup);
							//playsound
							current_block.material = 0;}
						SetBlock(world.GetChunk(current_pos.x,current_pos.y,current_pos.z), current_pos, current_block);
					}
					
					else {
						SetBlock(world.GetChunk(current_pos.x,current_pos.y,current_pos.z), current_pos, current_block);
					}
				}
			}
		return true;
	}

	public static bool applySphere(World world, Vector3 center, float radius)
	{
		WorldPos current_pos;
		WorldPos start_pos;
		Block current_block;
		Vector3 V_pos;
		Vector3 V_distance;
		Vector3 O_pos;
		float distanceVtoC;
		float distanceOtoC;
		float push;
		Vector3 V_push = new Vector3();
		Vector3 V_offset;
		int landing;

		int size = ((int)radius + 2) * 2; //this is to capture a cube of voxels
		int half = size / 2;

		Block[,,] brush = new Block[size,size,size]; //this is where we will save our changes before final

		start_pos = GetBlockPos(new Vector3(center.x+half-1,center.y+half-1,center.z+half-1));

		for(int a = size-1; a > -1; a-=1) 
			for(int b = size-1; b > -1; b-=1) 
				for(int c = size-1; c > -1; c-=1)
				{
					brush[a,b,c] = new Block();
					current_pos = GetBlockPos(new Vector3(center.x+a-half,center.y+b-half,center.z+c-half));
					current_block = world.GetBlock(current_pos.x,current_pos.y,current_pos.z);
				
					O_pos = new Vector3(current_block.offx + current_pos.x, current_block.offx + current_pos.y, current_block.offx + current_pos.z);
					V_pos = new Vector3(0.5f + current_pos.x, 0.5f + current_pos.y, 0.5f + current_pos.z);
					
					distanceVtoC = Vector3.Distance(V_pos, center);
					distanceOtoC = Vector3.Distance(O_pos, center);

					push = radius/distanceVtoC;
					V_distance = V_pos-center;
					V_push = (V_distance)*push;
					V_pos = center+V_push;

					V_offset = new Vector3(V_pos.x - current_pos.x, V_pos.y - current_pos.y, V_pos.z - current_pos.z);

					//if(distanceOtoC<distanceVtoC) {
						landing = Terra.insideBound(V_offset, distanceVtoC, radius, radius+1.55f);
						brush[a,b,c].offx = Mathf.Clamp(V_offset.x,0.0f,1.0f);
						brush[a,b,c].offy = Mathf.Clamp(V_offset.y,0.0f,1.0f);
						brush[a,b,c].offz = Mathf.Clamp(V_offset.z,0.0f,1.0f);
					//}
					//else landing = 1;
					

					if(landing < 0) { //should be inside sphere
						brush[a,b,c].material = 1;
						landing = 0;
					}
					else { 
						if(landing > 0) {
							brush[a,b,c].material = -1; //should be outside, no change
						}
						else if(landing == 0)  //landed in voxel on edge
						{ 
							
								brush[a,b,c].offx = V_offset.x;
								brush[a,b,c].offy = V_offset.y;
								brush[a,b,c].offz = V_offset.z;
							
							
							brush[a,b,c].material = 1;
						}
						
					}

				if((landing&8)>0) {
					brush[a,b,c+1].material = 0;
					if((landing&4)>0) {
						 brush[a,b+1,c+1].material = 0;
						if((landing&2)>0 ) brush[a+1,b+1,c+1].material = 0;
					}
					if((landing&2)>0)brush[a+1,b,c+1].material = 0;
				}
				else if((landing&4)>0) {
					 brush[a,b+1,c].material = 0;
					if((landing&2) >0) brush[a+1,b+1,c].material = 0;
				}
				else if((landing&2)>0) brush[a+1,b,c].material = 0;

//				if(distanceOtoC>(radius)) {
//					brush[a,b,c].offx = current_block.offx;
//					brush[a,b,c].offy = current_block.offy;
//					brush[a,b,c].offz = current_block.offz;
//
//					}
				}





		for(int a = size-1; a > -1; a-=1) 
			for(int b = size-1; b > -1; b-=1) 
				for(int c = size-1; c > -1; c-=1)
				{	
					current_pos = GetBlockPos(new Vector3(center.x+a-half,center.y+b-half,center.z+c-half));
					current_block = world.GetBlock(current_pos.x,current_pos.y,current_pos.z);
					if(brush[a,b,c].material == 0) { //set offset, don't change block
						current_block.setoffset(brush[a,b,c].offx, brush[a,b,c].offy, brush[a,b,c].offz);
						SetBlock(world.GetChunk(current_pos.x,current_pos.y,current_pos.z), current_pos, current_block);
					}
					else{
						if(brush[a,b,c].material > 0) {
							current_block.setvariant(current_pos.x,current_pos.y,current_pos.z);
							current_block.setoffset(brush[a,b,c].offx, brush[a,b,c].offy, brush[a,b,c].offz);
							current_block.material = 0;
							SetBlock(world.GetChunk(current_pos.x,current_pos.y,current_pos.z), current_pos, current_block);
						}
					
						else {
								SetBlock(world.GetChunk(current_pos.x,current_pos.y,current_pos.z), current_pos, current_block);
						}
					}
				}
		return true;
	}
	public static bool pushCircle(World world, Vector3 center, float radius, int material, float amount)
	{
		WorldPos current_pos;
		Block current_block;
		Vector3 V_pos;
		Vector3 O_pos;
		float distanceVtoC;
		Vector3 directionVtoC;
		Vector3 V_offset;
		
		int size = ((int)radius + 2) * 2; //this is to capture a cube of voxels
		int half = size / 2;
		
		Block[,,] brush = new Block[size,size,size]; //this is where we will save our changes before final
		for(int a = size-1; a > -1; a-=1) 
			for(int b = size-1; b > -1; b-=1) 
				for(int c = 2; c > -1; c-=1)
			{
				brush[a,b,c] = new Block();
				current_pos = GetBlockPos(new Vector3(center.x+a-half,center.y+b-half,center.z+c-1));
				current_block = world.GetBlock(current_pos.x,current_pos.y,current_pos.z);
				
				O_pos = new Vector3(current_block.offx + current_pos.x, current_block.offy + current_pos.y, current_block.offz + current_pos.z);
				V_pos = new Vector3(0.5f + current_pos.x, 0.5f + current_pos.y, 0.5f + center.z);
				
				distanceVtoC = Vector3.Distance(V_pos, center);
				directionVtoC = V_pos - center;
				Vector3 bump = directionVtoC/distanceVtoC; //normalized direction

				float amt = (radius-distanceVtoC)/radius;
				bump = bump*amt;

				V_pos = O_pos+bump;
				
				V_offset = new Vector3(V_pos.x - current_pos.x, V_pos.y - current_pos.y, V_pos.z - center.z);

				//landing = Terra.insideBound(V_offset, distanceVtoC, radius, radius+1.55f);

				brush[a,b,c].offx = Mathf.Clamp(V_offset.x,0.0f,1.0f);
				brush[a,b,c].offy = Mathf.Clamp(V_offset.y,0.0f,1.0f);
				brush[a,b,c].offz = Mathf.Clamp(V_offset.z,0.0f,1.0f);

				current_block.setoffset(brush[a,b,c].offx, brush[a,b,c].offy, brush[a,b,c].offz);
				SetBlock(world.GetChunk(current_pos.x,current_pos.y,current_pos.z), current_pos, current_block);

			}
		return true;
	}
	public static bool applyCircle(World world, Vector3 center, float radius, int material, float amount)
	{
		WorldPos current_pos;
		Block current_block;
		Vector3 V_pos;
		Vector3 V_direction;
		Vector3 O_pos;
		float distanceVtoC;
		float distanceOtoC;
		float push;
		Vector3 V_push = new Vector3 ();
		Vector3 V_offset;
		int landing;
		
		int size = ((int)radius + 2) * 2; //this is to capture a cube of voxels
		int half = size / 2;
		
		Block[,,] brush = new Block[size, size, size]; //this is where we will save our changes before final
		
		for (int a = size-1; a > -1; a-=1) {
			for (int b = size-1; b > -1; b-=1) {
				for (int c = size-1; c > -1; c-=1) {
					brush [a, b, c] = new Block ();
					current_pos = GetBlockPos (new Vector3 (center.x + a - half, center.y + b - half, center.z + c - half));
					current_block = world.GetBlock (current_pos.x, current_pos.y, current_pos.z);
				
					O_pos = new Vector3 (current_block.offx + current_pos.x, current_block.offy + current_pos.y, current_block.offz + current_pos.z);
					V_pos = new Vector3 (0.5f + current_pos.x, 0.5f + current_pos.y, 0.5f + center.z);
				
					distanceVtoC = Vector3.Distance (V_pos, center);
					distanceOtoC = Vector3.Distance (O_pos, center);
				
					push = (radius / distanceVtoC);
				
					V_direction = V_pos - center;
					V_push = (V_direction) * push;


					V_pos.Set (center.x + V_push.x, center.y + V_push.y, center.z + V_push.z);

					//if (distanceOtoC > radius) V_offset = current_block.getoffset();
					V_offset = new Vector3 (V_pos.x - current_pos.x, V_pos.y - current_pos.y, V_pos.z - center.z);
				
					landing = Terra.insideBound (V_offset, distanceVtoC, radius, radius + 1.55f);
				
				
					brush [a, b, c].offx = Mathf.Clamp (V_offset.x, 0.0f, 1.0f);
					brush [a, b, c].offy = Mathf.Clamp (V_offset.y, 0.0f, 1.0f);
					brush [a, b, c].offz = Mathf.Clamp (V_offset.z, 0.0f, 1.0f);
				
					if (landing < 0) { //should be inside sphere
						brush [a, b, c].material = 1;
						landing = 0;
					} else { 
						if (landing > 0) {
							brush [a, b, c].material = -1; //should be outside, no change
						} else if (landing == 0) {  //landed in voxel on edge 
							brush [a, b, c].offx = V_offset.x;
							brush [a, b, c].offy = V_offset.y;
							brush [a, b, c].offz = V_offset.z;
							brush [a, b, c].material = 1;
						}
					
					}

					if ((landing & 8) > 0) {

						brush [a, b, c + 1].material = 0;
						if ((landing & 4) > 0) {
							brush [a, b + 1, c + 1].material = 0;
							if ((landing & 2) > 0)
								brush [a + 1, b + 1, c + 1].material = 0;
						}
						if ((landing & 2) > 0)
							brush [a + 1, b, c + 1].material = 0;
					} else if ((landing & 4) > 0) {
						brush [a, b + 1, c].material = 0;
						if ((landing & 2) > 0)
							brush [a + 1, b + 1, c].material = 0;
					} else if ((landing & 2) > 0)
						brush [a + 1, b, c].material = 0;
					if (c == 0)
						brush [a, b, c].material = 0;
				}
			}
		}
		
		for (int a = size-1; a > -1; a-=1) {
			for (int b = size-1; b > -1; b-=1) {
				for (int c = size-1; c > -1; c-=1) {	
					current_pos = GetBlockPos (new Vector3 (center.x + a - half, center.y + b - half, center.z + c - half));
					current_block = world.GetBlock (current_pos.x, current_pos.y, current_pos.z);
					if (brush [a, b, c].material == 0) { //set offset, don't change block
						current_block.setoffset (brush [a, b, c].offx, brush [a, b, c].offy, brush [a, b, c].offz);
						SetBlock (world.GetChunk (current_pos.x, current_pos.y, current_pos.z), current_pos, current_block);
					} else {
						if (brush [a, b, c].material > 0) { //signals inside radius
							if (material == -2) { //damage
								if (current_block.DamageBlock (current_pos, amount, Vector3.zero)) {
									Pickup newPickup = new Pickup ();
									newPickup.reset (current_block.material, 1);
									newPickup.setPosition (new Vector3 (current_pos.x + 0.5f, current_pos.y + 0.5f, 1.5f), Quaternion.identity);
									world.createPickUp (newPickup);
									//playsound
									current_block.material = 0;
								}
								SetBlock (world.GetChunk (current_pos.x, current_pos.y, current_pos.z), current_pos, current_block);
							} else if (material == -3) { //paint? place?
								current_block.setoffset (brush [a, b, c].offx, brush [a, b, c].offy, brush [a, b, c].offz);
								current_block.material = 0;
								SetBlock (world.GetChunk (current_pos.x, current_pos.y, current_pos.z), current_pos, current_block);
							} else { //paint? place?
								int vposx = (Mathf.FloorToInt (((current_pos.x / 7.0f) - ((int)(current_pos.x / 7.0f))) * 7.0f));
								int vposy = (Mathf.FloorToInt (((current_pos.y / 7.0f) - ((int)(current_pos.y / 7.0f))) * 7.0f));
								if (vposx < 0)
									vposx += 7;
								if (vposy < 0)
									vposy += 7;
								brush [a, b, c].varientx = vposx;
								brush [a, b, c].varienty = vposy;
								current_block.setoffset (brush [a, b, c].offx, brush [a, b, c].offy, brush [a, b, c].offz);
								current_block.material = material;
								SetBlock (world.GetChunk (current_pos.x, current_pos.y, current_pos.z), current_pos, current_block);
							}
						} else {
							SetBlock (world.GetChunk (current_pos.x, current_pos.y, current_pos.z), current_pos, current_block);
						}
					}
				}
			}
		}
		return true;
	}
	public static int insideBound(Vector3 offset, float distance, float radius, float bound) {
		float push = radius/distance;
		bool edge = true;
		int lastinrow = 0;
		if (distance > bound)
			return 1;

		if(offset.x<(0.0)) {
			edge = false;
			if(push<1.0) lastinrow |= 1; //pulling, so outside
		}
		else if(offset.x>1.0) {
			edge = false;
			if(push<1.0) lastinrow |= 2; //pulling, so outside
		}

		//now for the y coord
		if(offset.y<(0.0)) {
			edge = false;
			if(push<1.0) lastinrow |= 1; //pulling, so outside
		}
		else if(offset.y>1.0) {
			edge = false;
			if(push<1.0) lastinrow |= 4; //pulling, so outside
		}

		//now for the z coord
		if(offset.z<(0.0)) {
			edge = false;
			if(push<1.0) lastinrow |= 1; //pulling, so outside
		}
		else if(offset.z>1.0) {
			edge = false;
			if(push<1.0) lastinrow |= 8; //pulling, so outside
		}

		if (lastinrow > 0)
			return lastinrow;

		//if we get here, all three coords are either inside (push>1) or on the edge (within offset bounds)

		if (edge) {
			return 0; //exactly on the edge
		}
		else
			return -1; //inside but not on edge

	}

    public static WorldPos GetBlockPos(Vector3 pos)
    {
        WorldPos blockPos = new WorldPos(
            Mathf.FloorToInt(pos.x),
			Mathf.FloorToInt(pos.y),
			Mathf.FloorToInt(pos.z)
            );

        return blockPos;
    }

    public static WorldPos GetBlockPos(RaycastHit hit, bool adjacent)
    {
        Vector3 pos = new Vector3(
            MoveWithinBlock(hit.point.x, hit.normal.x, adjacent),
            MoveWithinBlock(hit.point.y, hit.normal.y, adjacent),
            MoveWithinBlock(hit.point.z, hit.normal.z, adjacent)
            );

        return GetBlockPos(pos);
    }

    static float MoveWithinBlock(float pos, float norm, bool adjacent)
    {
        if (pos - (int)pos == 0.5f || pos - (int)pos == -0.5f)
        {
//            if (adjacent)
//            {
//                pos += (norm / 2);
//            }
//            else
//            {
//                pos -= (norm / 2);
//            }
        }

        return (float)pos;
    }

	public static bool SetBlock(Chunk chunk, WorldPos pos, Block block)
	{
		if (chunk == null)
			return false;

		chunk.world.SetBlock(pos.x, pos.y, pos.z, block);
		
		return true;
	}

    public static bool SetBlock(RaycastHit hit, Block block, bool adjacent)
    {
        Chunk chunk = hit.collider.GetComponent<Chunk>();
        if (chunk == null)
            return false;

        WorldPos pos = GetBlockPos(hit, adjacent);

        chunk.world.SetBlock(pos.x, pos.y, pos.z, block);

        return true;
    }

	public static bool DamageBlock(Chunk chunk, WorldPos pos, float amount, Vector3 direction)
	{
		if (chunk == null)
			return false;
		
		Block block = chunk.world.GetBlock(pos.x, pos.y, pos.z);
		if (block.DamageBlock (pos, amount, direction)) {
			Block newblock = new Block();
			newblock.offx = block.offx;
			newblock.offy = block.offy;
			newblock.offz = block.offz;
			newblock.material = 0;
			block = newblock;
			block.changed = true;
			chunk.world.SetBlock (pos.x, pos.y, pos.z, block);
			return true;
		} else {
			block.changed = true;
			block.material = 3;
			chunk.world.SetBlock (pos.x, pos.y, pos.z, block);
		}
		return false;
	}

	public static bool DamageBlock(RaycastHit hit, float amount, Vector3 direction)
	{
		Chunk chunk = hit.collider.GetComponent<Chunk>();
		if (chunk == null)
			return false;
		
		WorldPos pos = GetBlockPos(hit, false);
		
		Block block = chunk.world.GetBlock(pos.x, pos.y, pos.z);
		if(block.DamageBlock(pos,amount,direction)) {
			block = new Block();
			block.material = 0;
			chunk.world.SetBlock(pos.x, pos.y, pos.z, block);
			return true;
		}
		return false;
	}

    public static Block GetBlock(RaycastHit hit, bool adjacent)
    {
        Chunk chunk = hit.collider.GetComponent<Chunk>();
        if (chunk == null)
            return null;

        WorldPos pos = GetBlockPos(hit, adjacent);

        Block block = chunk.world.GetBlock(pos.x, pos.y, pos.z);

        return block;
    }
}