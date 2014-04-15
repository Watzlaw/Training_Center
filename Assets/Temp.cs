using UnityEngine;
using System.Collections;

public class Temp : MonoBehaviour {

	public float speed = 0;
	public Transform target =null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * speed);
	}
}
