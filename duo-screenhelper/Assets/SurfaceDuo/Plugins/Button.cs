// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Microsoft.Device.Display;
using System;
using UnityEngine;

public class Button : MonoBehaviour
{
    HingeSensor hingeSensor;

    void Start()
    {
        hingeSensor = HingeSensor.Start();
    }

    void OnApplicationQuit()
    {
        hingeSensor?.StopSensing();
    }
    // layout constants
    const float LEFT_MARGIN = 20f;
    const float ROW_HEIGHT = 50.0f;
    const float COL_WIDTH = 570.0f;
    const float HEAD_INDENT = 410.0f;
    private void OnGUI()
    {
        //Changes both color and font size
        GUIStyle localStyle = new GUIStyle();
        localStyle.normal.textColor = Color.black;
        localStyle.fontSize = 45;

#if UNITY_EDITOR
        // Hardcode the hinge mask for the Unity game preview
        if (Screen.width == DeviceHelper.SURFACEDUO_SPANNEDWIDTH)
        { // double-portrait
            GUI.backgroundColor = Color.gray;
            GUI.Box(new Rect(x: DeviceHelper.SURFACEDUO_SCREENWIDTH, y: 0, width: DeviceHelper.SURFACEDUO_HINGEWIDTH, height: DeviceHelper.SURFACEDUO_SCREENHEIGHT),"");
        }
        else if (Screen.height == DeviceHelper.SURFACEDUO_SPANNEDHEIGHT)
        { // double-landscape
            GUI.backgroundColor = Color.gray;
            var r = new Rect(x: 0, y: DeviceHelper.SURFACEDUO_SCREENWIDTH, width: DeviceHelper.SURFACEDUO_SCREENHEIGHT, height: DeviceHelper.SURFACEDUO_HINGEWIDTH);
            GUI.Box(r, "");
        }
#endif
        
        GUI.Label(new Rect(HEAD_INDENT, ROW_HEIGHT * 1, 200, 20), "-Unity (2020.3.14) for Surface Duo-", localStyle);
        
        GUI.Label(new Rect(LEFT_MARGIN, ROW_HEIGHT * 3, 200, 20), "Unity screen orientation:", localStyle);
        GUI.Label(new Rect(HEAD_INDENT, ROW_HEIGHT * 4, 400, 20), "-DeviceHelper-", localStyle);
        GUI.Label(new Rect(LEFT_MARGIN, ROW_HEIGHT * 5, 200, 20), "IsDualScreenDevice:", localStyle);
        GUI.Label(new Rect(HEAD_INDENT, ROW_HEIGHT * 6, 200, 20), "-ScreenHelper-", localStyle);
        
        GUI.Label(new Rect(LEFT_MARGIN, ROW_HEIGHT * 7, 200, 20), "IsDualMode:", localStyle);
        GUI.Label(new Rect(LEFT_MARGIN, ROW_HEIGHT * 8, 200, 20), "GetCurrentRotation:", localStyle);
        GUI.Label(new Rect(LEFT_MARGIN, ROW_HEIGHT * 9, 200, 20), "GetHinge:", localStyle);
        GUI.Label(new Rect(LEFT_MARGIN, ROW_HEIGHT * 10, 200, 20), "GetScreenRectangles:", localStyle);
                                                                 // GetScreenRectangles returns two rows
        GUI.Label(new Rect(HEAD_INDENT, ROW_HEIGHT * 12, 200, 20), "-Sensor-", localStyle);
        GUI.Label(new Rect(LEFT_MARGIN, ROW_HEIGHT * 13, 200, 20), "Hinge angle:", localStyle);

        GUI.Label(new Rect(HEAD_INDENT, ROW_HEIGHT * 15, 200, 20), "-Window Manager (beta02)-", localStyle);
        GUI.Label(new Rect(LEFT_MARGIN, ROW_HEIGHT * 16, 200, 20), "CurrentWindowMetrics:", localStyle);
        GUI.Label(new Rect(LEFT_MARGIN, ROW_HEIGHT * 17, 200, 20), "MaximumWindowMetrics:", localStyle);
        GUI.Label(new Rect(HEAD_INDENT, ROW_HEIGHT * 18, 200, 20), "-FoldingFeature-", localStyle);
        GUI.Label(new Rect(LEFT_MARGIN, ROW_HEIGHT * 19, 200, 20), "IsSeparating:", localStyle);
        GUI.Label(new Rect(LEFT_MARGIN, ROW_HEIGHT * 20, 200, 20), "Orientation:", localStyle);
        GUI.Label(new Rect(LEFT_MARGIN, ROW_HEIGHT * 21, 200, 20), "OcclusionType:", localStyle);
        GUI.Label(new Rect(LEFT_MARGIN, ROW_HEIGHT * 22, 200, 20), "State:", localStyle);
        GUI.Label(new Rect(LEFT_MARGIN, ROW_HEIGHT * 23, 200, 20), "Bounds:", localStyle);

        localStyle.normal.textColor = Color.blue;

        // These methods don't require a check for IsDualScreenDevice
        GUI.Label(new Rect(COL_WIDTH, ROW_HEIGHT * 3, 400, 20), Screen.orientation.ToString(), localStyle);
        GUI.Label(new Rect(COL_WIDTH, ROW_HEIGHT * 5, 400, 20), ScreenHelper.IsDualScreenDevice().ToString(), localStyle);
        
        if (DeviceHelper.IsDualScreenDevice())
        {
            try
            {
                // draw the DisplayMask rectangle, but with a 25 pixel bleed so you can see on the device
                var displayMask = DisplayMask.FromResourcesRect();
                foreach (var rect in displayMask.GetBoundingRects())
                {
                    var o = Screen.orientation;
                    if (o == ScreenOrientation.LandscapeLeft || o == ScreenOrientation.LandscapeRight)
                    {   // wide
                        var r = new Rect(x: rect.x - 25, y: rect.y, width: rect.width + 50, height: rect.height);
                        GUI.Box(r, "");
                    }
                    else
                    {   // portrait - tall
                        var r = new Rect(x: rect.y, y: rect.x - 25, width: rect.height, height: rect.width + 50);
                        GUI.Box(r, "");
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("DisplayMask.FromResourcesRect: " + e);
            }

            try
            {
                var isDualMode = ScreenHelper.IsDualMode();
                GUI.Label(new Rect(COL_WIDTH, ROW_HEIGHT * 7, 400, 20), isDualMode.ToString() + "     (same as 'Is Spanned?')", localStyle);

            }
            catch (System.Exception e)
            {
                Debug.LogWarning("ScreenHelper.IsDualMode: " + e);
                localStyle.fontSize = 24; // to read the exception message
                GUI.Label(new Rect(200, ROW_HEIGHT * 7, 400, 20), e.ToString(), localStyle);
            }

            try
            {
                var currentRotation = ScreenHelper.GetCurrentRotation();
                GUI.Label(new Rect(COL_WIDTH, ROW_HEIGHT * 8, 400, 20), currentRotation.ToString(), localStyle);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("ScreenHelper.GetCurrentRotation: " + e);
            }

            try
            {
                var hinge = ScreenHelper.GetHinge();
                GUI.Label(new Rect(COL_WIDTH, ROW_HEIGHT * 9, 400, 20), hinge.ToString(), localStyle);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("ScreenHelper.GetHinge: " + e);
            }

            try
            {
                var rects = ScreenHelper.GetScreenRectangles();
                var rectString = "";
                foreach (var rect in rects) {
                    rectString += rect.ToString() + "," + Environment.NewLine;
                }
                GUI.Label(new Rect(COL_WIDTH, ROW_HEIGHT * 10, 1000, 20 * 2), rectString, localStyle);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("ScreenHelper.GetHinge: " + e);
            }

            if (hingeSensor != null)
            {
                try
                {
                    var angle = hingeSensor.GetHingeAngle();
                    GUI.Label(new Rect(COL_WIDTH, ROW_HEIGHT * 13, 400, 20), $"{angle}°", localStyle);
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"Hinge sensor read error: {e}");
                    GUI.Label(new Rect(COL_WIDTH, ROW_HEIGHT * 13, 400, 20), e.ToString(), localStyle);
                }
            }
            else
            {
                GUI.Label(new Rect(COL_WIDTH, ROW_HEIGHT * 13, 400, 20), $"Error creating hinge sensor reader", localStyle);
            }



            #region Window Manager
            try
            {
                var rect = WindowManagerHelper.GetCurrentWindowMetricsBounds();
                GUI.Label(new Rect(COL_WIDTH, ROW_HEIGHT * 16, 200, 20), rect.ToString(), localStyle);

                rect = WindowManagerHelper.GetMaximumWindowMetricsBounds();
                GUI.Label(new Rect(COL_WIDTH, ROW_HEIGHT * 17, 200, 20), rect.ToString(), localStyle);
            }
            catch (System.Exception e)
            {
                //GUI.Label(new Rect(LEFT_MARGIN, ROW_HEIGHT * 18, 1000, 20), e.ToString(), localStyle);
                Debug.LogWarning("WindowManagerHelper.GetCurrentWindowMetricsBounds: " + e);
            }


            try
            {
                var isSeparating = WindowManagerHelper.IsSeparating();
                GUI.Label(new Rect(COL_WIDTH, ROW_HEIGHT * 19, 200, 20), isSeparating.ToString(), localStyle);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("WindowManagerHelper.IsSeparating: " + e);
            }

            try
            {
                var orientation = WindowManagerHelper.Orientation();
                GUI.Label(new Rect(COL_WIDTH, ROW_HEIGHT * 20, 200, 20), orientation, localStyle);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("WindowManagerHelper.Orientation: " + e);
            }

            try
            {
                var occlusionType = WindowManagerHelper.OcclusionType();
                GUI.Label(new Rect(COL_WIDTH, ROW_HEIGHT * 21, 200, 20), occlusionType, localStyle);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("WindowManagerHelper.OcclusionType: " + e);
            }

            try
            {
                var state = WindowManagerHelper.State();
                GUI.Label(new Rect(COL_WIDTH, ROW_HEIGHT * 22, 200, 20), state, localStyle);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("WindowManagerHelper.State: " + e);
            }

            try
            {
                var bounds = WindowManagerHelper.Bounds();
                GUI.Label(new Rect(COL_WIDTH, ROW_HEIGHT * 23, 200, 20), bounds.ToString(), localStyle);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("WindowManagerHelper.Bounds: " + e);
            }
            #endregion
        }
#if UNITY_EDITOR
        else
        {
            GUI.Label(new Rect(LEFT_MARGIN, ROW_HEIGHT * 22, 400, 20), "(most dual-screen attributes have no value in editor)", localStyle);
        }
#endif
    }
}
