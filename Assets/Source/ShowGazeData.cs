using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;


[ExecuteInEditMode]
public class ShowGazeData : MonoBehaviour {

    
    public bool visualisationOfAccuracyWindow = true;

    public Texture2D gazeCursor;
    public GUIStyle fontStyle; 
    private int xPos;
    private int yPos;

    private string gazeButtonText = "Show GazeCursor";
    private bool isGazeCursorActive = false; 
    

    void OnGUI()
    {
        xPos = (int)(Screen.width * 0.45f);
        yPos = (int)(Screen.height * 0.45f);
        #region showGazeData
        GUI.Label(new Rect(xPos, yPos, Screen.width * 0.3f, Screen.height * 0.3f), "Data From GazeModel",fontStyle);
        GUI.Label(new Rect(xPos, yPos+40, Screen.width * 0.3f, Screen.height * 0.3f), "GazeRightEye:" + gazeModel.posRightEye.ToString(),fontStyle);
        GUI.Label(new Rect(xPos, yPos+60, Screen.width * 0.3f, Screen.height * 0.3f), "GazeLeftEye:" + gazeModel.posGazeLeft.ToString(),fontStyle);
        GUI.Label(new Rect(xPos, yPos+80, Screen.width * 0.3f, Screen.height * 0.3f), "3D-PositionEyeRight: " + gazeModel.posRightEye.ToString(),fontStyle);
        GUI.Label(new Rect(xPos, yPos+100, Screen.width * 0.3f, Screen.height * 0.3f), "3D-PositionEyeLeft: " + gazeModel.posLeftEye.ToString(),fontStyle);
        #endregion


        #region Buttons
        //Start CalibrationButton
        if (GUI.Button(new Rect(Screen.width * 0.35f, Screen.height * 0.7f, Screen.width * 0.1f, Screen.height * 0.1f), "Start Calibration"))
            gazeMonoComponent.StartCalibration();

        //Start ValidationButton
        if (GUI.Button(new Rect(Screen.width * 0.45f, Screen.height * 0.7f, Screen.width * 0.1f, Screen.height * 0.1f), "Start Validation"))
        {
            if (visualisationOfAccuracyWindow)
                gazeMonoComponent.StartValidation(1);
            else
                gazeMonoComponent.StartValidation(0);
        }

        
        //ShowGazeButton
        if (GUI.Button(new Rect(Screen.width * 0.55f, Screen.height * 0.7f, Screen.width * 0.1f, Screen.height * 0.1f), gazeButtonText))
        {
            if(!isGazeCursorActive)
            {
                isGazeCursorActive = true;
                gazeButtonText = "Hide GazeCursor";
            }
            else
            {
                isGazeCursorActive = false;
                gazeButtonText = "Show GazeCursor";

            }
        }
        #endregion

        #region drawGazeCursor
        //Draw GazeCursor only if it is activated
        if(isGazeCursorActive)
        {
            Vector3 posGaze = gazeModel.posGazeLeft+gazeModel.posGazeRight;
            posGaze.x -= 0.5f*gazeCursor.width;
            posGaze.y -= 0.5f*gazeCursor.height;
            GUI.DrawTexture(new Rect(posGaze.x, posGaze.y, gazeCursor.width, gazeCursor.height), gazeCursor);
        }
        #endregion

    }

}
