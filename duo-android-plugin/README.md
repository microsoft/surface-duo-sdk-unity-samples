---
page_type: sample
languages:
- csharp
description: "Android Studio project for the helper JAR file used by the ScreenHelper sample"
urlFragment: "duo-android-plugin"
---

# Android dual-screen SDK plugin for Unity

Exposes dual-screen APIs for the Surface Duo to Unity game developers.

## Prerequisites

- Android Studio

## Build

Run gradle task `exportJar` (in **Tasks/other**)

## Result

Emits **SurfaceDuoHingeSensorPlugin.jar** in the **/library/release/** directory.

The functionality is a lightweight wrapper over the hinge angle sensor on the Surface Duo, to be used in the Unity projects in this repo.
Drop the JAR file in your **Assets/Plugins/Android** to use (alongside the **mainTemplate.gradle** file).
