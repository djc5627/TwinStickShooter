using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [Tooltip("The Destination Transform")]
    public Transform destination;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // if collide with player, teleport
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Transform playerTrans = col.gameObject.transform;
            Teleport(playerTrans);
        }
    }

    //Teleport by moving Transform position
    private void Teleport(Transform trans)
    {
        trans.position = destination.position;
    }
}
