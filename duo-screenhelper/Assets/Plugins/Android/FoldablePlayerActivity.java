// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
package com.microsoft.device.dualscreen.unity;

import com.unity3d.player.UnityPlayerActivity;

import androidx.window.java.layout.WindowInfoRepositoryCallbackAdapter;
import androidx.window.layout.DisplayFeature;
import androidx.window.layout.FoldingFeature;
import androidx.window.layout.WindowInfoRepository;
import androidx.window.layout.WindowLayoutInfo;

import android.os.Handler;
import android.os.Looper;
import android.util.Log;

import androidx.appcompat.app.AppCompatActivity;
import androidx.core.util.Consumer;

import java.util.concurrent.Executor;
import android.os.Bundle;
import android.util.Log;

public class FoldablePlayerActivity extends UnityPlayerActivity {

    WindowLayoutInfo lastLayoutInfo = null;
    FoldingFeature lastFoldingFeature = null;

    String TAG = "JWM";
    WindowInfoRepositoryCallbackAdapter wir;

    protected void onCreate(Bundle savedInstanceState) {
        // call UnityPlayerActivity.onCreate()
        super.onCreate(savedInstanceState);
        // print debug message to logcat
        Log.d(TAG, "onCreate called!");
        wir = new WindowInfoRepositoryCallbackAdapter(
                WindowInfoRepository.Companion.getOrCreate(
                        this
                )
        );
        Log.d(TAG, "WindowManager created");
    }
    @Override
    protected void onStart() {
        super.onStart();
        wir.addWindowLayoutInfoListener(runOnUiThreadExecutor(), (newLayoutInfo -> {
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
        }));
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
    public void onBackPressed()
    {
        // instead of calling UnityPlayerActivity.onBackPressed() we just ignore the back button event
        // super.onBackPressed();
    }
}
