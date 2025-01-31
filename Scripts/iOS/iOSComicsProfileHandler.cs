using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;

namespace TreasureIslandPlugin.iOS
{
    public class ComicsProfileHandler
    {
        [DllImport("__Internal")]
        private static extern void SetProfile(long instanceId, string paramData, System.Action<long, bool, string> callback);
        
        private static readonly Dictionary<long, Action<ComicsModel.Completion>> _callbacks = new();
        
        public static void SetProfile(ComicsModel.ProfileModel model, Action<ComicsModel.Completion> completionHandler)
        {
            long instanceId = DateTime.UtcNow.Ticks;
            _callbacks[instanceId] = completionHandler;
            // Call extern "C"
            SetProfile(instanceId, model.ToDataString(), OnCompletionHandler);
        }

        [MonoPInvokeCallback(typeof(Action<long, bool, string>))]
        public static void OnCompletionHandler(long instanceId, bool success, string message)
        {
            if (_callbacks.TryGetValue(instanceId, out var callback))
            {
                ComicsModel.Completion complete = new(
                    callback: "SetProfile",
                    success: success,
                    message: message
                );
                callback.Invoke(complete);
                _callbacks.Remove(instanceId);
            }
        }
    }
}