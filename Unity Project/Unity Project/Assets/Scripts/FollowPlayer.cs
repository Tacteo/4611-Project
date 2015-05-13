using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {
	public GameObject player;
	public float rotateSpeed = 1;
	public float targettingOffset = 20.0f;

	private Movement charMove;
	private Vector3 offset;
	private bool useController;
	private float desiredAngle = 0;

	// Use this for initialization
	void Start() {
		//offset = new Vector3(0f, 1.5f, 3f);
		offset = player.transform.position - transform.position;
		charMove = (Movement)player.GetComponent(typeof(Movement));
		//useController = charMove.usingJoystick();
	}

	void LateUpdate() {
		if (!player.GetComponent<Movement> ().canControl) {
			return;
		}
		float horizontal;
		//if(useController) {
		//	horizontal = Input.GetAxis("4th Axis") * rotateSpeed;
		//}
		//else {
			//rotate player
		horizontal = Input.GetAxis("Mouse X") * rotateSpeed;

		
		//this rotates the camera so it's behind you if you are targetting
		//might want to get rid of this because the camera jerks too much
		//try to smooth out the transition to this spot
		if(Input.GetButton("Target")){
			desiredAngle = player.transform.eulerAngles.y+targettingOffset;
		}else{
		//GameObject temp = new GameObject();

		//temp.transform.position = player.transform.position;
		//temp.transform.rotation = transform.rotation;

		//player.transform.Rotate(0, horizontal, 0);
		//temp.transform.Rotate(0, horizontal, 0);
		//orient camera
		//float desiredAngle = temp.transform.eulerAngles.y;
		desiredAngle += horizontal;

		}
		Quaternion rotation = Quaternion.Euler(0, desiredAngle, -30);
		transform.position = player.transform.position - (rotation * offset);
		transform.LookAt(player.transform);
		//charMove.setDir(horizontal);
	}

	// Update is called once per frame
	void Update() {
	
	}
}
