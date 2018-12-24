using UnityEngine;

public class GhostController : MonoBehaviour {

	public enum GhostState {
		Normal,
		Aggressive,
		Fleeing,
		Charmed
	}

	public Transform target;

	[Header("Movement")]
	public float speed = 1;
	public float swayDistance = 1;
	public float swaySpeed = 1;

	[Header ("Status")]
	public GhostState state;
	public float agressiveMultiplier = 2;
	public float fleeMultiplier = 5;

	private Animator anim;
	private SpriteRenderer spr;
	private Vector3 currentPos;
	private float angle = 0;
	private int oldSign;
	private Vector2 randomDir = Vector2.right;
	private float speedMultiplier = 1;

	void Start () {
		anim = GetComponent<Animator> ();
		spr = GetComponent<SpriteRenderer> ();
		target = GameObject.FindGameObjectWithTag ("Player").transform;

		currentPos = transform.position;
	}
	
	void Update () {
		float oldXPos = currentPos.x;

		// Update sway calculation
		angle += swaySpeed * Time.deltaTime;
		float amplitude = Mathf.Sin(angle);
		if(oldSign != (int)Mathf.Sign(amplitude)) {
			randomDir = Random.insideUnitCircle;
			oldSign = (int)Mathf.Sign (amplitude);
		}

		// Make the movement sway with offset smoothly
		Vector3 deltaPos = currentPos + new Vector3 (randomDir.x * amplitude * swayDistance, randomDir.y * amplitude * swayDistance, 0);
		transform.position = Vector3.Lerp (transform.position, deltaPos, Time.deltaTime);

		// Condition to transition to other state
		CheckState ();

		// State update
		switch(state) {
			case GhostState.Normal:
				speedMultiplier = 1;
				MoveToTarget ();    // Move towards target if exist
				break;
			case GhostState.Aggressive:
				speedMultiplier = agressiveMultiplier;
				MoveToTarget ();
				break;
			case GhostState.Fleeing:
				MoveAway ();
				break;
			case GhostState.Charmed:
				break;
		}

		// Update animation
		anim.SetBool ("isDancing", state == GhostState.Fleeing);
		spr.flipX = (Mathf.Sign(oldXPos - currentPos.x) > 0); // flip the sprite
	}

	void CheckState () {
		Radio radio = FindObjectOfType<Radio> ();
		if(radio) {
			if(radio.isSignalClear) {
				// Different radio station cause different state of the ghost
				if(radio.stationName == "Calm FM") {
					state = GhostState.Fleeing;
				} else if (radio.stationName == "Horror FM") {
					state = GhostState.Aggressive;
				} else {
					Debug.Log ("Station name unrecognized!");
				}
			} else {
				// Static noise cause the ghost to normally haunts the player
				state = GhostState.Normal;
			}
		}
	}

	void MoveToTarget () {
		if (target) {
			currentPos = Vector3.MoveTowards (currentPos, target.position, speed * speedMultiplier * Time.deltaTime);
		} else {
			Debug.Log ("No target found!");
		}
	}

	void MoveAway () {
		Vector2 fleeDirection;
		if (target) {
			fleeDirection = (currentPos - target.position).normalized;
			if(fleeDirection.magnitude < 0.1) { // if direction zero
				fleeDirection = Random.insideUnitCircle;
			}
			Disappear ();
		} else {
			fleeDirection = Random.insideUnitCircle;
		}

		MoveTowards (fleeDirection * fleeMultiplier);
	}

	void MoveTowards (Vector2 direction) {
		currentPos = currentPos + (Vector3)(direction * speed * Time.deltaTime);
	}

	public void Disappear () {
		spr.color = Color.Lerp (spr.color, new Color(1, 1, 1, 0), Time.deltaTime);
		if(spr.color.a <= 0.3) {
			Destroy (gameObject, 1);
		}
	}
	
	void OnDrawGizmos () {
		Gizmos.color = Color.green;
		Gizmos.DrawLine (currentPos - Vector3.right, currentPos + Vector3.right);
		Gizmos.DrawLine (currentPos - Vector3.up, currentPos + Vector3.up);
	}
}
