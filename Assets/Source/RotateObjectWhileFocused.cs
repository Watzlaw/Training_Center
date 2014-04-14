using UnityEngine;
using System.Collections;

public class RotateObjectWhileFocused : MonoBehaviourWithGazeComponent{

    public float rotationsPerMinute = 100.0f;

	void Start () {
	
	}
	
	void Update () {
	
	}

    public override void OnGazeEnter(RaycastHit hit)
    {
    
    }

    public override void OnGazeStay(RaycastHit hit)
    {
        transform.Rotate(0, 0, rotationsPerMinute * Time.deltaTime);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Destroy(gameObject);
        }
    }

    public override void OnGazeExit()
    {
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }
}
