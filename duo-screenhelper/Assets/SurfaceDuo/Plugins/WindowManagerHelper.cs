// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using UnityEngine;

namespace Microsoft.Device.Display
{
    public static class WindowManagerHelper
    {
        static readonly string WINDOWMANAGERPROVIDER_CLASSNAME = "androidx.window.WindowManager";

        static AndroidJavaObject windowManagerObject = null;

        static AndroidJavaObject foldablePlayerActivity = null;

        static WindowManagerHelper()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            foldablePlayerActivity = OnPlayer.Run(p =>
            {
                return p.GetStatic<AndroidJavaObject>("currentActivity");
            });            

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

        public static bool IsSeparating() {
#if !UNITY_EDITOR && UNITY_ANDROID
            var foldingFeatureObject = foldablePlayerActivity.Call<AndroidJavaObject>("getFoldingFeature");
            if (foldingFeatureObject == null)
            {
                return false;
            }
            else 
            {
                return foldingFeatureObject.Call<bool>("isSeparating");
            }
#else
            return false;
#endif
        }

        public static string Orientation()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            var foldingFeatureObject = foldablePlayerActivity.Call<AndroidJavaObject>("getFoldingFeature");
            if (foldingFeatureObject == null)
            {
                return "n/a";
            }
            else 
            {
                var orientation = foldingFeatureObject.Call<AndroidJavaObject>("getOrientation");
                var orientationString = orientation.Call<string>("toString");
                Debug.LogWarning($"Orientation {orientationString}");
                return orientationString;
            }
#else
            return "n/a";
#endif
        }

        public static string OcclusionType()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            var foldingFeatureObject = foldablePlayerActivity.Call<AndroidJavaObject>("getFoldingFeature");
            if (foldingFeatureObject == null)
            {
                return "n/a";
            }
            else
            {
                var occlusionType = foldingFeatureObject.Call<AndroidJavaObject>("getOcclusionType");
                var occlusionTypeString = occlusionType.Call<string>("toString");
                Debug.LogWarning($"Orientation {occlusionTypeString}");
                return occlusionTypeString;
            }
#else
            return "n/a";
#endif
        }


        public static string State()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            var foldingFeatureObject = foldablePlayerActivity.Call<AndroidJavaObject>("getFoldingFeature");
            if (foldingFeatureObject == null)
            {
                return "n/a";
            }
            else
            {
                var state = foldingFeatureObject.Call<AndroidJavaObject>("getState");
                var stateString = state.Call<string>("toString");
                Debug.LogWarning($"State {stateString}");
                return stateString;
            }
#else
            return "n/a";
#endif
        }

        public static RectInt Bounds()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            var foldingFeatureObject = foldablePlayerActivity.Call<AndroidJavaObject>("getFoldingFeature");
            if (foldingFeatureObject == null)
            {
                return new RectInt(0, 0, 0, 0); ;
            }
            else
            {
                var rect = foldingFeatureObject.Call<AndroidJavaObject>("getBounds");

                Debug.LogWarning($"Bounds {rect}");

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
#else
            return new RectInt(0, 0, 0, 0);
#endif
        }
    }
}