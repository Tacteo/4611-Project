using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
	public int health = 100;
	public bool canControl = true;
	public float speed = 6.0F;
	public float jumpSpeed = 0.0F;
	public float gravity = 20.0F;
	private Vector3 moveDirection = Vector3.zero;

	private bool attacking = false;
	private bool queueAttack = false;
	private float timer = 0; //how long it has been since the attack animation started

	private int attackCombo = 0;//how many times you've attacked without a pause
	private int attackComboMax = 3;//max combo you can get without a pause

	public GameObject camera;
	public GameObject enemy;

	private void controlPause() {
		if (Input.GetKeyDown (KeyCode.P) ){
			if (canControl) {
				Time.timeScale = 0;
				canControl = false;
			} else {
				canControl = true;
			}
		}
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.LoadLevel("ForfeitScreen");
			return;
		}

		controlPause ();
		if (!canControl) {
			return;
		} else {
			Time.timeScale = 1;
		}

		if(Input.GetButtonDown("Attack")){
			attacking = true;
			queueAttack = true;
		}

		CharacterController controller = GetComponent<CharacterController>();
		if(!attacking){
			if (controller.isGrounded) {
				moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
				moveDirection = camera.transform.TransformDirection(moveDirection);
				moveDirection.y = 0;
				moveDirection *= speed;

				if(Input.GetButtonDown("Jump")){
					if(Input.GetButton("Target")){
						moveDirection = moveDirection*2;
					}
					moveDirection.y = jumpSpeed;
				}
				if (moveDirection.x == 0.0f &&
				    moveDirection.z == 0.0f) {
					GetComponent<Animation>().CrossFade("Idle");
				} else if(Vector3.SqrMagnitude(moveDirection)<0.2f) {
					GetComponent<Animation>().CrossFade("WalkAnimation");
				} else{
					GetComponent<Animation>().CrossFade("RunAnimation");
				}
				//looks at location that is either the direction traveling or the target enemy
				if(!Input.GetButton("Target")){
					if(Vector3.SqrMagnitude(moveDirection)>0.1f){
						transform.LookAt(transform.position+new Vector3(moveDirection.z,0.0f,-moveDirection.x));
					}
				}else{
					Vector3 difference = enemy.transform.position - transform.position;
					transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation((transform.position+new Vector3(difference.z,0.0f,-difference.x))-transform.position), 0.25f);
				}
				

			}
		}else{
			moveDirection = new Vector3(0,0,0);
		}

		//the player pressed the attack button or is in the middle of an attack
		if(attacking){
			if(attackCombo==0){
				timer = 0;
				attackCombo = 1;
				queueAttack = false;
			}
			if(queueAttack){
				if(timer<0.1f){
					queueAttack = false;
				}
			}
			
			if(timer==0){
				GetComponent<Animation>().Play("SwingDownAnimation");
			}
			timer += Time.deltaTime;
			if(timer>0.8f){
				attacking = false;
				attackCombo = 0;
				timer = 0;
			}else if((timer>0.4f)&&(queueAttack)&&(attackCombo<attackComboMax)){
				timer = 0;
				GetComponent<Animation>().Rewind ();

				GetComponent<Animation>().Play("SwingDownAnimation");

				attackCombo++;
			}
			
		}

		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move(moveDirection * Time.deltaTime);
		

		if (health <= 0) {
			Application.LoadLevel("DeathScreen");
		}
	}
}
