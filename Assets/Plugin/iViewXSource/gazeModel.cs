using UnityEngine;
using System.Collections;

 public class gazeModel
{
    public static bool isRunning { get; set; }

    #region gazeData
    public static Vector3 posLeftEye {   get;  set; }
    public static Vector3 posRightEye {  get;  set; }

    public static Vector3 posHead {  get;  set; }

    public static Vector2 posGazeLeft {  get;  set; }
    public static Vector2 posGazeRight {  get;  set; }

    public static float diameter_leftEye {  get;  set; }
    public static float diameter_rightEye {  get;  set; }
    #endregion

    #region connectionData
    public static string IPAdressIViewX {  get;  set; }
    public static string IPAdressOwnPC {  get;  set; }

    public static int subnetMaskiViewX {  get;  set; }
    public static int subnetMaskOwnPC {  get;  set; }
    #endregion

    #region accuracyData
    public static double deviationXLeft {get;set;}
    public static double deviationYLeft { get; set; }
    public static double deviationXRight { get; set; }
    public static double deviationYRight { get; set; }
    #endregion

    #region systemStates
    public static bool isCalibration { get; set; }
    public static bool isValidation {get; set; }
    #endregion

    #region images
    public static int widthEyeImage { get; set; }
    public static int heightEyeImage { get; set; }

    public static int widthTrackingImage { get; set; }
    public static int heightTrackingImage { get; set; }
    
    public static Color32[] eyeImageColorArray { get; set; }
    public static Color32[] trackingMonitorArray { get; set; }

    public static Texture2D eyeImage { get; set; }
    public static Texture2D trackingMonitor { get; set; }
    #endregion

}
