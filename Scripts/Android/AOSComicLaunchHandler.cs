using System;
using System.Text;
using UnityEngine;

namespace TreasureIslandPlugin.Android {    
    public class ComicLaunchHandler
    {
        public ComicLaunchHandler(ComicsModel.LaunchModel model)
        {
            _model = model;
        }

        private readonly ComicsModel.LaunchModel _model;

        public void Launch(Action<ComicsModel.Completion> completionHandler)
        {
            using AndroidJavaClass javaClass = new("kr.co.studioguru.sdk.treasureisland.bridge.Bridge");
            using AndroidJavaClass unityPlayer = new("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            BridgeCallback callback = new(bridge: this, completionHandler: completionHandler);
            javaClass.CallStatic("launch", currentActivity, _model.ToDataString(), callback);
        }

        // Android BridgeCallback
        private class BridgeCallback : AndroidJavaProxy
        {
            private readonly ComicLaunchHandler _bridge;
            private readonly Action<ComicsModel.Completion> _completionHandler;


            public BridgeCallback(
                ComicLaunchHandler bridge, 
                Action<ComicsModel.Completion> completionHandler
            ) : base("kr.co.studioguru.sdk.treasureisland.bridge.Bridge$BridgeCallback")
            {
                _bridge = bridge;
                _completionHandler = completionHandler;
            }

            // Android Interface
            public void onComplete(string result)
            {
                ComicsModel.Completion complete = JsonUtility.FromJson<ComicsModel.Completion>(result);            
                _completionHandler.Invoke(complete);
            }
        }
    }
}
