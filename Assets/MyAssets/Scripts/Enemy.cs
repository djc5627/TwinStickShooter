using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {
    [Tooltip("Money to drop on death")]
    public GameObject cashDrop;
    protected SpawnEnemies linkedSpawner;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //Link enemy to the spawner that controls it
    public void setLinkedSpawner(SpawnEnemies spawner)
    {
        linkedSpawner = spawner;
    }
}
