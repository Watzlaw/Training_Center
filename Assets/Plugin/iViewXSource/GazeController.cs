using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Runtime.InteropServices;
using EyeTrackingController;

public class GazeController
{
    private EyeTrackingController.EyeTrackingController ETDevice;


    #region SampleData
    EyeTrackingController.EyeTrackingController.CalibrationStruct eyeTracker_CalibrationData;       
    EyeTrackingController.EyeTrackingController.SampleStruct eyeTracker_sampleData;                 
    EyeTrackingController.EyeTrackingController.AccuracyStruct eyeTracker_AccuracyData;             
    EyeTrackingController.EyeTrackingController.SystemInfoStruct eyeTracker_SystemInfoData;
    
    EyeTrackingController.EyeTrackingController.ImageStruct eyeTracker_trackingMonitor;
    EyeTrackingController.EyeTrackingController.ImageStruct eyeTracer_eyeImage;

    # endregion
   
    #region CallbackRoutine
    private delegate void CalibrationCallback(EyeTrackingController.EyeTrackingController.CalibrationPointStruct calibrationPointData);
    private delegate void GetSampleCallback(EyeTrackingController.EyeTrackingController.SampleStruct sampleData);
    private delegate void GetEyeImagedata(EyeTrackingController.EyeTrackingController.ImageStruct imageData);
    private delegate void GetTrackingMonitor(EyeTrackingController.EyeTrackingController.ImageStruct imageData);
    # endregion
    
    #region Instances of the Callbacks
    CalibrationCallback calibrationCallback;
    GetSampleCallback sampleDataCallback;
    GetEyeImagedata eyeImageCallback;
    GetTrackingMonitor trackingMonitorCallback;
    #endregion

    #region CallbackFunctions
    void getCalibration(EyeTrackingController.EyeTrackingController.CalibrationPointStruct calibrationPoints)
    {

    }
    
    //Write the Data from the current Sample into the Gazemodel.cs
    void getSampleData( EyeTrackingController.EyeTrackingController.SampleStruct sampleData)
    {
        if (gazeModel.isRunning)
        {
            //Set Position of the Eyes
            gazeModel.posRightEye = new Vector3((float)sampleData.rightEye.eyePositionX, (float)sampleData.rightEye.eyePositionY, (float)sampleData.rightEye.eyePositionZ);
            gazeModel.posLeftEye = new Vector3((float)sampleData.leftEye.eyePositionX, (float)sampleData.leftEye.eyePositionY, (float)sampleData.leftEye.eyePositionZ);

            //Set Position of the Head
            gazeModel.posHead = new Vector3(0.5f * (gazeModel.posLeftEye.x + gazeModel.posRightEye.x),
                                            0.5f * (gazeModel.posLeftEye.y + gazeModel.posRightEye.y),
                                            0.5f * (gazeModel.posLeftEye.z + gazeModel.posRightEye.z));
            //TEST: yPosition right?


            //Set Gaze Left&Right
            gazeModel.posGazeLeft = new Vector2((float)sampleData.leftEye.gazeX, (float)sampleData.leftEye.gazeY);
            gazeModel.posGazeRight = new Vector2((float)sampleData.rightEye.gazeX, (float)sampleData.rightEye.gazeY);

            //Set Diameter of the Eyes
            gazeModel.diameter_leftEye = (float)sampleData.leftEye.diam;
            gazeModel.diameter_rightEye = (float)sampleData.rightEye.diam;
        }

    }

    void getEyeImagedata(EyeTrackingController.EyeTrackingController.ImageStruct imageData)
    {
        if (gazeModel.isRunning)
        {
            //TODO: CONVERT MONO TO RGB
            gazeModel.widthEyeImage = imageData.imageWidth;
            gazeModel.heightEyeImage = imageData.imageHeight;

            int colorchannels = 3;
            byte[] imageBuffer = new byte[imageData.imageSize];

            Marshal.Copy(imageData.imageBuffer, imageBuffer, 0, imageData.imageSize);
            Color32[] colorArray = new Color32[imageData.imageSize];


            for (int i = 0; i < imageBuffer.Length; i += colorchannels)
            {
                Color32 color = new Color32(imageBuffer[i], imageBuffer[i + 1], imageBuffer[i + 2], 255);
                colorArray[i / colorchannels] = color;
            }

            gazeModel.eyeImageColorArray = colorArray;
            System.GC.Collect();
        }
    }

    void getTrackingMonitordata(EyeTrackingController.EyeTrackingController.ImageStruct imageData)
    {
        if (gazeModel.isRunning)
        {
            gazeModel.widthTrackingImage = imageData.imageWidth;
            gazeModel.heightTrackingImage = imageData.imageHeight;

            byte[] imageBuffer = new byte[imageData.imageSize];
            Marshal.Copy(imageData.imageBuffer, imageBuffer, 0, imageData.imageSize);
            Color32[] colorArray = new Color32[imageData.imageSize / 3];

            for (int i = 0; i < imageBuffer.Length; i += 3)
            {
                Color32 color = new Color32(imageBuffer[i], imageBuffer[i], imageBuffer[i], 255);
                colorArray[i / 3] = color;
            }

            gazeModel.trackingMonitorArray = colorArray;
            System.GC.Collect();
        }
        
    }
    #endregion

    private void getLogdata(int errorID, int state)
    {
        
        if (errorID > 1)
            Debug.LogError("Error by " + iViewX_ErrorIDContainer.getState(state) + ": " + iViewX_ErrorIDContainer.getErrorMessage(errorID));
        else
            Debug.Log(iViewX_ErrorIDContainer.getState(state) + " finished");
    }
    
    private int connectToServer()
    {
        int errorID = 0;
        try
        {
            errorID = ETDevice.iV_Connect(new StringBuilder(gazeModel.IPAdressIViewX), gazeModel.subnetMaskiViewX, new StringBuilder(gazeModel.IPAdressIViewX), gazeModel.subnetMaskOwnPC);
        }

        catch (System.Exception e)
        {
            errorID = 99;
            Debug.LogError(e.Message);
        }

        if (errorID == 1)
            gazeModel.isRunning = true;
        
        return errorID;
    }

    private int disconnectFromServer()
    {
        int errorID = 0;
        try
        {
            errorID = ETDevice.iV_Disconnect();

        }

        catch (System.Exception e)
        {
            errorID = 99;
            Debug.LogError(e.Message);
        }

        if(errorID ==1)
        gazeModel.isRunning = false;
        
        return errorID;
    }

    private void setCallbacks()
    {
        ETDevice.iV_SetCalibrationCallback(calibrationCallback);
        ETDevice.iV_SetSampleCallback(sampleDataCallback);
        ETDevice.iV_SetTrackingMonitorCallback(trackingMonitorCallback);
        ETDevice.iV_SetEyeImageCallback(eyeImageCallback);

    }

    public GazeController()
    {
        //Init the Controller
        ETDevice = new EyeTrackingController.EyeTrackingController();

        //Init the Callbacks
        calibrationCallback = new CalibrationCallback(getCalibration);
        sampleDataCallback = new GetSampleCallback(getSampleData);
        eyeImageCallback = new GetEyeImagedata(getEyeImagedata);
        trackingMonitorCallback = new GetTrackingMonitor(getTrackingMonitordata);

        setCallbacks();
        ETDevice.iV_EnableGazeDataFilter();
    
    }

    public void initEyeThread()
    {
        int errorID = connectToServer();
        
        //ErrorMessage
        getLogdata(errorID, iViewX_ErrorIDContainer.STATE_CONNECT);
    }

    public void finish()
    {
        int errorID = disconnectFromServer();

        //ErrorMessage
        getLogdata(errorID, iViewX_ErrorIDContainer.STATE_DISCONNET);
    }

    public void loadCalibration(string id_Calibration) 
    {
        int errorID = ETDevice.iV_LoadCalibration(new StringBuilder(id_Calibration));

        //ErrorMessage
        getLogdata(errorID, iViewX_ErrorIDContainer.STATE_CALIBRATE);
    }

    public void startCalibration()
    {
        int errorID = 0;
                
        //get the data from the DriverSoftware
        errorID = ETDevice.iV_GetCalibrationParameter(ref eyeTracker_CalibrationData);
        getLogdata(errorID, iViewX_ErrorIDContainer.STATE_SETUPCALIBRATION);

        
        //start the calibration
        errorID = ETDevice.iV_Calibrate();
        getLogdata(errorID, iViewX_ErrorIDContainer.STATE_CALIBRATE);

    }

    public void startValidation(int accuracyVisualisation)
    {
        int errorID = ETDevice.iV_Validate();
        
        //ErrorMessage of StartValidation
        getLogdata(errorID, iViewX_ErrorIDContainer.STATE_SETUPVALIDATE);
        
        getAccuracy(accuracyVisualisation);
    }

    public void getAccuracy(int visualisation)
    {

        int errorID = ETDevice.iV_GetAccuracy(ref eyeTracker_AccuracyData,visualisation);

        //Write devationData into GazeModel
        gazeModel.deviationXLeft = eyeTracker_AccuracyData.deviationXLeft;
        gazeModel.deviationYLeft = eyeTracker_AccuracyData.deviationYLeft;
        gazeModel.deviationXRight = eyeTracker_AccuracyData.deviationXRight;
        gazeModel.deviationYRight = eyeTracker_AccuracyData.deviationYRight;

        //ErrorMessage
        getLogdata(errorID, iViewX_ErrorIDContainer.STATE_VALIDATE);
    }

    public void showEyeImageMonitor()
    {
        ETDevice.iV_ShowEyeImageMonitor();
    }

    public void showTrackingMonitor()
    {
        ETDevice.iV_ShowTrackingMonitor();
    }


}

