using UnityEngine;
using System.Collections;


public class ExampleGazeMenu : MonoBehaviour {

    private ArrayList gazeUI = new ArrayList();
    private bool isDrawing = false; 
    
    public GUIStyle myStyle;
    
    void Start()
    {

        //Set the Actions of the Buttons
        buttonCallbackListener writeIntroConsoleButton = button1_Action;
        buttonCallbackListener quitApplicationButton = button2_Action;

        //Create new Buttonelements and add them to the gazeUI
        gazeUI.Add(new GazeButton(new Rect(Screen.width * 0.35f, Screen.height*0.15f, 512, 256), "Say Hello!", myStyle, writeIntroConsoleButton));
        gazeUI.Add(new GazeButton(new Rect(Screen.width * 0.35f, Screen.height*0.5f, 512, 256), "Quit", myStyle, quitApplicationButton));

    }
    
    void OnGUI()
    {
        //Draw every Button from the ArrayList gazeUI
        if (isDrawing)
        {
            foreach (GazeButton button in gazeUI)
            {
                button.OnGUI();
            }
        }
    }

    void Update()
    {
        //Update only if the buttons are visible
        if (isDrawing)
        {
            foreach (GazeButton button in gazeUI)
            {
                button.Update();
            }
        }

       
        if(Input.GetButtonDown("SelectGUI"))
        {
            if (isDrawing)
                isDrawing = false;
            else
                isDrawing = true; 
        }

        else if(Input.GetButtonUp("SelectGUI"))
        {
            isDrawing = false; 
        }
    }

    //ButtonActions

    public void button1_Action()
    {
        Debug.Log("Hello GazeGUI!");
    }


    public void button2_Action()
    {
        Debug.Log("Button2_Pressed");
        Application.Quit();
    }
}
