﻿using UnityEngine;
using System.Collections;
/*
public static class RendererExtensions
{
  public static bool IsVisibleFrom(this Renderer renderer, Camera camera)
  {
    Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
    return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
  }
}*/

public class EnemyScript : MonoBehaviour {
	private int direction = 0;
	public float speed;
	public Vector2 headPopForce;
	public int Health = 2;
	public Sprite DeadHeadSprite;
	public ParticleSystem Decapitation;
	public AudioSource AxeHitSound;
	public AudioSource AxeKillSound;
	public AudioSource DeathSound;
	public AudioSource DeathMoanSound;

	private float leftWall = 0.0f;
	private float rightWall = 0.0f;
	private BoxCollider2D collider;
	private Rigidbody2D head;
	private int currentDir = 1;
	private bool hasPlatform = false;
	private bool firstVisible = true;
	// Use this for initialization
	void Start () {
		collider = GetComponent<BoxCollider2D>();
		head = GetComponent<DistanceJoint2D>().connectedBody;
		Face(1);
	}
	
	// Update is called once per frame
	void Update () {
		if(firstVisible && GetComponent<Renderer>().IsVisibleFrom(Camera.main)) {
			DeathSound.Play();
			firstVisible = false;
		}
		transform.Translate(speed, 0, 0);
		if(collider.bounds.center.x - collider.bounds.extents.x <= leftWall) {
			speed *= -1;
		}
		else if(collider.bounds.center.x + collider.bounds.extents.x >= rightWall) {
			speed *= -1;
		}
		if(speed > 0 && currentDir != 1) {
			Face(1);
		}
		else if (speed < 0 && currentDir != -1) {
			Face(-1);
		}
	}

	private void Face(int dir) {
		float scaleX = transform.localScale.x;
		float headScaleX = head.gameObject.transform.localScale.x;
		if(dir == -1 && scaleX > 0) {
			scaleX *= -1;
			headScaleX *= -1;
		} else if(dir == 1 && scaleX < 0) {
			scaleX *= -1;
			headScaleX *= -1;
		}
		transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
		head.gameObject.transform.localScale = new Vector3(headScaleX, head.gameObject.transform.localScale.y, head.gameObject.transform.localScale.z);
		currentDir = dir;
	}

	void OnCollisionStay2D(Collision2D collision){
		if(!hasPlatform && collision.gameObject.name.Contains("Ground")) {
			hasPlatform = true;
			leftWall = collision.collider.bounds.center.x - collision.collider.bounds.extents.x;
			rightWall = collision.collider.bounds.center.x + collision.collider.bounds.extents.x;
		}
	}

	void OnCollisionEnter2D(Collision2D collision){
		if(collision.gameObject.name.Contains("Tomahawk")) {
			if(collision.gameObject.GetComponent<TomahawkScript>().InHand) {
				Application.LoadLevel("GameOver");
			}
			else {
				Destroy(collision.gameObject);
				AxeHitSound.Play();
				Health -= 1;
				if(Health == 0) {
					AxeKillSound.Play();
					DeathMoanSound.Play();
					CircleCollider2D headCollider = head.gameObject.GetComponent<CircleCollider2D>();
					Vector3 decapPos;
					decapPos = new Vector3(headCollider.bounds.center.x, headCollider.bounds.extents.y + headCollider.bounds.center.y, 0);
					ParticleSystem decap = (ParticleSystem) Instantiate(Decapitation, decapPos, new Quaternion(0,0,0,0));
					Destroy(decap, 3);
					decap.Play();
					GetComponent<DistanceJoint2D>().enabled = false;
					head.freezeRotation = false;
					head.AddForce(headPopForce, ForceMode2D.Impulse);
					head.gameObject.GetComponent<SpriteRenderer>().sprite = DeadHeadSprite;
					Destroy(gameObject);
				}
					
			}
		}
	}
}
