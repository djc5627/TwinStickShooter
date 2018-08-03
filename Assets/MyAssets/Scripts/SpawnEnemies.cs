using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour {
    [Tooltip("Width of spawn range from origin to either side")]
    public float width = 18f;
    [Tooltip("Height of spawn range from origin to either side")]
    public float height = 10f;
    [Tooltip("Number of seconds between enemy spawns")]
    public float spawnInterval = 1f;
    private float nextSpawn = 0f;

    public GameObject[] enemies;
    public GameObject[] spawnEffects;
    //For Activating/Deact portals on room clear
    public GameObject portalContainer;

    [Tooltip("Number of enemies to spawn in this room")]
    public int maxEnemyCount = 50;

    //Num enemies spawned so far, counts mult. enemies as 1 unit
    private int spawnedEnemyCount = 0;
    //Current enemies in room, counts mult. enemies as multiple units
    private int currEnemyCount = 0;

    private bool playerInRoom = false;


    //TODO: use raycast to make sure not spawning too close to player
    //TODO: implement count and communicate with enemies to decrease enemy count
    //TODO: change spawn effect color based on enemy spawned
    //TODO: seperate rate/count for each enemy
    //TODO: rate/count over time

    // Use this for initialization
    void Start () {
        portalContainer.active = false;
    }

	// Update is called once per frame
	void Update () {
        DebugBox();
        Spawner();
        RoomCleared();

    }

    //Manage where/when enemies spawn at interval
    private void Spawner()
    {
        //Spawn at interval if player in room
        if (Time.time >= nextSpawn && playerInRoom && spawnedEnemyCount < maxEnemyCount)
        {
            int randIndex = Random.Range(0, enemies.Length);
            Vector2 spawnPos = new Vector2(Random.Range(transform.position.x - width, transform.position.x + width),
                Random.Range(transform.position.y - height, transform.position.y + height));
            StartCoroutine(SpawnRandomEnemy(spawnPos, randIndex));
            nextSpawn = Time.time + spawnInterval;
        }
    }

    //Spawn a random enemy/effect and increase enemy count
    IEnumerator SpawnRandomEnemy(Vector3 pos, int index)
    {
        spawnedEnemyCount++;
        incEnemyCount(1);
        Instantiate(spawnEffects[index], pos, Quaternion.identity);
        yield return new WaitForSeconds(1.2f);
        
        //Link the new enemy to this spawner
        GameObject enemyObj = Instantiate(enemies[index], pos, Quaternion.identity);
        enemyObj.GetComponent<Enemy>().setLinkedSpawner(this);

        //TODO: Do this cleaner
        //If the basic, spawn more of them (temp method)
        if (index == 0)
        {
            incEnemyCount(2);

            Vector2 offset1 = new Vector2(pos.x + .2f, pos.y - .2f);
            Vector2 offset2 = new Vector2(pos.x - .2f, pos.y - .2f);

            GameObject enemyObj1 = Instantiate(enemies[index], offset1, Quaternion.identity);
            GameObject enemyObj2 = Instantiate(enemies[index], offset2, Quaternion.identity);

            //Link them all to the same spawner
            enemyObj1.GetComponent<Enemy>().setLinkedSpawner(this);
            enemyObj2.GetComponent<Enemy>().setLinkedSpawner(this);
        }
    }

    //Draw rect from spawning bounds
    private void DebugBox ()
    {
        
        Vector3 topLeft = new Vector3(transform.position.x - width, transform.position.y + height, 0);
        Vector3 topRight = new Vector3(transform.position.x + width, transform.position.y + height, 0);
        Vector3 bottomLeft = new Vector3(transform.position.x - width, transform.position.y - height, 0);
        Vector3 bottomRight = new Vector3(transform.position.x + width, transform.position.y - height, 0);

        Debug.DrawLine(topLeft, topRight);
        Debug.DrawLine(topRight, bottomRight);
        Debug.DrawLine(bottomRight, bottomLeft);
        Debug.DrawLine(bottomLeft, topLeft);
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        // if player in trigger
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerInRoom = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        // if player left trigger
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerInRoom = false;
        }
    }

    //Increment the current enemy count by num
    //For use by enemy when killed
    public void incEnemyCount(int num)
    {
        currEnemyCount += num;
    }

    //If all enemies have been spawned and current enemy count is 0
    //Activate the portals
    private void RoomCleared()
    {
        if (spawnedEnemyCount == maxEnemyCount && currEnemyCount == 0)
        {
            portalContainer.active = true;
        }
    }
}
