using System;
using System.Text;
using UnityEngine;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using AOT;

namespace TreasureIslandPlugin.Android 
{
    public class ComicsScript
    {
        public static void Initialize(ComicsModel.InitModel entity, Action<ComicsModel.Completion> completionHandler)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                ComicsInitHadler handler = new(model: entity);
                handler.Initialize(completionHandler: completionHandler);
                return;
            }
        }

        public static void SetProfile(ComicsModel.ProfileModel entity, Action<ComicsModel.Completion> completionHandler)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                ComicsProfileHandler handler = new(model: entity);
                handler.Launch(completionHandler: completionHandler);
                return;
            }
        }

        public static void Launch(ComicsModel.LaunchModel entity, Action<ComicsModel.Completion> completionHandler)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                ComicLaunchHandler handler = new(model: entity);
                handler.Launch(completionHandler: completionHandler);
                return;
            }           
        }
    }
}