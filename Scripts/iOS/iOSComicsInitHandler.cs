using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;

namespace TreasureIslandPlugin.iOS
{
    public class ComicsInitHandler
    {   
        [DllImport("__Internal")]
        private static extern void Initialize(long instanceId, string paramData, System.Action<long, bool, string> callback);
        
        
        private static readonly Dictionary<long, Action<ComicsModel.Completion>> _callbacks = new();
        
        public static void Initializer(ComicsModel.InitModel model, Action<ComicsModel.Completion> completionHandler)
        {
            long instanceId = DateTime.UtcNow.Ticks;
            _callbacks[instanceId] = completionHandler;
            // Call extern "C"
            Initialize(instanceId, model.ToDataString(), OnCompletionHandler);
        }

        [MonoPInvokeCallback(typeof(Action<long, bool, string>))]
        public static void OnCompletionHandler(long instanceId, bool success, string message)
        {
            if (_callbacks.TryGetValue(instanceId, out var callback))
            {
                ComicsModel.Completion complete = new(
                    callback: "Initialize",
                    success: success,
                    message: message
                );
                callback.Invoke(complete);
                _callbacks.Remove(instanceId);
            }
        }
    }
}
