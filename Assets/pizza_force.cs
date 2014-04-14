using UnityEngine;
using System.Collections;

public class pizza_force : MonoBehaviour {

    //Rigidbody bullet = new Rigidbody();
    public int speed = 20;
    public GameObject bullet = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetButtonDown("Fire1"))
        {
            shoot();
        }
	
	}

    void shoot()
    {
        GameObject shot = GameObject.Instantiate(bullet, transform.position, transform.rotation) as GameObject;
        shot.rigidbody.AddForce(transform.forward * 500);
    }

    //void OnMouseDown()
    //{
    //    rigidbody.AddForce(-transform.forward * 500);
    //}
}
