using UnityEngine;
using System.Collections;

public class pizza_force : MonoBehaviour {

    //Rigidbody bullet = new Rigidbody();
    public float speed = 500f;
    public GameObject bullet = null;
	public GameObject shot;
	public GameObject vater = null;

	// Use this for initialization
	void Start () {

		moveOnSpawn();
	
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
		//Debug.Log("hit");
        //Rigidbody shot = Instantiate(bullet, transform.position, transform.rotation) as Rigidbody;
		//shot.AddForce(transform.forward * 500);
		//shot.velocity = transform.TransformDirection(Vector3(0,0,500));


		shot.transform.parent = null;
		shot.rigidbody.useGravity = true;
		shot.rigidbody.AddForce(transform.forward*500);
    }

	void moveOnSpawn()
	{
		shot = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
		shot.rigidbody.Sleep();
		shot.transform.parent = vater.transform;
		shot.transform.Rotate(-90,0,0);
		shot.transform.Translate (0,3,0);
		//shot.transform.Translate (Vector3.forward * Time.deltaTime);
	}

    //void OnMouseDown()
    //{
    //    rigidbody.AddForce(-transform.forward * 500);
    //}
}
