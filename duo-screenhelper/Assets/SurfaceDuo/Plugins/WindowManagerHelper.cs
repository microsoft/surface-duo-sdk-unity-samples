// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using UnityEngine;

namespace Microsoft.Device.Display
{
    public static class WindowManagerHelper
    {
        static readonly string WINDOWMANAGERPROVIDER_CLASSNAME = "androidx.window.WindowManager";

        static AndroidJavaObject windowManagerObject = null;
        static WindowManagerHelper()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            windowManagerObject = OnPlayer.Run(p =>
            {
                var activity = p.GetStatic<AndroidJavaObject>("currentActivity");

                return new AndroidJavaObject(WINDOWMANAGERPROVIDER_CLASSNAME, activity);
            });
#endif
        }

        public static RectInt GetCurrentWindowMetricsBounds()
        {
            if (windowManagerObject == null) return new RectInt(-1, -1, -1, -1);

            var windowMetricsObject = windowManagerObject.Call<AndroidJavaObject>("getCurrentWindowMetrics");
            var rect = windowMetricsObject.Call<AndroidJavaObject>("getBounds");

            Debug.LogWarning($"CurrentWindowMetricsBounds {rect}");

            if (rect == null)
            {
                return new RectInt(0, 0, 0, 0);
            }
            else {
                var left = rect.Get<int>("left");
                var top = rect.Get<int>("top");
                var width = rect.Call<int>("width");
                var height = rect.Call<int>("height");

                return new RectInt(xMin: left, yMin: top, width: width, height: height);
            }
        }

        public static RectInt GetMaximumWindowMetricsBounds()
        {
            if (windowManagerObject == null) return new RectInt(-1, -1, -1, -1);

            var windowMetricsObject = windowManagerObject.Call<AndroidJavaObject>("getMaximumWindowMetrics");
            var rect = windowMetricsObject.Call<AndroidJavaObject>("getBounds");

            Debug.LogWarning($"MaximumWindowMetricsBounds {rect}");

            if (rect == null)
            {
                return new RectInt(0, 0, 0, 0);
            }
            else
            {
                var left = rect.Get<int>("left");
                var top = rect.Get<int>("top");
                var width = rect.Call<int>("width");
                var height = rect.Call<int>("height");

                return new RectInt(xMin: left, yMin: top, width: width, height: height);
            }
        }
    }
}