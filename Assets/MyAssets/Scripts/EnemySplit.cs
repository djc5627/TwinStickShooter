using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Refactor this class against enemy follow
public class EnemySplit : Enemy
{
    private Rigidbody2D rb;
    private GameObject player;
    private Vector3 playerPos;
    private Vector3 playerDir;

    public GameObject splitObj;
    public float forceMult = 5f;
    public float maxSpeed = 2f;
    public float splitScale = .5f;

    // Use this for initialization
    void Start () {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
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
    }

    //Move the enemy in direction of player by setting adding force
    private void Move()
    {
        rb.AddForce(playerDir * forceMult);

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    public void SetParameters(float scale, int hp)
    {
        health = hp;
        transform.localScale = transform.localScale * scale;
    }

    //TODO: give distance between them when they split
    private void OnCollisionEnter2D(Collision2D col)
    {
        // if collide with player bullet, split or destroy
        if (col.gameObject.layer == LayerMask.NameToLayer("PlayerBullet"))
        {
            //if health at 1, die
            if (health == 1)
            {
                linkedSpawner.incEnemyCount(-1);
                Destroy(col.gameObject);
                Destroy(gameObject);
            }
            //if health >1, die, split, and reduce health
            else if (health > 1)
            {
                Destroy(col.gameObject);
                //Add enemy to count because being split into more
                linkedSpawner.incEnemyCount(1);

                //Give the children's trans an offset so dont spawn inside each other and glitch
                Vector2 offset1 = new Vector2(transform.position.x + .2f, transform.position.y);
                Vector2 offset2 = new Vector2(transform.position.x - .2f, transform.position.y);

                GameObject childObj1 = Instantiate(splitObj, offset1, transform.rotation);
                GameObject childObj2 = Instantiate(splitObj, offset2, transform.rotation);

                EnemySplit split1 = childObj1.GetComponent<EnemySplit>();
                EnemySplit split2 = childObj2.GetComponent<EnemySplit>();
                Collider2D col1 = childObj1.GetComponent<Collider2D>();
                Collider2D col2 = childObj2.GetComponent<Collider2D>();

                //Link Children to same spawner
                split1.linkedSpawner = this.linkedSpawner;
                split2.linkedSpawner = this.linkedSpawner;

                //TODO: Make sure this is valid workaround or find better one
                //Temp workaround for children having disabled components and freezing
                split1.enabled = true;
                split2.enabled = true;
                col1.enabled = true;
                col2.enabled = true;

                split1.SetParameters(splitScale, health - 1);
                split2.SetParameters(splitScale, health - 1);

                Destroy(gameObject);
            }
            
        }
    }
}

