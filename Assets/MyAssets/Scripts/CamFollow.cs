using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Get working with mult players
public class CamFollow : MonoBehaviour {
    [Tooltip("Size of camera when zoom out button held")]
    public float zoomSize = 20f;

    private Transform player;
    //Ortho size of camera before zoom so can return to it
    private float startSize;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
    }

    void Start () {
        startSize = GetComponent<Camera>().orthographicSize;
    }
	
	// Update is called once per frame
	void Update () {

    }

    void LateUpdate()
    {
        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
        CameraZoomOut();
    }

    private void CameraZoomOut()
    {
        if (Input.GetButton("Map"))
        {
            GetComponent<Camera>().orthographicSize = 20f;
        }
        else
        {
            GetComponent<Camera>().orthographicSize = startSize;
        }
    }
}
