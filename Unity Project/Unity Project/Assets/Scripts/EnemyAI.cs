using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {
	public GameObject player;
	public float attackRange = 2.1f; //Max distance for attacking
	public float followRange = 2.0f; //Distance at which enemy will follow player
	public float moveSpeed = 1.5f;
	public int health = 100;

	private float timer = 0; //Time in attack range (seconds)
	private bool attacking = false; //Used for animation
	private int attackAnim = 0;
	private bool inRange = false;
	private float attackRangeSq;
	private float followRangeSq;
	private Vector3 enemyPosition;
	private Vector3 playerPosition;
	private int rand;


	/**
	 * AI
	 * 
	 * If the enemy is currently attacking, keep playing animation. Don't move while attacking
	 * 
	 * If distance between player and enemy is greater than followRange, move towards player
	 * 
	 * If within attacking range, increment timer
	 * 	-If timer is >= 2, start attacking
	 */
	
	// Update is called once per frame
	void Update () {
		enemyPosition = transform.position;
		playerPosition = player.transform.position;

		transform.LookAt(playerPosition);
		transform.Rotate(0, 90, 0);

		//If distance to player is large, move to player
		float distsq = (enemyPosition - playerPosition).sqrMagnitude;
		if(distsq > followRangeSq) {
			var step = moveSpeed * Time.deltaTime;
			Vector3 newPos =  Vector3.MoveTowards(enemyPosition, playerPosition, step);
			if((newPos-playerPosition).magnitude < followRange) {
				inRange = true;
			} else {
				inRange = false;
			}
			if (!inRange) {
				transform.position = newPos;
				GetComponent<Animation>().CrossFade("WalkAnimation");
			}
		}

		//Update if in range and internal timer
		if (inRange) {
			timer += Time.deltaTime;
			if (timer >= 1.0f) {
				attacking = true;
				// do random animation
				rand = Random.Range(0,4);
				switch (rand) 
				{
				case 0:
					GetComponent<Animation>().CrossFade("SwingDownAnimation");
					break;
				case 1:
					GetComponent<Animation>().CrossFade("SwingLeftAnimation");
					break;
				case 2:
					GetComponent<Animation>().CrossFade("SwingRightAnimation");
					break;
				case 3:
					GetComponent<Animation>().CrossFade("SwingUpAnimation");
					break;
				default:
					GetComponent<Animation>().CrossFade("ThrustAnimation");
					break;
				}
				player.GetComponent<Movement>().health -= 2;
			} else {
				attacking = false;
				GetComponent<Animation>().CrossFade("FightingStanceAnimation");
			}
		} else {
			timer = 0;
		}

		// deal damage if player sword is in enemy
		var collisions = Physics.OverlapSphere(enemyPosition, 1.0f);
		int i = collisions.Length;
		for (i = 0; i < collisions.Length; i++) {
			if(collisions[i].CompareTag("PlayerSword")) {
				health -= 5;
			}
		}

		// show victory screen if enemy dies
		if (health <= 0) {
			Application.LoadLevel("VictoryScreen");
		}
	}
}












