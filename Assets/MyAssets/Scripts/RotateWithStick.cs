using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithStick : MonoBehaviour {

    public int playerNumber = 1;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 rawInput = new Vector2(Input.GetAxisRaw("RHorizontal_P" + playerNumber), Input.GetAxisRaw("RVertical_P" + playerNumber));
        Vector2 normInput = rawInput.normalized;

        float rotZ = Mathf.Atan2(normInput.y, normInput.x) * Mathf.Rad2Deg;

        //If left stick not at default pos
        if (normInput.x != 0 || normInput.y != 0)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ - 90);
        }
    }
}
