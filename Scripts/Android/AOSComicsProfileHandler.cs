using System;
using System.Text;
using UnityEngine;

namespace TreasureIslandPlugin.Android
{
    public class ComicsProfileHandler
    {
        public ComicsProfileHandler(ComicsModel.ProfileModel model)
        {
            _model = model;
        }

        private readonly ComicsModel.ProfileModel _model;

        public void Launch(Action<ComicsModel.Completion> completionHandler)
        {
            using AndroidJavaClass javaClass = new("kr.co.studioguru.sdk.treasureisland.bridge.Bridge");
            BridgeCallback callback = new(bridge: this, completionHandler: completionHandler);
            javaClass.CallStatic("profile", _model.ToDataString(), callback);
        }

        // Android BridgeCallback
        private class BridgeCallback : AndroidJavaProxy
        {
            private readonly ComicsProfileHandler _bridge;
            private readonly Action<ComicsModel.Completion> _completionHandler;

            public BridgeCallback(
                ComicsProfileHandler bridge, 
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