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

	public static void BlendBlock(Block target, World world, WorldPos position, float strength, float additive)
	{
		WorldPos [] points = new WorldPos[7];
		Block[] blocks = new Block[7];
		Vector3 [] offsets = new Vector3[7];
		Vector3[] globalPoints = new Vector3[7];
		Vector3 [] relativeOffsets = new Vector3[7];


		points [0] = new WorldPos(position.x,   position.y,		position.z); 	//prime
		points [1] = new WorldPos(position.x, 	position.y+1,	position.z); 	//above
		points [2] = new WorldPos(position.x, 	position.y-1,	position.z); 	//below
		points [3] = new WorldPos(position.x-1, position.y,		position.z);	//west
		points [4] = new WorldPos(position.x+1, position.y, 	position.z);	//east
		points [5] = new WorldPos(position.x, 	position.y, 	position.z+1);  //north
		points [6] = new WorldPos(position.x, 	position.y, 	position.z-1);	//south

		Block aboveDiagonal = 	world.GetBlock (position.x+1, 	position.y+1, 	position.z+1);
		Block diagonal = 		world.GetBlock (position.x+1, 	position.y, 	position.z+1);
		Block aboveNorth = 		world.GetBlock (position.x, 	position.y+1, 	position.z+1);
		Block aboveEast = 		world.GetBlock (position.x+1, 	position.y+1, 	position.z);
		Block north = 			world.GetBlock (position.x, 	position.y, 	position.z+1);
		Block east = 			world.GetBlock (position.x+1, 	position.y, 	position.z);
		Block above = 			world.GetBlock (position.x, 	position.y, 	position.z);

		//prepare offsets where we might use them
		for (int a = 0; a < 7; a += 1) {
			blocks [a] = world.GetBlock (points [a].x, points [a].y, points [a].z);
			offsets [a] = blocks [a].getoffset ();
			globalPoints [a] = new Vector3 (points [a].x + offsets [a].x, points [a].y + offsets [a].y, points [a].z + offsets [a].z);
			relativeOffsets [a] = globalPoints [a] - globalPoints [0];
		}

		//This next part tests the faces that may be associated with our target, or "prime" voxel.
		Vector3 vectorSum = new Vector3(.0f, .0f, .0f);
		int pointCount = 1;
		Vector3 faceDirection = new Vector3 (0f, 0f, 0f); //we will use this for adding and taking away ... hopefully

		if ((target.faces&1) != 0) //top face shares with point south and point west
		{
			vectorSum = vectorSum + new Vector3 (relativeOffsets [6].x, 0, relativeOffsets [6].z) + new Vector3 (relativeOffsets [3].x, 0, relativeOffsets [3].z);
			pointCount += 2;
			faceDirection.y += 1f;
		}
		else if((above.faces&2) != 0) {  //(face down of above shares same points)
			vectorSum = vectorSum + new Vector3 (relativeOffsets [6].x, 0, relativeOffsets [6].z) + new Vector3 (relativeOffsets [3].x, 0, relativeOffsets [3].z);
			pointCount += 2;
			faceDirection.y -= 1f;
		}

		if ((aboveDiagonal.faces&2) != 0) //down face (of diagonal) shares with target, east, and north
		{
			vectorSum = vectorSum + new Vector3 (relativeOffsets [4].x, 0, relativeOffsets [4].z) + new Vector3 (relativeOffsets [5].x, 0, relativeOffsets [5].z);
			pointCount += 2;
			faceDirection.y -= 1f;
		}
		else if((diagonal.faces&1) != 0) //face up of below the diagonal shares same
		{
			vectorSum = vectorSum + new Vector3 (relativeOffsets [4].x, 0, relativeOffsets [4].z) + new Vector3 (relativeOffsets [5].x, 0, relativeOffsets [5].z);
			pointCount += 2;
			faceDirection.y += 1f;
		}

		if ((aboveNorth.faces&2) != 0) //bottom face shares with point north and point west
		{
			vectorSum = vectorSum + new Vector3 (relativeOffsets [5].x, 0, relativeOffsets [5].z) + new Vector3 (relativeOffsets [3].x, 0, relativeOffsets [3].z);
			pointCount += 2;
			faceDirection.y -= 1f;
		}
		else if((north.faces&1) != 0) {  //(face up of north shares same points)
			vectorSum = vectorSum + new Vector3 (relativeOffsets [5].x, 0, relativeOffsets [5].z) + new Vector3 (relativeOffsets [3].x, 0, relativeOffsets [3].z);
			pointCount += 2;
			faceDirection.y += 1f;
		}

		if ((aboveEast.faces&2) != 0) //down face of east shares with target, east, and south
		{
			vectorSum = vectorSum + new Vector3 (relativeOffsets [4].x, 0, relativeOffsets [4].z) + new Vector3 (relativeOffsets [6].x, 0, relativeOffsets [6].z);
			pointCount += 2;
			faceDirection.y -= 1f;
		}
		else if((east.faces&1) != 0) //face up of east same
		{
			vectorSum = vectorSum + new Vector3 (relativeOffsets [4].x, 0, relativeOffsets [4].z) + new Vector3 (relativeOffsets [6].x, 0, relativeOffsets [6].z);
			pointCount += 2;
			faceDirection.y += 1f;
		}
		////faces on top and bottom

		if ((target.faces&4) != 0) //north face is shared with bottom point and west point it is also north's south face
		{
			vectorSum = vectorSum + new Vector3 (relativeOffsets [2].x, relativeOffsets [2].y, 0) + new Vector3 (relativeOffsets [3].x, relativeOffsets [3].y, 0);
			pointCount += 2;
			faceDirection.z += 1f;
		}
		else if((north.faces&8) != 0) //south face of the north block
		{
			vectorSum = vectorSum + new Vector3 (relativeOffsets [2].x, relativeOffsets [2].y, 0) + new Vector3 (relativeOffsets [3].x, relativeOffsets [3].y, 0);
			pointCount += 2;
			faceDirection.z -= 1f;
		}

		if ((aboveDiagonal.faces&8) != 0) //south facing (of diagonal) shares the face with target, and target east, and above target
		{
			vectorSum = vectorSum + new Vector3 (relativeOffsets [4].x, relativeOffsets [4].y, 0) + new Vector3 (relativeOffsets [1].x, relativeOffsets [1].y, 0);
			pointCount += 2;
			faceDirection.z -= 1f;
		}
		else if((aboveEast.faces&4) != 0)  //north face of above east
		{
			vectorSum = vectorSum + new Vector3 (relativeOffsets [4].x, relativeOffsets [4].y, 0) + new Vector3 (relativeOffsets [1].x, relativeOffsets [1].y, 0);
			pointCount += 2;
			faceDirection.z += 1f;
		}

		if ((east.faces&4) != 0) //north face of east is shared with bottom point and east point
		{
			vectorSum = vectorSum + new Vector3 (relativeOffsets [2].x, relativeOffsets [2].y, 0) + new Vector3 (relativeOffsets [4].x, relativeOffsets [4].y, 0);
			pointCount += 2;
			faceDirection.z += 1f;
		}
		else if((diagonal.faces&8) != 0) //south face of block below diagonal
		{
			vectorSum = vectorSum + new Vector3 (relativeOffsets [2].x, relativeOffsets [2].y, 0) + new Vector3 (relativeOffsets [4].x, relativeOffsets [4].y, 0);
			pointCount += 2;
			faceDirection.z -= 1f;
		}

		if ((aboveNorth.faces&8) != 0) //south facing of aboveNorth shares the face with aboveTarget
		{
			vectorSum = vectorSum + new Vector3 (relativeOffsets [3].x, relativeOffsets [3].y, 0) + new Vector3 (relativeOffsets [1].x, relativeOffsets [1].y, 0);
			pointCount += 2;
			faceDirection.z -= 1f;
		}
		else if((above.faces&4) != 0)  //north face of above 
		{
			vectorSum = vectorSum + new Vector3 (relativeOffsets [3].x, relativeOffsets [3].y, 0) + new Vector3 (relativeOffsets [1].x, relativeOffsets [1].y, 0);
			pointCount += 2;
			faceDirection.z += 1f;
		}
		////faces on north and south
		if ((target.faces&16) != 0) //east face of target is shared with bottom and south points 
		{
			vectorSum = vectorSum + new Vector3(0, relativeOffsets [2].y, relativeOffsets [2].z) + new Vector3(0, relativeOffsets [6].y, relativeOffsets [6].z) ;
			pointCount += 2;
			faceDirection.x += 1f;
		}
		else if((east.faces&32) != 0) //it is also the west face of east point
		{
			vectorSum = vectorSum + new Vector3(0, relativeOffsets [2].y, relativeOffsets [2].z) + new Vector3(0, relativeOffsets [6].y, relativeOffsets [6].z) ;
			pointCount += 2;
			faceDirection.x -= 1f;
		}

		if ((aboveDiagonal.faces&32) != 0) //west facing (of diagonal) shares target, north and above points
		{
			vectorSum = vectorSum + new Vector3(0, relativeOffsets [5].y, relativeOffsets [5].z) + new Vector3(0, relativeOffsets [1].y, relativeOffsets [1].z) ;
			pointCount += 2;
			faceDirection.x -= 1f;
		}
		else if((aboveNorth.faces&16) != 0) //east face of above north is same points
		{
			vectorSum = vectorSum + new Vector3(0, relativeOffsets [5].y, relativeOffsets [5].z) + new Vector3(0, relativeOffsets [1].y, relativeOffsets [1].z) ;
			pointCount += 2;
			faceDirection.x += 1f;
		}

		if ((north.faces&16) != 0) //east face of north block is shared with bottom and north points 
		{
			vectorSum = vectorSum + new Vector3(0, relativeOffsets [2].y, relativeOffsets [2].z) + new Vector3(0, relativeOffsets [5].y, relativeOffsets [5].z) ;
			pointCount += 2;
			faceDirection.x += 1f;
		}
		else if((diagonal.faces&32) != 0) //it is also the west face of belowDiagonal
		{
			vectorSum = vectorSum + new Vector3(0, relativeOffsets [2].y, relativeOffsets [2].z) + new Vector3(0, relativeOffsets [5].y, relativeOffsets [5].z) ;
			pointCount += 2;
			faceDirection.x -= 1f;
		}

		if ((aboveEast.faces&32) != 0) //west facing (of aboveeast) shares target, south and above points
		{
			vectorSum = vectorSum + new Vector3(0, relativeOffsets [6].y, relativeOffsets [6].z) + new Vector3(0, relativeOffsets [1].y, relativeOffsets [1].z) ;
			pointCount += 2;
			faceDirection.x -= 1f;
		}
		else if((above.faces&16) != 0) //east face of above is same points
		{
			vectorSum = vectorSum + new Vector3(0, relativeOffsets [6].y, relativeOffsets [6].z) + new Vector3(0, relativeOffsets [1].y, relativeOffsets [1].z) ;
			pointCount += 2;
			faceDirection.x += 1f;
		}
		strength = 0.45f;
		Vector3 vectorAverage = vectorSum / pointCount;
		faceDirection = Vector3.Min (Vector3.Max (faceDirection, new Vector3 (-1f, -1f, -1f)), new Vector3 (1f, 1f, 1f));
		//offsets [0] = Vector3.Min (Vector3.Max ((offsets [0] * (1 - strength)) + (vectorAverage * strength), Vector3.zero), Vector3.one);

		offsets [0] = offsets [0] + (vectorAverage*strength) + (faceDirection*additive);//((offsets [0] * (1f - strength)) + (vectorAverage * strength));
//		Debug.Log (vectorSum);
//		Debug.Log (pointCount);
//		Debug.Log (offsets [0]);
		//trying - if we have moved against the face, we have decreased
		//if we have moved with the face, we have increased
		//now how do we compare the direction with our landing???

		Block newBlock;
		Vector3 landingf = new Vector3 ((position.x + offsets [0].x), (position.y + offsets [0].y), (position.z + offsets [0].z));
		WorldPos landing = new WorldPos ((int)landingf.x, (int)landingf.y, (int)landingf.z);
		bool outside = false;

//		newBlock = world.GetBlock (landing.x, landing.y, landing.z);
//
//		if ((offsets [0].y) > 1.1f) { //we have moved up (landing is > position
//			newBlock.material = target.material;
//		} else if ((offsets [0].y) < -0.1f) { //we are moving down
//			target.material = blocks [2].material;
//		}
//		if ((offsets [0].x) > 1.1f) { //we moved east
//			newBlock.material = target.material;
//		} else if ((offsets [0].x) < -0.1f) { //we moved west
//			target.material = blocks [3].material;
//		}
//		if ((offsets [0].z) > 1.1f) { //we moved forward (north)
//			newBlock.material = target.material;
//		} else if ((offsets [0].z) < -0.1f) { //we moved south
//			target.material = blocks [6].material;
//		} 
//
//		if (outside) { //if you follow the code, you see that sometimes the material is not changed, but just the offsets of the landing block
//			newBlock.ClipOffset (offsets [0]); //sets as the remainder of the offset
//			world.SetBlock (landing.x, landing.y, landing.z, newBlock);
//		} 

		target.SnapOffset (offsets [0]);
		world.SetBlock (position.x, position.y, position.z, target);

	}
		
	public static bool applySphere(World world, Vector3 center, float radius)
	{
		WorldPos current_pos;
		WorldPos start_pos;
		WorldPos test_pos;
		Block current_block;
		Vector3 V_pos;
		Vector3 O_pos;
		Vector3 T_pos = new Vector3();
		float distanceVtoC;
		float distanceOtoC;
		float push;
		Vector3 V_push = new Vector3();
		Vector3 V_offset;
		Vector3 directionCtoV = new Vector3();

		int size = ((int)radius + 3) * 2; //this is to capture a cube of voxels
		int half = size / 2;

		Block[,,] brush = new Block[size,size,size]; //this is where we will save our changes before final

		center.Set (Mathf.Floor (center.x)+0.5f, Mathf.Floor (center.y)+0.5f, Mathf.Floor (center.z)+0.5f);

		start_pos = GetBlockPos(new Vector3(center.x+half-1,center.y+half-1,center.z+half-1));

		for (int a = size - 1; a > -1; a -= 1)
			for (int b = size - 1; b > -1; b -= 1)
				for (int c = size - 1; c > -1; c -= 1) {
					brush [a, b, c] = new Block ();
					current_pos = GetBlockPos (new Vector3 (center.x + a - half, center.y + b - half, center.z + c - half));
					current_block = world.GetBlock (current_pos.x, current_pos.y, current_pos.z);
				
					//world position of voxel
					O_pos = new Vector3 (current_block.offx + current_pos.x, current_block.offy + current_pos.y, current_block.offz + current_pos.z);
					//position of center of voxel
					V_pos = new Vector3 (0.5f + current_pos.x, 0.5f + current_pos.y, 0.5f + current_pos.z);

					distanceVtoC = Vector3.Distance (V_pos, center);
					//distanceOtoC = Vector3.Distance (O_pos, center);

					//normalized direction
					directionCtoV = (V_pos-center) / distanceVtoC;
					//we can multiply by our radius to find the point on sphere that is in line
					T_pos = center + (directionCtoV * radius);

					if (distanceVtoC > (radius)) { //marked as "outside"
						brush [a, b, c].material = -2; 
					} else if (distanceVtoC < (radius) ) { //marked as "inside"
						brush [a, b, c].material = 0; 
					} 

					if ((T_pos.x - current_pos.x) > 1.15f)
						continue;
					if ((T_pos.y - current_pos.y) > 1.15f)
						continue;
					if ((T_pos.z - current_pos.z) > 1.15f)
						continue;
					if ((T_pos.x - current_pos.x) < -0.15f)
						continue;
					if ((T_pos.y - current_pos.y) < -0.15f)
						continue;
					if ((T_pos.z - current_pos.z) < -0.15f)
						continue;

					brush [a, b, c].offx = Mathf.Clamp((T_pos.x) - (current_pos.x), 0.0f, 1.0f);
					brush [a, b, c].offy = Mathf.Clamp((T_pos.y) - (current_pos.y), 0.0f, 1.0f);
					brush [a, b, c].offz = Mathf.Clamp((T_pos.z) - (current_pos.z), 0.0f, 1.0f);

					brush [a, b, c].material = 1; //marked as "participating"


				}

		//second loop to find and cancel out "participating" voxels that need to NOT change material
		for(int a = size-1; a > -1; a-=1) 
			for(int b = size-1; b > -1; b-=1) 
				for(int c = size-1; c > -1; c-=1)
				{	
					current_pos = GetBlockPos(new Vector3(center.x+a-half,center.y+b-half,center.z+c-half));
					current_block = world.GetBlock(current_pos.x,current_pos.y,current_pos.z);
					if(brush[a,b,c].material == -2) { //this is outside, so any prime voxel connected needs to NOT change material
						//we will need to see if any connecting voxels are "participating"
						for (int d = 0; d < 2; d += 1) { 
							if (a + d > (size-1))
								continue;
							else
								for (int e = 0; e < 2; e += 1) {
									if (b + e > (size-1))
										continue;
									else
										for (int f = 0; f < 2; f += 1) {
											if (c + f > (size-1))
												continue;
											else if (brush [d+a, e+b, f+c].material == 1)
												brush [d+a, e+b, f+c].material = -1; //mark as "NOT-affected"
										}
								}
						}
									
					}

				}
		//third loop to act on our markings
		if (false) {
			for (int a = size - 1; a > -1; a -= 1)
				for (int b = size - 1; b > -1; b -= 1)
					for (int c = size - 1; c > -1; c -= 1) {

						current_pos = GetBlockPos (new Vector3 (center.x + a - half, center.y + b - half, center.z + c - half));
						current_block = world.GetBlock (current_pos.x, current_pos.y, current_pos.z);
						int flagmat = brush [a, b, c].material;

						if (flagmat == 0) { //"Inside"
							BlendBlock (current_block, world, current_pos, 0.2f, 0.02f);
							//current_block.material = 0; 
						} else if (flagmat == 1) { //"Participating" volume inside, voxel coord on the perimeter
							BlendBlock (current_block, world, current_pos, 0.1f, 0.02f);
							//current_block.material = 0;
							//current_block.setoffset (brush [a, b, c].offx, brush [a, b, c].offy, brush [a, b, c].offz);
						} else if (flagmat == -1) { //"NON-Affected" volume outside even though cord is on perimeter
							//current_block.setoffset (brush [a, b, c].offx, brush [a, b, c].offy, brush [a, b, c].offz);

						} else {

						}


						//SetBlock(world.GetChunk(current_pos.x,current_pos.y,current_pos.z), current_pos, current_block);
					}
		} else {
			for (int a = size - 1; a > -1; a -= 1)
				for (int b = size - 1; b > -1; b -= 1)
					for (int c = size - 1; c > -1; c -= 1) {
					
						current_pos = GetBlockPos (new Vector3 (center.x + a - half, center.y + b - half, center.z + c - half));
						current_block = world.GetBlock (current_pos.x, current_pos.y, current_pos.z);
						int flagmat = brush [a, b, c].material;

						if (flagmat == 0) { //"Inside"
							//BlendBlock (current_block, world, current_pos, 0.2f, -0.001f);
							current_block.material = 0; 
						} else if (flagmat == 1) { //"Participating"
							//BlendBlock (current_block, world, current_pos, 0.1f, -0.001f);
							current_block.material = 0;
							current_block.setoffset (brush [a, b, c].offx, brush [a, b, c].offy, brush [a, b, c].offz);
						} else if (flagmat == -1) { //"NON-Affected"
							current_block.setoffset (brush [a, b, c].offx, brush [a, b, c].offy, brush [a, b, c].offz);

						} else {
						
						}


						SetBlock(world.GetChunk(current_pos.x,current_pos.y,current_pos.z), current_pos, current_block);
					}
		}
		return true;
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