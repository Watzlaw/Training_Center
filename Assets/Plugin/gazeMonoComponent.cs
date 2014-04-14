using UnityEngine;
using System.Threading;
using System.Collections;


public class gazeMonoComponent : MonoBehaviour {

    public bool activatePictures = false;

    public bool enableGazeReactionOfObjects = true; 
    //textures for the TrackingMonitor and the eyeImage
    private Texture2D finalTextureOfEyeImage;
    private Texture2D finalTextureOfMonitors;

    //IP's 
    private string IPOwnPC = "127.0.0.1";
    private string IPServerPC = "127.0.0.1";

    //SubnetMasks of the computers
    private int SubnetMaskOwnPC =5555;
    private int SubnetMaskServerPC = 4444;

    //Visualisation of Accuracy;  0 = no Extra Window; 1 = open Extern Window with a visualisation of the Accuracy
    private static int visualisationOfAccuracy = 1;

    private static Thread eyeThread;
    private static GazeController gazeController;

    // GazeReactionObjects;
    private MonoBehaviourWithGazeComponent oldSelection; 

    // init the EyeThread with the GazeController. The gaze Controller will try to connect to the IP adress.
    private void initEyeThread()
    {
        eyeThread = new Thread(gazeController.initEyeThread);
    }

    private void startEyeThread()
    {
        eyeThread.Start();
    }

    private void joinEyeThread()
    {
        gazeController.finish();
        eyeThread.Join();
    }

    private void sleepThread(int time)
    {
        Thread.Sleep(time);
    }

    private void sleepThread(System.TimeSpan timeSpanToSleep)
    {
        Thread.Sleep(timeSpanToSleep);
    }

    private void updateConnectionInformationInGazeModel()
    {
        gazeModel.IPAdressIViewX = IPServerPC;
        gazeModel.IPAdressOwnPC = IPOwnPC;
        gazeModel.subnetMaskiViewX = SubnetMaskServerPC;
        gazeModel.subnetMaskOwnPC = SubnetMaskOwnPC;
    }

    private void startCalibrationRoutine()
    {
        if (gazeModel.isCalibration)
        {
            gazeModel.isCalibration = false;
            StartCoroutine(IEnumerator_StartCalibration());
        }
    }

    private void startValidationRoutine(int visualisationOfAccuracy)
    {
        if(gazeModel.isValidation) 
        {
            gazeModel.isValidation = false;
            StartCoroutine(IEnumerator_StartValidation(visualisationOfAccuracy));
        }
    }

    private void getCalibrationDataFromDriver()
    {
        gazeController.loadCalibration("Default");
    }

    private void updateEyeImage()
    {
        if (activatePictures)
        {
            try
            {
                Color32[] arrayColor = gazeModel.eyeImageColorArray;

                finalTextureOfEyeImage = new Texture2D(gazeModel.widthEyeImage, gazeModel.heightEyeImage, TextureFormat.RGB24, false);
                finalTextureOfEyeImage.SetPixels32(arrayColor);
                finalTextureOfEyeImage.Apply();
                gazeModel.eyeImage = finalTextureOfEyeImage;

            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e.Message);
            }
        }
    }

    private void updateTrackingMonitor()
    {
        if (activatePictures)
        {
            try
            {
                Color32[] arrayColor = gazeModel.trackingMonitorArray;

                finalTextureOfMonitors = new Texture2D(gazeModel.widthTrackingImage, gazeModel.heightTrackingImage, TextureFormat.RGB24, false);
                finalTextureOfMonitors.SetPixels32(arrayColor);
                finalTextureOfMonitors.Apply();
                gazeModel.trackingMonitor = finalTextureOfMonitors;

            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e.Message);
            }
        }
    }

    private void manageCoroutineUpdateTrackingScreen()
    {
        StartCoroutine(IEnumerator_UpdateTrackingScreen());
    }

    private void manageCoroutineUpdateEyeImage()
    {
        StartCoroutine(IEnumerator_UpdateEyeImage());
    }
   
    private void rayCastGazeRay()
    {
        Ray raygaze;
        RaycastHit hit;


        //CheckINput;
        if(gazeModel.isRunning)
        {
            raygaze = Camera.main.ScreenPointToRay(gazeModel.posGazeRight);
        }   
            
        else
        {
            raygaze = Camera.main.ScreenPointToRay(Input.mousePosition);
        }



        if (Physics.Raycast(raygaze, out hit, 1500f))
        {
            MonoBehaviourWithGazeComponent hitMono =hit.collider.gameObject.GetComponent<MonoBehaviourWithGazeComponent>();
            if(hitMono != null)
            {
                // Save the OldSelection
                if (oldSelection == null)
                    oldSelection = hitMono;

                else if(hitMono != oldSelection)
                {
                    oldSelection.OnObjectExit();
                    oldSelection = hitMono;
                }
                // Invoke Start and Update of the GazeEvent
                hitMono.OnObjectHit(hit);

            }

        }
        else
        {
            if(oldSelection != null)
            {
                oldSelection.OnObjectExit();
                oldSelection = null; 
            }
        }
    }
    
    public static void StartCalibration()
    {
        gazeModel.isCalibration = true;
    }

    public static void StartValidation(int visualisationOfAccuracy)
    {
        gazeMonoComponent.visualisationOfAccuracy = visualisationOfAccuracy;
        gazeModel.isValidation = true;
    }

    public static void ShowTrackingMonitor()
    {
        gazeController.showTrackingMonitor();
    }

    public static void ShowEyeImageMonitor()
    {
        gazeController.showEyeImageMonitor();
    }


    void Start()
    {
        gazeModel.isRunning = false;
        updateConnectionInformationInGazeModel();
        gazeController = new GazeController();

        initEyeThread();
        startEyeThread();

            InvokeRepeating("manageCoroutineUpdateTrackingScreen", 2f, 0.2f);
            InvokeRepeating("manageCoroutineUpdateEyeImage", 2f, 0.2f);
    }
    
    void Update()
    {
        if (gazeModel.isRunning)
        {
            startCalibrationRoutine();
            startValidationRoutine(visualisationOfAccuracy);
        }
        
        if(enableGazeReactionOfObjects)
        {
            rayCastGazeRay();
        }
        
    }

    void OnApplicationQuit()
    {
        joinEyeThread();
    }


    IEnumerator IEnumerator_StartCalibration()
    {
        if (Screen.fullScreen == true)
            Screen.fullScreen = false;

        yield return new WaitForFixedUpdate();
        gazeController.startCalibration();
        Screen.fullScreen = true;
        yield return null;
    }

    IEnumerator IEnumerator_StartValidation(int visualisationOfAccuracy)
    {
        if (Screen.fullScreen == true)
            Screen.fullScreen = false;
        yield return new WaitForFixedUpdate();
        gazeController.startValidation(visualisationOfAccuracy);
        Screen.fullScreen = true;
        yield return null;
    }

    IEnumerator IEnumerator_UpdateTrackingScreen()
    {
        updateTrackingMonitor();
        yield return new WaitForEndOfFrame();
        System.GC.Collect();
        yield return null;
    }

    IEnumerator IEnumerator_UpdateEyeImage()
    {
        updateEyeImage();
        yield return new WaitForEndOfFrame();
        System.GC.Collect();
        yield return null;
    }
}
