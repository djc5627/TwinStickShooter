using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurnAround : Enemy
{
    public float speed = 3f;

    private Rigidbody2D rb;
    private int direction;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        direction = Random.Range(0, 4);
    }
	
    //TODO: Find neater way than cases for this
	// Update is called once per frame
	void Update () {
		switch(direction)
        {
            //up
            case 0:
                rb.velocity = Vector2.up * speed;
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                break;
                
            //down
            case 1:
                rb.velocity = Vector2.down * speed;
                transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                break;

            //right
            case 2:
                rb.velocity = Vector2.right * speed;
                transform.rotation = Quaternion.Euler(0f, 0f, -90f);
                break;

            //left
            case 3:
                rb.velocity = Vector2.left * speed;
                transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                break;

            default:
                Debug.Log("Invalid direction");
                break;
        }
	}

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            switch(direction)
            {
                //up
                case 0:
                    direction = 1;
                    break;

                //down
                case 1:
                    direction = 0;
                    break;

                //right
                case 2:
                    direction = 3;
                    break;

                //left
                case 3:
                    direction = 2;
                    break;

                default:
                    Debug.Log("Invalid direction");
                    break;
            }
        }
        if (col.gameObject.layer == LayerMask.NameToLayer("PlayerBullet"))
        {
            linkedSpawner.incEnemyCount(-1);
            Destroy(gameObject);
            Destroy(col.gameObject);
        }
    }
}
