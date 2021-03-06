---
page_type: sample
languages:
- csharp
description: "Adapt or build dual-screen games for the Surface Duo using Unity for Android"
urlFragment: "duo-screenhelper"
---

# Surface Duo ScreenHelper for Unity

Exposes dual-screen APIs for the Surface Duo to Unity game developers. You can access the screen size, the size of the mask where the device hinge is, and the hinge angle as the device is manipulated. This sample just displays these values on the screen, but they are ready for use in your Unity games.

## Prerequisites

- Unity 2019.4.18f1
- Project settings for Android
- Surface Duo device

_^ Unity 2019.3 and later will [not deploy to x86-based Android emulators](https://blogs.unity3d.com/2019/03/05/android-support-update-64-bit-and-app-bundles-backported-to-2017-4-lts/) so a Surface Duo device is required for testing. Use an older commit of this repo that targets Unity 2018 for Surface Duo emulator support._

### Hinge angle sensor

Accessing the sensor requires an additional Android package. Build the [duo-android-plugin](/microsoft/surface-duo-sdk-unity-samples/tree/master/duo-android-plugin) library project in this repo, and copy the **SurfaceDuoHingeSensorPlugin.jar** file from **/library/release/** into the Unity folder **/Assets/Plugins/Android/**.

## Result

![Unity in Surface Duo emulator](Screenshots/06-unity-device-small.png)

## Next steps

Follow the steps from [this blog post](https://devblogs.microsoft.com/surface-duo/dual-screen-games-with-unity-for-android) to implement a custom gradle file in your Unity project and add the C# script that will let you access the device APIs in your game!
