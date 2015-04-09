#pragma strict

function Start () {

}

function Update () {

}

function placeBlock() {
/*
	// place block by raycasting + active inv slot
	var playerInv : PlayerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent(PlayerInventory);
	if(playerInv.activeInventorySlot == -1) return;
	var activeBlock : Item = items.library[playerInv.contents[playerInv.activeInventorySlot].id];
	//TODO: get actual number of blocks needed based on maxHealth of block
	var qtyNeeded : int = (activeBlock.maxHealth / 25);
	if(playerInv.hasItem(activeBlock.id,qtyNeeded) == -1) return;
	var ray : Ray;
	var hit : RaycastHit;
	var trueHit : RaycastHit;
	var playerEye : Transform = GameObject.FindGameObjectWithTag("Player").Find("laserSource").transform;
	var newBlockPosition : Vector3;
	
	ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	if(Physics.Raycast(ray, hit, 100.0)) {
		//mouse position hit something
		Debug.DrawLine(playerEye.position, hit.point, Color.yellow,2.0);
		var layMask : LayerMask;
		layMask = (1<<0);
		if(Physics.Raycast(Ray(Vector3(playerEye.position.x,playerEye.position.y), (Vector3(hit.point.x,hit.point.y) - Vector3(playerEye.position.x,playerEye.position.y)).normalized), trueHit, 3.0, layMask)){
			//raycast from player hit a block
			Debug.DrawLine(playerEye.position, trueHit.point, Color.white,2.0);	
			//TODO: figure out where to put the block
			var xMax : float = trueHit.collider.bounds.center.x + trueHit.collider.bounds.extents.x;
			var xMin : float = trueHit.collider.bounds.center.x - trueHit.collider.bounds.extents.x;
			if(trueHit.point.x < xMax && trueHit.point.x > xMin ){
				//within Y axis
				if(trueHit.point.y < trueHit.collider.bounds.center.y){
					//bottom
					newBlockPosition = trueHit.transform.position - Vector3(0.0,globalBlockScale,0.0);
				} else {
					//top
					newBlockPosition = trueHit.transform.position + Vector3(0.0,globalBlockScale,0.0);
				}
			} else {
				if(trueHit.point.x < trueHit.collider.bounds.center.x){
					//left
					newBlockPosition = trueHit.transform.position - Vector3(globalBlockScale,0.0,0.0);
				} else {
					//right
					newBlockPosition = trueHit.transform.position + Vector3(globalBlockScale,0.0,0.0);
				}
			}
			Debug.DrawLine(newBlockPosition,playerEye.position,Color.magenta,3.0);
			
			//have our world controller create the block
			var worldcont : GameObject = GameObject.FindGameObjectWithTag("mc").GetComponent(WorldController);
			worldcont.placeBlock(activeBlock.id, newBlockPosition);
			
			playerInv.deleteItem(activeBlock.id,qtyNeeded);
		}
	}
	*/
}