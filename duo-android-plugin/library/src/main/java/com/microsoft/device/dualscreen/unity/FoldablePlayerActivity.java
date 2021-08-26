// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
package com.microsoft.device.dualscreen.unity;

//import com.unity3d.player.UnityPlayerActivity;

import androidx.window.FoldingFeature;
import androidx.window.WindowLayoutInfo;
import androidx.window.WindowManager;

import android.os.Handler;
import android.os.Looper;
import android.util.Log;

import androidx.appcompat.app.AppCompatActivity;
import androidx.core.util.Consumer;

import java.util.concurrent.Executor;
import android.os.Bundle;
import android.util.Log;

public class FoldablePlayerActivity extends AppCompatActivity { //UnityPlayerActivity {

    WindowLayoutInfo lastLayoutInfo = null;
    FoldingFeature lastFoldingFeature = null;

    String TAG = "JWM";
    WindowManager wm;
    StateContainer stateContainer = new StateContainer();

    protected void onCreate(Bundle savedInstanceState) {
        // call UnityPlayerActivity.onCreate()
        super.onCreate(savedInstanceState);
        // print debug message to logcat
        Log.d(TAG, "onCreate called!");
        wm = new WindowManager(this);
        Log.d(TAG, "WindowManager created");
    }
    @Override
    protected void onStart() {
        super.onStart();
        wm.registerLayoutChangeCallback(runOnUiThreadExecutor(), stateContainer);
    }

    @Override
    protected void onStop() {
        super.onStop();
        wm.unregisterLayoutChangeCallback(stateContainer);
    }

    class StateContainer implements Consumer<WindowLayoutInfo>
    {
        //WindowLayoutInfo lastLayoutInfo = null;
        @Override
        public void accept(WindowLayoutInfo newLayoutInfo) {
            lastLayoutInfo = newLayoutInfo;
            Log.d(TAG, "Feature " + newLayoutInfo.getDisplayFeatures().toString() );

            lastFoldingFeature = null; // race condition?
            if (newLayoutInfo.getDisplayFeatures().size() > 0)
            {
                newLayoutInfo.getDisplayFeatures().forEach(displayFeature -> {
                    FoldingFeature foldingFeature = (FoldingFeature)displayFeature;
                    if (foldingFeature != null)
                    {   // only set if it's a fold, not other feature type. only works for single-fold devices.
                        lastFoldingFeature = foldingFeature;
                    }
                });
            }
        }
    }

    Executor runOnUiThreadExecutor()
    {
        return new MyExecutor();
    }
    class MyExecutor implements Executor
    {
        Handler handler = new Handler(Looper.getMainLooper());
        @Override
        public void execute(Runnable command) {
            handler.post(command);
        }
    }

    public FoldingFeature getFoldingFeature()
    {
        return lastFoldingFeature;
    }

    @Override
    public void onBackPressed()
    {
        // instead of calling UnityPlayerActivity.onBackPressed() we just ignore the back button event
        // super.onBackPressed();
    }
}
