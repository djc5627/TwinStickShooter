using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {
    protected SpawnEnemies linkedSpawner;
    public int health;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //If dead, destory game object and drop cash
    virtual public void  Death()
    {
        if (health <= 0f)
        {
            Destroy(gameObject);
        }
    }

    //increment health by inc and check if dead
    virtual public void incHealth(int inc)
    {
        health += inc;
        Death();
    }

    //Link enemy to the spawner that controls it
    public void setLinkedSpawner(SpawnEnemies spawner)
    {
        linkedSpawner = spawner;
    }
}
