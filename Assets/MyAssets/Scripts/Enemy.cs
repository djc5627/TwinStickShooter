using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {
    protected SpawnEnemies linkedSpawner;
    public int health;

    private SpriteRenderer spriteRend;

	// Use this for initialization
	public void Start () {
        spriteRend = GetComponent<SpriteRenderer>();
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
        //If taking damage, and have sprite, flash red
        if (inc <= 0 && spriteRend != null)
            StartCoroutine(damageFlash());
            
        health += inc;
        Death();
    }

    //Flash the sprite red
    IEnumerator damageFlash()
    {
        spriteRend.color = Color.red;
        yield return new WaitForSeconds(.015f);
        spriteRend.color = Color.white;

        yield return null;
    }

    //Link enemy to the spawner that controls it
    public void setLinkedSpawner(SpawnEnemies spawner)
    {
        linkedSpawner = spawner;
    }
}
