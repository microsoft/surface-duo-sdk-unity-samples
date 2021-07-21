// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
package com.microsoft.device.dualscreen.unity;

import com.unity3d.player.UnityPlayerActivity;

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

public class FoldablePlayerActivity extends UnityPlayerActivity {
/*
    WindowManager wm;

    protected void onCreate(Bundle savedInstanceState) {
        // call UnityPlayerActivity.onCreate()
        super.onCreate(savedInstanceState);
        // print debug message to logcat
        Log.d("FoldablePlayerActivity", "onCreate called!");
        wm = new WindowManager(this);
        Log.d("FoldablePlayerActivity", "WindowManager created");
    }
    
    public void onBackPressed()
    {
        // instead of calling UnityPlayerActivity.onBackPressed() we just ignore the back button event
        // super.onBackPressed();
    }
*/

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
        WindowLayoutInfo lastLayoutInfo = null;
        @Override
        public void accept(WindowLayoutInfo newLayoutInfo) {
            lastLayoutInfo = newLayoutInfo;
            Log.d(TAG, "Feature " + newLayoutInfo.getDisplayFeatures().toString() );
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

    public void onBackPressed()
    {
        // instead of calling UnityPlayerActivity.onBackPressed() we just ignore the back button event
        // super.onBackPressed();
    }
}
