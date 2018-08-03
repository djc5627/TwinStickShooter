using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPull : MonoBehaviour {
    [Tooltip("Speed it comes at the player")]
    public float pullSpeed = 5f;
    [Tooltip("Dist before grabbed to avoid collision with player")]
    public float grabDist = 1f;

    private Transform target;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (target != null)
        {
            /**
             * REMOVED BECAUSE KEEPS PUSHING PLAYER
            //Move towards target
            float step = pullSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);

            RaycastHit2D hit = Physics2D.Raycast(transform.position,  target.position - transform.position, grabDist);
            Debug.DrawRay(transform.position, target.position - transform.position, Color.red);
            if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                hit.collider.gameObject.GetComponent<Player>().incMoney(1);
                Destroy(gameObject);
            }**/
        }
	}

    private void OnTriggerEnter2D(Collider2D col)
    {
        // if player in trigger give cash and destroy
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            col.gameObject.GetComponent<Player>().incMoney(1);
            Destroy(gameObject);
        }
    }
}
