using System;
using System.Text;
using UnityEngine;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using AOT;

namespace TreasureIslandPlugin.iOS
{
    public class ComicsScript
    {
        public static void Initialize(ComicsModel.InitModel entity, Action<ComicsModel.Completion> completionHandler)
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                ComicsInitHandler.Initializer(model: entity, completionHandler: completionHandler);
                return;
            }
        }

        public static void SetProfile(ComicsModel.ProfileModel entity, Action<ComicsModel.Completion> completionHandler)
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                ComicsProfileHandler.SetProfile(model: entity, completionHandler: completionHandler);
                return;
            }
        }

        public static void Launch(ComicsModel.LaunchModel entity, Action<ComicsModel.Completion> completionHandler)
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                ComicsLaunchHandler.Launch(model: entity, completionHandler: completionHandler);
                return;
            }
        }
    }
}