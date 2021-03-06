﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    //---------P1----------------

    //Left Stick
    public Vector2 rawL_P1 { get; private set; }
    public Vector2 normL_P1 { get; private set; }
    public float rotZL_P1 { get; private set; }
    public bool movedL_P1 { get; private set; }
    public float deadzoneL_P1 = .3f;

    //Right Stick
    public Vector2 rawR_P1 { get; private set; }
    public Vector2 normR_P1 { get; private set; }
    public float rotZR_P1 { get; private set; }
    public bool movedR_P1 { get; private set; }
    public float deadzoneR_P1 = .3f;

    //Buttons
    public bool mapIsPressed { get; private set; }
    public bool thrustIsPressed { get; private set; }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //-----------------P1---------------------

        //Left Stick
        rawL_P1 = new Vector2(Input.GetAxis("LHorizontal_P1"), Input.GetAxis("LVertical_P1"));

        if (rawL_P1.magnitude < deadzoneL_P1)
        {
            rawL_P1 = Vector2.zero;
        }

        normL_P1 = rawL_P1.normalized;
        rotZL_P1 = Mathf.Atan2(normL_P1.y, normL_P1.x) * Mathf.Rad2Deg;

        if (normL_P1.x != 0 || normL_P1.y != 0)
            movedL_P1 = true;
        else
            movedL_P1 = false;

        //Right Stick
        rawR_P1 = new Vector2(Input.GetAxis("RHorizontal_P1"), Input.GetAxis("RVertical_P1"));
        
        if (rawR_P1.magnitude < deadzoneR_P1)
        {
            rawR_P1 = Vector2.zero;
        }

        normR_P1 = rawR_P1.normalized;
        rotZR_P1 = Mathf.Atan2(normR_P1.y, normR_P1.x) * Mathf.Rad2Deg;

        if (normR_P1.x != 0 || normR_P1.y != 0)
            movedR_P1 = true;
        else
            movedR_P1 = false;

        //Buttons
        if (Input.GetButton("Map"))
            mapIsPressed = true;
        else
            mapIsPressed = false;

        if (Input.GetButtonDown("Thrust"))
            thrustIsPressed = true;
        else
            thrustIsPressed = false;
    }
}
