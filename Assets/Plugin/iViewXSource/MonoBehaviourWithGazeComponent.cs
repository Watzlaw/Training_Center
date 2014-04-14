using UnityEngine;
using System.Collections;

public abstract class MonoBehaviourWithGazeComponent : MonoBehaviour {

    private bool isSelected = false;

    public void OnObjectHit(RaycastHit hit)
    {
        if(isSelected)
        {
            OnGazeStay(hit);
        }
        else
        {
            OnGazeEnter(hit);
            isSelected = true;
        }


    }
    
    public void OnObjectExit()
    {
        isSelected = false;
        OnGazeExit();
    }

    public abstract void OnGazeEnter(RaycastHit hit);
    public abstract void OnGazeStay(RaycastHit hit);
    public abstract void OnGazeExit();
}
