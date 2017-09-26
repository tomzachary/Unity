using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    private Transform followee;
    private Vector3 position;
	// Use this for initialization
	void Start () {
        position = new Vector3(0, 0, -3);
      
	}
	
	// Update is called once per frame
	void Update () {
        followee = GameObject.Find("Wizard").transform;
        transform.position = followee.position + position;
	}
}
