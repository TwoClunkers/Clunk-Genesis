using UnityEngine;
using System.Collections;

public static class Terra
{
	public static bool applySphere(World world, Vector3 center, float radius)
	{
		WorldPos current_pos;
		WorldPos last_pos;
		WorldPos start_pos;
		Block current_block;
		Block last_block;
		Vector3 V_pos;
		Vector3 V_distance;
		float distanceVtoC;
		Vector3 directionVtoC;
		float push;
		Vector3 V_push;
		Vector3 V_offset;
		int landing;

		int size = ((int)radius + 1) * 2; //this is to capture a cube of voxels
		int half = size / 2;

		Block[,,] brush = new Block[size,size,size]; //this is where we will save our changes before final

		start_pos = GetBlockPos(new Vector3(center.x+half-1,center.y+half-1,center.z+half-1));

		for(int a = size-1; a > -1; a-=1) 
			for(int b = size-1; b > -1; b-=1) 
				for(int c = size-1; c > 2; c-=1)
				{
					brush[a,b,c] = new Block();
					current_pos = GetBlockPos(new Vector3(center.x+a-half,center.y+b-half,center.z+c-half));
					current_block = world.GetBlock(current_pos.x,current_pos.y,current_pos.z);
				
					V_pos = new Vector3(0.5f + current_pos.x, 0.5f + current_pos.y, 0.5f + current_pos.z);
					distanceVtoC = Vector3.Distance(V_pos, center);
					push = radius/distanceVtoC;
					V_distance = V_pos-center;

					V_push = (V_distance)*push;
					V_pos = center+V_push;

					V_offset = new Vector3(V_pos.x - current_pos.x, V_pos.y - current_pos.y, V_pos.z - current_pos.z);

					landing = Terra.insideBound(V_offset,push);

					brush[a,b,c].offx = Mathf.Clamp(V_offset.x,0.0f,1.0f);
				    brush[a,b,c].offy = Mathf.Clamp(V_offset.y,0.0f,1.0f);
					brush[a,b,c].offz = Mathf.Clamp(V_offset.z,0.0f,1.0f);

					if(landing < 0) { //should be inside sphere
						brush[a,b,c].material = 2;
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
				
							brush[a,b,c].material = 2;
						}
						
					}


					//now to catch the voxels on edge, but with areas outside
					if((landing&8)>0) {
						brush[a,b,c+1].material = 0;
						if((landing&4)>0) {
						Debug.Log(landing);
							brush[a,b+1,c+1].material = 0;
							if((landing&2)>0 ) brush[a+1,b+1,c+1].material = 0;
						}
						if((landing&2)>0) brush[a+1,b,c+1].material = 0;
					}
					if((landing&4)>0) {
						brush[a,b+1,c].material = 0;
						if((landing&2) >0) brush[a+1,b+1,c].material = 0;
					}
					if((landing&2)>0) brush[a+1,b,c].material = 0;

				if(c<4) brush[a,b,c].material = 0;
				else if(c>6) brush[a,b,c].material = 0;
				}
		for(int a = size-1; a > -1; a-=1) 
			for(int b = size-1; b > -1; b-=1) 
				for(int c = size-1; c > 2; c-=1)
				{	
					current_pos = GetBlockPos(new Vector3(center.x+a-half,center.y+b-half,center.z+c-half));
					if(brush[a,b,c].material == 0) { //set offset, keep block
						current_block = world.GetBlock(current_pos.x,current_pos.y,current_pos.z);
						current_block.setoffset(brush[a,b,c].offx, brush[a,b,c].offy, brush[a,b,c].offz);
						SetBlock(world.GetChunk(current_pos.x,current_pos.y,current_pos.z), current_pos, current_block);
					}
					else{
						if(brush[a,b,c].material > 0) {
							int vposx = (Mathf.FloorToInt (((current_pos.x / 7.0f) - ((int)(current_pos.x / 7.0f))) * 7.0f));
							int vposy = (Mathf.FloorToInt (((current_pos.y / 7.0f) - ((int)(current_pos.y / 7.0f))) * 7.0f));
							if(vposx<0) vposx += 7;
							if(vposy<0) vposy += 7;
							brush[a,b,c].varientx = vposx;
							brush[a,b,c].varienty = vposy;
							SetBlock(world.GetChunk(current_pos.x,current_pos.y,current_pos.z), current_pos, brush[a,b,c]);
						}
					}
				}
		return true;
	}
	public static int insideBound(Vector3 offset, float push) {
		bool edge = true;
		int lastinrow = 0;
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

		if (edge)
			return 0; //exactly on the edge
		else
			return -1; //inside but not on edge

	}

    public static WorldPos GetBlockPos(Vector3 pos)
    {
        WorldPos blockPos = new WorldPos(
            Mathf.RoundToInt(pos.x),
            Mathf.RoundToInt(pos.y),
            Mathf.RoundToInt(pos.z)
            );

        return blockPos;
    }

    public static WorldPos GetBlockPos(RaycastHit hit, bool adjacent = false)
    {
        Vector3 pos = new Vector3(
            MoveWithinBlock(hit.point.x, hit.normal.x, adjacent),
            MoveWithinBlock(hit.point.y, hit.normal.y, adjacent),
            MoveWithinBlock(hit.point.z, hit.normal.z, adjacent)
            );

        return GetBlockPos(pos);
    }

    static float MoveWithinBlock(float pos, float norm, bool adjacent = false)
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

    public static bool SetBlock(RaycastHit hit, Block block, bool adjacent = false)
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
			Block newblock = new BlockAir ();
			newblock.offx = block.offx;
			newblock.offy = block.offy;
			newblock.offz = block.offz;
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
			block = new BlockAir();
			chunk.world.SetBlock(pos.x, pos.y, pos.z, block);
			return true;
		}
		return false;
	}

    public static Block GetBlock(RaycastHit hit, bool adjacent = false)
    {
        Chunk chunk = hit.collider.GetComponent<Chunk>();
        if (chunk == null)
            return null;

        WorldPos pos = GetBlockPos(hit, adjacent);

        Block block = chunk.world.GetBlock(pos.x, pos.y, pos.z);

        return block;
    }
}