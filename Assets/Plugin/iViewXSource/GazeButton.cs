using UnityEngine;
using System.Collections;

[ExecuteInEditMode]


public delegate void buttonCallbackListener();

public class GazeButton : GUIElement{

    public GUIStyle myStyle;
    public bool isVisible = true;

    private string content = "";
    private Rect position;
    private Rect colliderPosition; 
    private GUIStyle actualStyleOfTheElement = new GUIStyle();

    private buttonCallbackListener actionToDo;
    

	public GazeButton(Rect position,string content,GUIStyle myStyle,buttonCallbackListener callback)
    {
        this.position = position;
        this.content = content;

        colliderPosition = new Rect(position.x, Camera.main.pixelHeight-position.y-position.height,position.width,position.height);

        if(myStyle!= null)
        {
            this.myStyle = myStyle;
            actualStyleOfTheElement = this.myStyle; 
        }
        actionToDo = callback;
    }

    public GazeButton(Rect position, string content)
    {
        this.position = position;
        this.content = content;

    }

    public void OnGUI()
    {
        if(isVisible)
        {
                GUI.Label(position, content,actualStyleOfTheElement);
        }

    }


    public bool Update()
    {
        Vector2 positionGaze;

        // Emulate Gazedata via Mouseinput, if the Connection is Lost
        if (!gazeModel.isRunning)
        {
            positionGaze = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
        }

        // Load the GazeData From the gazemodel
        else
        {
            positionGaze = (gazeModel.posGazeLeft + gazeModel.posGazeRight) * 0.5f;
        }

        //Check Colision
        if (colliderPosition.Contains(Input.mousePosition))
        {
            setFocused();

            if (Input.GetButtonUp("SelectGUI"))
                actionToDo();
        }

        else
        {
            setActive();
        }
        return false; 
    }

    private void setActive()
    {
        actualStyleOfTheElement.normal = myStyle.active;
    }

    private void setFocused()
    {
        actualStyleOfTheElement.normal = myStyle.focused;
    }
}
