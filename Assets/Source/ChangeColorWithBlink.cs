using UnityEngine;
using System.Collections;

public class ChangeColorWithBlink : MonoBehaviour {

    public Light light;

    private float diameterLeft;
    private float diameterRight; 

    private int dwellTime =0;
    private int maxDwellTime = 60;
    void Update()
    {
        if(gazeModel.isRunning)
        {
            diameterLeft = gazeModel.diameter_leftEye;
            diameterRight = gazeModel.diameter_rightEye;


            if ((diameterLeft <= 0 && diameterRight > 0) || (diameterLeft > 0 && diameterRight <= 0))
            {
                dwellTime++;
                
                if(dwellTime>= maxDwellTime)
                {
                        dwellTime = 0;
                    changeColorOfTheLight();
                }
            }
        }
    }


    private void changeColorOfTheLight()
    {
        Color newLightColor = new Color(Random.Range(0,25),Random.Range(0,25),Random.Range(0,25));
        light.color = newLightColor;
    }

}
