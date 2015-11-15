using UnityEngine;
using System.Collections;

public class Modify : MonoBehaviour
{

    Vector3 scroll;


	public int mode;
	void Awake()
	{
		mode = 0;
	}

    void Update()
    {
		scroll = Input.mouseScrollDelta;

		if(scroll.y > 0) mode = Mathf.Clamp(mode+1, 1, 2);
		else if(scroll.y < 0) mode = Mathf.Clamp(mode-1, 0, 1);

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            RaycastHit hit;
			Vector3 raystart = transform.position;
			raystart.z += (mode-1);
            if (Physics.Raycast(raystart , transform.forward, out hit, 100))
            {
                Terra.SetBlock(hit, new BlockAir(), false);
            }
        }

//        rot = new Vector2(
//            rot.x + Input.GetAxis("Mouse X") * 3,
//            rot.y + Input.GetAxis("Mouse Y") * 3);
//
//        transform.localRotation = Quaternion.AngleAxis(rot.x, Vector3.up);
//        transform.localRotation *= Quaternion.AngleAxis(rot.y, Vector3.left);
//
//        transform.position += transform.forward * 3 * Input.GetAxis("Vertical");
//        transform.position += transform.right * 3 * Input.GetAxis("Horizontal");
    }
}