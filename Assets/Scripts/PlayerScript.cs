using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
	public float speed;
	public float jumpForce;
	public Vector2 throwForce;
	public float torqueForce;
	public Transform timer;
	public int headsToSavage;
	public float savageSpeed;
	public float savageJumpForce;
	public Vector2 savageThrowForce;
	public float savageTorqueForce;
	public AudioSource AxeThrowSound;
	public AudioSource JumpSound;
	public int ReloadSpeed;
	public Transform EndOfStage;
	public string NextScene;

	private float distToGround;
	private BoxCollider2D collider;
	private bool isGrounded = false;
	private int reload = 0;
	private DistanceJoint2D joint;
	private Rigidbody2D tomahawk;
	private Vector2 tomahawkPos;
	private Quaternion tomahawkRot;
	private Rigidbody2D body;
	private Vector2 playerPos;
	private int headCount = 0;
	private Animator animator;
	private bool isSavage = false;

	enum State {
		Idle,
		StepRight,
		StepLeft,
		AttackRight,
		AttackLeft,
		StepAttackLeft,
		StepAttackRight,
		SavageIdle,
		SavageMove,
		SavageAttack,
		SavageStepAttack
	}

	private State currentState = State.Idle;
	const string aIdle = "PlayerIdle";
	const string aStep = "PlayerWalking";
	const string aAttack = "PlayerAttack";
	const string aStepAttack = "PlayerStepAttack";
	const string aSavageIdle = "SavageIdle";
	const string aSavageWalk = "SavageWalking";
	const string aSavageAttack = "SavageAttacking";
	const string aSavageStepAttack = "SavageStepAttack";

	// Use this for initialization
	void Start () {
		collider  = GetComponent<BoxCollider2D>();
		body = GetComponent<Rigidbody2D>();
		joint = GetComponent<DistanceJoint2D>();
		animator = GetComponent<Animator>();
		tomahawk = joint.connectedBody;
		headCount = this.GetHeadCount();
	}
	
	// Update is called once per frame
	void Update () {
		if(reload > 0) {
			reload--;
			if(reload == 0) {
				Vector2 newPos = (Vector2)transform.position - playerPos;
				Rigidbody2D tomahawkClone = (Rigidbody2D) Instantiate(tomahawk, tomahawkPos + newPos, tomahawkRot);
				joint.connectedBody = tomahawkClone;
				joint.enabled = true;
				tomahawk = tomahawkClone;
			}
		}
		if (headCount >= headsToSavage) {
			isSavage = true;
		}
		if(transform.position.x > EndOfStage.position.x) {
			this.SetHeadCount(headCount);
			Application.LoadLevel(NextScene);

		}
	}

	void FixedUpdate() {
		float movement;
		if(!isSavage) {
			movement = Input.GetAxis("Horizontal") * speed;
		} else {
			movement = Input.GetAxis("Horizontal") * savageSpeed;
		}
		transform.Translate(movement, 0, 0);

		if(currentState != State.AttackRight && currentState != State.StepAttackRight) {
			if(movement > 0) {
				ChangeState(State.StepRight);
			}
			else if(movement < 0) {
				ChangeState(State.StepLeft);
			}
			else {
				ChangeState(State.Idle);
			}
		}

		

		if(Input.GetKeyDown("space") && isGrounded) {
			JumpSound.Play();
			body.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
			if(body.velocity.y > jumpForce) {
				body.velocity = new Vector2(body.velocity.x, jumpForce);
			}
		}

		if(Input.GetKeyDown(KeyCode.S) && reload == 0) {
			ThrowTomahawk();
		}

	}

	void OnCollisionStay2D(Collision2D collision){
		if(!collision.gameObject.name.Contains("Tomahawk")) {
			isGrounded = true;
		}
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if(collision.gameObject.name.Contains("Head")) {
			Destroy(collision.gameObject);
			headCount++;
			timer.gameObject.SendMessage("IncreaseTime", 3.0f);
		}
	}

	void OnCollisionExit2D(Collision2D collision){
		if(!collision.gameObject.name.Contains("Tomahawk")) {
			isGrounded = false;
		}
	}

	private void ThrowTomahawk() {
		if(currentState == State.Idle) {
			ChangeState(State.AttackRight);
		}
		else if(currentState == State.StepRight) {
			ChangeState(State.StepAttackRight);
		}
		joint.enabled = false;
		tomahawkPos = tomahawk.transform.position;
		tomahawkRot = tomahawk.transform.rotation;
		playerPos = transform.position;
		float xMultiplier = (Random.value * 0.3f) + 0.9f;
		float yMultiplier = (Random.value * 2.0f) + 0.5f;
		if(isSavage) {
			Vector2 randThrowForce = new Vector2(savageThrowForce.x * xMultiplier, throwForce.y * yMultiplier);
			joint.connectedBody.gameObject.SendMessage("Throw");
			tomahawk.AddForce(randThrowForce + body.velocity, ForceMode2D.Impulse);
			tomahawk.AddTorque(savageTorqueForce, ForceMode2D.Force);
		}
		else {
			Vector2 randThrowForce = new Vector2(throwForce.x * xMultiplier, throwForce.y * yMultiplier);
			joint.connectedBody.gameObject.SendMessage("Throw");
			tomahawk.AddForce(randThrowForce + body.velocity, ForceMode2D.Impulse);
			tomahawk.AddTorque(torqueForce, ForceMode2D.Force);
		}
		AxeThrowSound.Play();
		reload = ReloadSpeed;

	}

	private void ChangeState(State state) {
		if(state != currentState) {
			currentState = state;
			if(isSavage) {
				switch(state) {
					case State.Idle:
						state = State.SavageIdle;
						break;
					case State.StepRight:
						state = State.SavageMove;
						break;
					case State.StepLeft:
						state = State.SavageMove;
						break;
					case State.AttackRight:
						state = State.SavageAttack;
						break;
					case State.StepAttackRight:
						state = State.SavageStepAttack;
						break;
				}
			}
			EnterState(state);
		}
	}

	private void EnterState(State state) {
		switch (state) {
			case State.Idle:
				animator.Play(aIdle);
				break;
			case State.StepLeft:
				animator.Play(aStep);
				//Face(-1);
				break;
			case State.StepRight:
				animator.Play(aStep);
				//Face(1);
				break;
			case State.AttackLeft:
				animator.Play(aAttack);
				//Face(-1);
				break;
			case State.AttackRight:
				animator.Play(aAttack);
				//Face(1);
				break;
			case State.StepAttackLeft:
				animator.Play(aStepAttack);
				//Face(-1);
				break;
			case State.StepAttackRight:
				animator.Play(aStepAttack);
				//Face(1);
				break;
			case State.SavageIdle:
				animator.Play(aSavageIdle);
				break;
			case State.SavageMove:
				animator.Play(aSavageWalk);
				break;
			case State.SavageAttack:
				animator.Play(aSavageAttack);
				break;
			case State.SavageStepAttack:
				animator.Play(aSavageStepAttack);
				break;
		}
	}

	private void Face(int dir) {
		float scaleX = transform.localScale.x;
		if(dir == -1 && scaleX > 0) {
			scaleX *= -1;
		} else if(dir == 1 && scaleX < 0) {
			scaleX *= -1;
		}
		transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
	}
}
