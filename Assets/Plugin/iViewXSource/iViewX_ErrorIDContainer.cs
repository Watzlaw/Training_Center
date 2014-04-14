using System;
using UnityEngine;
using System.Collections.Generic;

public class iViewX_ErrorIDContainer {

    public const int STATE_CONNECT = 0;
    public const int STATE_DISCONNET = 1;
    public const int STATE_SETUPCALIBRATION = 2;
    public const int STATE_CALIBRATE = 3;
    public const int STATE_SETUPVALIDATE = 4;
    public const int STATE_VALIDATE = 5;

    private static Dictionary<int, string> ErrorId = new Dictionary<int, string>()
        {
            {1, "intended functionality has been fulfilled"},
            {2, "No new data available"},
            {3, "Calibration was aborted"},
            {100, "failed to establish connection"},
            {101, "no connection established"},
            {102, "system is not calibrated"},
            {103, "system is not validated"},
            {104, "no SMI eye tracking application running"},
            {105, "wrong port settings"},
            {111, "eye tracking device required for this function is not connected"},
            {112, "parameter out of range"},
            {113, "eye tracking device required for this calibration method is not connected"},
            {121, "failed to create sockets"},
            {122, "failed to connect sockets"},
            {123, "failed to bind sockets"},
            {124, "failed to delete sockets"},
            {131, "no response from iView X; check iView X connection settings (IP addresses, ports) or last command"},
            {132, "iView X version could not be resolved"},
            {133, "wrong version of iView X"},
            {171, "failed to access log file"},
            {181, "socket error during data transfer"},
            {191, "recording buffer is empty"},
            {192, "recording is activated"},
            {193, "data buffer is full"},
            {194, "iView X is not ready"},
            
            {201, "no installed SMI eye tracking application detected"},
            {220, "Could not open port for TTL output"},
            {221, "Could not close port for TTL output"},
            {222, "Could not access AOI data"},
        };

    private static Dictionary<int, string> ErrorState = new Dictionary<int, string>()
        {
            {0, "Connect"},
            {1, "Disconnect"},
            {2, "Setup Calibration"},
            {3, "Calibration"},
            {4, "Setup Validation"},
            {5, "Validation"}

        };

    public static string getErrorMessage(int id)
    {
        return ErrorId[id];
    }

    public static string getState(int id)
    {
        return ErrorState[id];
    }
}
