using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;

namespace TreasureIslandPlugin.iOS
{
    public class ComicsLaunchHandler
    {
        [DllImport("__Internal")]
        private static extern void Launch(long instanceId, string paramData, System.Action<long, bool, string> callback);
        
        private static readonly Dictionary<long, Action<ComicsModel.Completion>> _callbacks = new();
        
        public static void Launch(ComicsModel.LaunchModel model, Action<ComicsModel.Completion> completionHandler)
        {
            long instanceId = DateTime.UtcNow.Ticks;
            _callbacks[instanceId] = completionHandler;
            // Call extern "C"
            Launch(instanceId, model.ToDataString(), OnCompletionHandler);
        }

        [MonoPInvokeCallback(typeof(Action<long, bool, string>))]
        public static void OnCompletionHandler(long instanceId, bool success, string message)
        {
            if (_callbacks.TryGetValue(instanceId, out var callback))
            {
                ComicsModel.Completion complete = new(
                    callback: "Launch",
                    success: success,
                    message: message
                );
                callback.Invoke(complete);
                _callbacks.Remove(instanceId);
            }
        }
    }
}
