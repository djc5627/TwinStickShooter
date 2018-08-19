using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : Enemy
{
    private Rigidbody2D rb;
    private GameObject player;
    private Vector3 playerPos;
    private Vector3 playerDir;
    private float rotZ;

    public float forceMult = 5f;
    public float maxSpeed = 2f;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        TrackPlayer();
    }

    private void FixedUpdate()
    {
        Move();
    }

    //TODO: Get working for multiple players (maybe pick closest player)
    private void TrackPlayer()
    {
        playerPos = player.transform.position;
        playerDir = (playerPos - transform.position).normalized;
        rotZ = Mathf.Atan2(playerDir.y, playerDir.x) * Mathf.Rad2Deg;
    }

    //Move the enemy in direction of player by setting adding force
    private void Move()
    {
        rb.rotation = rotZ - 90;
        rb.AddForce(playerDir * forceMult);

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        // if collide with player bullet, destroy this
        if (col.gameObject.layer == LayerMask.NameToLayer("PlayerBullet"))
        {
            linkedSpawner.incEnemyCount(-1);
            Destroy(gameObject);
            Destroy(col.gameObject);
        }
    }
}
