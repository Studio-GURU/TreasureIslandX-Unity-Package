using System;
using System.Text;
using UnityEngine;
using System.Runtime.InteropServices;

namespace TreasureIslandPlugin.Android
{
    public class ComicsInitHadler
    {
        public ComicsInitHadler(ComicsModel.InitModel model)
        {
            _model = model;
        }

        private readonly ComicsModel.InitModel _model;

        public void Initialize(Action<ComicsModel.Completion> completionHandler)
        {
            RunAndroid(completionHandler: completionHandler);
        }

        private void RunAndroid(Action<ComicsModel.Completion> completionHandler) 
        {
            using AndroidJavaClass javaClass = new("kr.co.studioguru.sdk.treasureisland.bridge.Bridge");
            using AndroidJavaClass unityPlayer = new("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            BridgeCallback callback = new(bridge: this, completionHandler: completionHandler);
            javaClass.CallStatic("initialize", currentActivity, _model.ToDataString(), callback);
        }

        // Android BridgeCallback
        private class BridgeCallback : AndroidJavaProxy
        {
            private readonly ComicsInitHadler _bridge;
            private readonly Action<ComicsModel.Completion> _completionHandler;

            public BridgeCallback(
                ComicsInitHadler bridge, 
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