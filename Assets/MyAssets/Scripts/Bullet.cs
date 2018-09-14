using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Basic type of Bullet
public class Bullet : MonoBehaviour {
    [Tooltip("Will the ammo bounce off walls")]
    public bool isBouncy = false;
    [Tooltip("How many times bullet can bounce (0 for inf)")]
    public int bounceCount = 0;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D col)
    {
        //If collide with wall
        if (col.gameObject.layer == LayerMask.NameToLayer("Walls") || col.gameObject.layer == LayerMask.NameToLayer("Door"))
        {
            //Destroy if not bouncy or out of bounces
            if (!isBouncy || bounceCount <= 1)
                Destroy(this.gameObject);
            else
                bounceCount -= 1;
        }
        
        
    }
}
