using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Digger : MonoBehaviour {


    public float moveTime = 0.1f; // Time it takes the digger to move to a new position (seconds)

    private float inverseMoveTime;

	// Use this for initialization
	void Start () {
        inverseMoveTime = 1f / moveTime;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
