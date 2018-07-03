using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject player;
    private Vector3 offset;

	// Use this for initialization
	void Start () {
        offset = transform.position - player.transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        Vector3 viewPos = GetComponent<Camera>().WorldToViewportPoint(player.transform.position);
        // Follow the player when they reach the edges of the screen
        if (viewPos.x > 0.7f || viewPos.x < 0.3f ||
            viewPos.y > 0.7f || viewPos.y < 0.3f)
        {
            transform.position = player.transform.position + offset;
        }
        else
        {
            offset = transform.position - player.transform.position;
        }
        transform.position = player.transform.position + offset;
	}
}
