// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using UnityEngine;

namespace Microsoft.Device.Display
{
    /// <summary>
    /// Zero-dependency helper class to check if running on a dual-screen Surface Duo
    /// before using ScreenHelper or DisplayMask APIs.
    /// </summary>
    public static class DeviceHelper
    {
        /// <summary>2784 - width when game spanned over double-portrait (wide)</summary>
        public const int SURFACEDUO_SPANNEDWIDTH = 2784;
        /// <summary>2784 - height when game spanned over double-landscape (tall)</summary>
        public const int SURFACEDUO_SPANNEDHEIGHT = 2784;
        /// <summary>1350 - single screen width in portrait (or the height in landscape)</summary>
        public const int SURFACEDUO_SCREENWIDTH = 1350;
        /// <summary>1800 - single screen height in portrait (or the width in landscape)</summary>
        public const int SURFACEDUO_SCREENHEIGHT = 1800;
        /// <summary>84</summary>
        public const int SURFACEDUO_HINGEWIDTH = 84;

        /// <summary>
        /// Determine whether your app is running on a dual-screen device. 
        /// You should perform this check before you call APIs from the Surface Duo SDK
        /// </summary>
        /// <remarks>
        /// https://docs.microsoft.com/dual-screen/android/sample-code/is-dual-screen-device
        /// </remarks>
        public static bool IsDualScreenDevice()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            try
            {
                return OnPlayer.Run(p => p
                    .GetStatic<AndroidJavaObject>("currentActivity")
                    .Call<AndroidJavaObject>("getApplicationContext")
                    .Call<AndroidJavaObject>("getPackageManager")
                    .Call<bool>("hasSystemFeature", "com.microsoft.device.display.displaymask"));
            }
            catch
            {
                return false;
            }
#else
            return false;
#endif
        }
    }

    /// <summary>
    /// Surface Duo ScreenHelper class.
    /// IMPORTANT: call <see cref="DeviceHelper.IsDualScreenDevice"/> before using methods on this class.
    /// </summary>
    /// <remarks>
    /// Requires updates to mainTemplate.gradle (the package is in jcenter):
    /// 
    /// dependencies {
    ///     implementation "com.microsoft.device.dualscreen:screenmanager-displaymask:1.0.0-beta1"
    ///     implementation "org.jetbrains.kotlin:kotlin-stdlib-jdk7:1.3.61"
    /// }
    /// 
    /// Docs:
    /// https://docs.microsoft.com/dual-screen/android/api-reference/dualscreen-library/core/screen-info
    /// </remarks>
    public class ScreenHelper
    {
        /// <summary>
        /// com.microsoft.device.dualscreen.ScreenInfo
        /// </summary>
        /// <remarks>_was_ com.microsoft.device.dualscreen.layout.ScreenHelper</remarks>
        [Obsolete("The class is obtained by the provider now")]
        static string SCREENHELPER_CLASSNAME = "com.microsoft.device.dualscreen.ScreenInfo";

        /// <summary>
        /// com.microsoft.device.dualscreen.ScreenInfoProvider
        /// </summary>
        /// <remarks>static class to get a reference to the ScreenInfo object</remarks>
        static string SCREENINFOPROVIDER_CLASSNAME = "com.microsoft.device.dualscreen.ScreenInfoProvider";

        /// <summary>
        /// Determine whether your app is running on a dual-screen device. 
        /// You should perform this check before you call APIs from the Surface Duo SDK.
        /// This method uses regular Android hasSystemFeature check, it does NOT require
        /// any custom SDK be referenced.
        /// </summary>
        /// <remarks>
        /// https://docs.microsoft.com/en-us/dual-screen/android/sample-code/is-dual-screen-device?tabs=java
        /// </remarks>
        public static bool IsDualScreenDevice()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            try
            {
                return OnPlayer.Run(p => p
                    .GetStatic<AndroidJavaObject>("currentActivity")
                    .Call<AndroidJavaObject>("getApplicationContext")
                    .Call<AndroidJavaObject>("getPackageManager")
                    .Call<bool>("hasSystemFeature", "com.microsoft.device.display.displaymask"));
            }
            catch
            {
                return false;
            }
#else
            return false;
#endif
        }
        /// <summary>
        /// Is the device a dual-screen Surface Duo?
        /// Uses the SDK ScreenHelper.isDeviceSurfaceDuo method
        /// </summary>
        public static bool IsDeviceSurfaceDuo()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            try
            {
                var isDuo = OnPlayer.Run(p =>
                {
                    var activity = p.GetStatic<AndroidJavaObject>("currentActivity");

                    using (var sc = new AndroidJavaClass(SCREENINFOPROVIDER_CLASSNAME))
                    {
                        var si = sc.CallStatic<AndroidJavaObject>("getScreenInfo", activity);
                        return si.Call<bool>("isSurfaceDuoDevice"); // was isDeviceSurfaceDuo
                    }
                });
                return isDuo;
            }
            catch
            {
                return false;
            }
#else
            return false;
#endif
        }

        /// <summary>
        /// Returns the coordinates of the Hinge in a Rect object.
        /// </summary>
        public static RectInt GetHinge()
        {
            var hinge = OnPlayer.Run(p =>
            {
                var activity = p.GetStatic<AndroidJavaObject>("currentActivity");

                using (var sc = new AndroidJavaClass(SCREENINFOPROVIDER_CLASSNAME))
                {
                    var si = sc.CallStatic<AndroidJavaObject>("getScreenInfo", activity);
                    return si.Call<AndroidJavaObject>("getHinge");
                }
            });

            if (hinge != null)
            {
                var left = hinge.Get<int>("left");
                var top = hinge.Get<int>("top");
                var width = hinge.Call<int>("width");
                var height = hinge.Call<int>("height");

                return new RectInt(left, top, width, height);
            }
            else return new RectInt (0,0,0,0); // TODO: cannot return null - is ok?
        }

        /// <summary>
        /// Returns a boolean that indicates whether the application is in spanned mode or not
        /// </summary>
        public static bool IsDualMode()
        {
            var isDualMode = OnPlayer.Run(p =>
            {
                var activity = p.GetStatic<AndroidJavaObject>("currentActivity");
                using (var sc = new AndroidJavaClass(SCREENINFOPROVIDER_CLASSNAME))
                {
                    var si = sc.CallStatic<AndroidJavaObject>("getScreenInfo", activity);
                    return si.Call<bool>("isDualMode");
                }
            });
            return isDualMode;
        }

        /// <summary>
        /// Returns the rotation of the screen - Surface.ROTATION_0 (0), Surface.ROTATION_90 (1), Surface.ROTATION_180 (2), Surface.ROTATION_270 (3)
        /// </summary>
        /// <remarks>_was_ getCurrentRotation</remarks>
        public static int GetCurrentRotation()
        {
            var rotation = OnPlayer.Run(p =>
            {
                var activity = p.GetStatic<AndroidJavaObject>("currentActivity");
                using (var sc = new AndroidJavaClass(SCREENINFOPROVIDER_CLASSNAME))
                {
                    var si = sc.CallStatic<AndroidJavaObject>("getScreenInfo", activity);
                    return si.Call<int>("getScreenRotation"); // was getCurrentRotation
                }
            });
            return rotation;
        }

        /// <summary>
        /// Returns a list of two elements that contain the coordinates of the screen rectangles
        /// </summary>
        public static RectInt[] GetScreenRectangles()
        {
            var jScreenRects = OnPlayer.Run(p =>
            {
                var activity = p.GetStatic<AndroidJavaObject>("currentActivity");
                using (var sc = new AndroidJavaClass(SCREENINFOPROVIDER_CLASSNAME))
                {
                    var si = sc.CallStatic<AndroidJavaObject>("getScreenInfo", activity);
                    return si.Call<AndroidJavaObject>("getScreenRectangles");
                }
            });

            var size = jScreenRects.Call<int>("size");
            if (size > 0)
            {
                var rectangles = new RectInt[size];
                for (var i = 0; i < size; i++)
                {
                    var jRect = jScreenRects.Call<AndroidJavaObject>("get", i);

                    var left = jRect.Get<int>("left");
                    var top = jRect.Get<int>("top");
                    var width = jRect.Call<int>("width");
                    var height = jRect.Call<int>("height");

                    rectangles[i] = new RectInt(left, top, width, height);
                    Debug.LogWarning($"GetScreenRectangles [{i}]: {rectangles[i]}");
                }
                return rectangles;
            }
            else return new RectInt[0]; // TODO: return null??
        }
    }

    /// <summary>
    /// Rotation enumeration
    /// </summary>
    public static class Surface
    {
        public static int ROTATION_0 = 0;
        public static int ROTATION_90 = 1;
        public static int ROTATION_180 = 2;
        public static int ROTATION_270 = 3;
    }

    /// <summary>
    /// Surface Duo DisplayMask helper. 
    /// Call <see cref="DeviceHelper.IsDualScreenDevice"/> before using methods on this class.
    /// </summary>
    /// <remarks>
    /// https://docs.microsoft.com/dual-screen/android/api-reference/display-mask
    /// </remarks>
    public class DisplayMask : IDisposable
    {
        /// <summary>
        /// com.microsoft.device.display.DisplayMask
        /// </summary>
        static string DISPLAYMASK_CLASSNAME = "com.microsoft.device.display.DisplayMask";

        private readonly AndroidJavaObject _displayMask;

        private DisplayMask(AndroidJavaObject displayMask)
        {
            _displayMask = displayMask;
        }

        /// <summary>
        /// Creates the display mask according to config_mainBuiltInDisplayMaskRect.
        /// </summary>
        public static DisplayMask FromResourcesRect()
        {
            var mask = OnPlayer.Run(p =>
            {
                var context = p.GetStatic<AndroidJavaObject>("currentActivity")
                    .Call<AndroidJavaObject>("getApplicationContext");

                using (var dm = new AndroidJavaClass(DISPLAYMASK_CLASSNAME))
                {
                    return dm.CallStatic<AndroidJavaObject>("fromResourcesRect", context);
                }
            });

            return new DisplayMask(mask);
        }
        /// <summary>
        /// Creates the display mask according to config_mainBuiltInDisplayMaskRectApproximation, which is the closest rectangle-base approximation of the mask.
        /// </summary>
        public static DisplayMask FromResourcesRectApproximation()
        {
            Debug.Log("DisplayMask.FromResourcesRectApproximation: ");

            var mask = OnPlayer.Run(p =>
            {
                var context = p.GetStatic<AndroidJavaObject>("currentActivity")
                    .Call<AndroidJavaObject>("getApplicationContext");

                using (var dm = new AndroidJavaClass(DISPLAYMASK_CLASSNAME))
                {
                    return dm.CallStatic<AndroidJavaObject>("fromResourcesRectApproximation", context);
                }
            });

            return new DisplayMask(mask);
        }
        /// <summary>
        /// Returns a list of Rects, each of which is the bounding rectangle for a non-functional area on the display.
        /// </summary>
        public RectInt[] GetBoundingRects()
        {
            var jrects = _displayMask.Call<AndroidJavaObject>("getBoundingRects");
            var size = jrects.Call<int>("size");

            Debug.Log("BoundingRects size: " + size);

            var rects = new RectInt[size];

            for (int i = 0; i < size; i++)
            {
                var jrect = jrects.Call<AndroidJavaObject>("get", i);

                var left = jrect.Get<int>("left");
                var top = jrect.Get<int>("top");
                var width = jrect.Call<int>("width");
                var height = jrect.Call<int>("height");

                rects[i] = new RectInt(xMin: left, yMin: top, width: width, height: height);
            }

            return rects;
        }

        public RectInt[] GetBoundingRectsForRotation(int rotation)
        {
            var jrects = _displayMask.Call<AndroidJavaObject>("getBoundingRectsForRotation", rotation);
            var size = jrects.Call<int>("size");

            Debug.Log("BoundingRects size: " + size);

            var rects = new RectInt[size];

            for (int i = 0; i < size; i++)
            {
                var jrect = jrects.Call<AndroidJavaObject>("get", i);

                var left = jrect.Get<int>("left");
                var top = jrect.Get<int>("top");
                var width = jrect.Call<int>("width");
                var height = jrect.Call<int>("height");

                rects[i] = new RectInt(xMin: left, yMin: top, width: width, height: height);
            }

            return rects;
        }
        /// <summary>
        /// Returns the bounding region of the mask
        /// </summary>
        /// <remarks>
        /// DisplayMask.getBounds returns an SKRegion which doesn't have a simple
        /// equivalent in C# (that I could think of). For Surface Duo the code just
        /// confirms it's a rectangle and calls getBounds() on the region.
        /// </remarks>
        public RectInt GetBoundsRegionBounds()
        {
            var jrects = _displayMask.Call<AndroidJavaObject>("getBounds");
            var isComplex = jrects.Call<bool>("isComplex"); // SKRegion
            var isEmpty = jrects.Call<bool>("isEmpty"); // SKRegion
            var isRect = jrects.Call<bool>("isRect"); // SKRegion

            Debug.LogWarning($"GetBounds isComplex:{isComplex} isEmpty:{isEmpty} isRect:{isRect}");

            if (isRect & !isEmpty & !isComplex)
            {
                var rect = jrects.Call<AndroidJavaObject>("getBounds");

                var left = rect.Get<int>("left");
                var top = rect.Get<int>("top");
                var width = rect.Call<int>("width");
                var height = rect.Call<int>("height");

                return new RectInt(xMin: left, yMin: top, width: width, height: height);
            }
            return new RectInt(0, 0, 0, 0);
        }

        /// <summary>
        /// Whether the app is spanned across both screens
        /// </summary>
        /// <remarks>
        /// https://docs.microsoft.com/en-us/dual-screen/android/sample-code/is-app-spanned?tabs=java
        /// </remarks>
        public static bool IsAppSpanned()
        {
            var isSpanned = OnPlayer.Run(p =>
            {
                var context = p.GetStatic<AndroidJavaObject>("currentActivity")
                    .Call<AndroidJavaObject>("getApplicationContext");
                using (var dm = new AndroidJavaClass(DISPLAYMASK_CLASSNAME))
                {
                    var displayMask = dm.CallStatic<AndroidJavaObject>("fromResourcesRectApproximation", context);
                    var region = displayMask.Call<AndroidJavaObject>("getBounds");
                    var boundings = displayMask.Call<AndroidJavaObject>("getBoundingRects");
                    var size = boundings.Call<int>("size");
                    if (size > 0)
                    {
                        var first = boundings.Call<AndroidJavaObject>("get", 0);
                        var rootView = p.GetStatic<AndroidJavaObject>("currentActivity")
                            .Call<AndroidJavaObject>("getWindow")
                            .Call<AndroidJavaObject>("getDecorView")
                            .Call<AndroidJavaObject>("getRootView");
                        var drawingRect = new AndroidJavaObject("android.graphics.Rect");
                        rootView.Call("getDrawingRect", drawingRect);
                        if (first.Call<bool>("intersect", drawingRect))
                        {
                            Debug.LogWarning($"Dual screen - intersect");
                            return true;
                        }
                        else
                        {
                            Debug.LogWarning($"Single screen - not intersect");
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            });
            return isSpanned;
        }

        public void Dispose()
        {
            _displayMask?.Dispose();
        }
    }


    /// <summary>
    /// Sensor reading class - requires JAR file from duo-android-plugin project
    /// </summary>
    public class HingeSensor : IDisposable
    {
        AndroidJavaObject plugin;

        /// <summary>
        /// Only get an object when the plugin is created
        /// </summary>
        HingeSensor(AndroidJavaObject sensorPlugin)
        {
            plugin = sensorPlugin;
        }
        /// <summary>
        /// Create an object to read the hinge sensor
        /// </summary>
        public static HingeSensor Start()
        {
#if UNITY_ANDROID
            var sensor = OnPlayer.Run(p =>
            {
                var context = p.GetStatic<AndroidJavaObject>("currentActivity");

                var plugin = new AndroidJavaClass("com.microsoft.device.dualscreen.unity.HingeAngleSensor")
                        .CallStatic<AndroidJavaObject>("getInstance", context);

                if (plugin != null)
                {
                    plugin.Call("setupSensor");
                    return new HingeSensor(plugin);
                }
                else
                {
                    return null;
                }
            });

            return sensor;
#else   
            return null;
#endif
        }

        /// <summary>
        /// Get the angle between the two screens
        /// </summary>
        /// <returns>0 to 360 (closed to fully opened), or -1 if error</returns>
        public float GetHingeAngle()
        {
#if UNITY_ANDROID
            if (plugin != null)
            {
                //float[] valueArray = plugin.Call<float[]>("getSensorValues", sensorName);
                float angle = plugin.Call<float>("getHingeAngle");
                return angle;
            }
            else return -1;
#else
            return -1;
#endif
        }

        public void StopSensing()
        {
            if (plugin != null)
            {
                plugin.Call("dispose");
                plugin = null;
            }
        }
        public void Dispose()
        {
            if (plugin != null)
            {
                plugin.Call("dispose");
                plugin = null;
            }
        }


    }

    /// <summary>
    /// Helper class to access Android from Unity
    /// </summary>
    internal class OnPlayer
    {
        public static T Run<T>(Func<AndroidJavaClass, T> runner)
        {
            if (runner == null)
                throw new ArgumentNullException(nameof(runner));

            using (var player = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                return runner(player);
            }
        }
    }
}
