using System.Reflection;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

namespace TreasureIslandPlugin.iOS
{
    public class ComicsModel {

        public enum Membership {
            Basic = 1,
            Channeling = 2
        }

        public enum Environment {
            Live = 1,
            Develop = 2,
            Test = 3
        }

        public enum Gender {
            Male = 1,
            Female = 2
        }

        [System.Serializable]
        public class Completion {
            public string callback;
            public bool success;
            public string message;

            public Completion(string callback, bool success, string message)
            {
                this.callback = callback;
                this.success = success;
                this.message = message;
            }
        }   

        public class InitModel
        {
            public string appId;
            public string appSecret;
            /**
            1: basic
            2: channleing
            **/
            public Membership membership;
            public bool allowLog;
            /**
            1: live
            2: test
            3: develop
            **/
            public Environment environment;
            public NotificationOptionModel notificationOption;
            public StatusbarOptionModel statusbarOption;

            public InitModel(
                string appId,
                string appSecret,
                Membership membership,
                bool allowLog,
                Environment environment,
                NotificationOptionModel notificationOption,
                StatusbarOptionModel statusbarOption
            )
            {
                this.appId = appId;
                this.appSecret = appSecret;
                this.membership = membership;
                this.allowLog = allowLog;
                this.environment = environment;
                this.notificationOption = notificationOption;
                this.statusbarOption = statusbarOption;
            }

            public string ToDataString() {
                StringBuilder builder = new();
                builder.Append("{");
                if (notificationOption != null && notificationOption.channelName.Length > 0 && notificationOption.notificationIconName.Length > 0) {
                    builder.Append("\"notificationOption\":{ ");
                    builder.Append("\"channelName\":\"" + notificationOption.channelName + "\", ");
                    builder.Append("\"notificationIconName\":\"" + notificationOption.notificationIconName + "\" ");
                    builder.Append("},");
                }
                if (statusbarOption != null && statusbarOption.statusbarColor.Length > 0) {
                    builder.Append("\"statusbarOption\":{ ");
                    builder.Append("\"statusbarColor\":\"" + statusbarOption.statusbarColor + "\", ");
                    builder.Append("\"isWindwoLight\":\"" + statusbarOption.isWindwoLight + "\" ");
                    builder.Append("},");
                }
                builder.Append("\"appId\":\"" + appId + "\", ");
                builder.Append("\"appSecret\":\"" + appSecret + "\", ");
                builder.Append("\"membership\":\"" + (int)membership + "\", ");
                builder.Append("\"allowLog\":\"" + allowLog + "\", ");
                builder.Append("\"environment\":\"" + (int)environment + "\" ");
                builder.Append("}");
                Debug.Log(builder.ToString());
                return builder.ToString();
            }
        }

        public class NotificationOptionModel {
            public string channelName;
            public string notificationIconName;
            public NotificationOptionModel(string channelName, string notificationIconName)
            {
                this.channelName = channelName;
                this.notificationIconName = notificationIconName;
            }
        }

        public class StatusbarOptionModel {
            public string statusbarColor;
            public bool isWindwoLight = false;
            public StatusbarOptionModel(string statusbarColor, bool isWindwoLight) {
                this.statusbarColor = statusbarColor;
                this.isWindwoLight = isWindwoLight;
            }
        }

        public class ProfileModel {
            public string signKey;
            public Gender? gender;
            public int birthYear;
            public ProfileModel(string signKey, Gender? gender = null, int birthYear = 0) 
            {
                this.signKey = signKey;
                this.gender = gender;
                this.birthYear = birthYear;
            }

            public string ToDataString() 
            {
                StringBuilder builder = new();
                builder.Append("{");
                builder.Append("\"signKey\":\"" + signKey + "\", ");
                if (gender != null)
                {
                    builder.Append("\"gender\":\"" + (int)gender + "\", ");
                }
                builder.Append("\"birthYear\":\"" + birthYear + "\" ");
                builder.Append("}");
                return builder.ToString();
            }
        }

        public class LaunchModel{
            public string advertisingId {get; private set;}
            public bool allowHeader {get; private set;}
            public string headerTitle {get; private set;}
            public bool allowBackButton {get; private set;}
            public bool allowCloseButton {get; private set;}
            public LaunchModel(
                string advertisingId, 
                bool allowHeader, 
                string headerTitle, 
                bool allowBackButton, 
                bool allowCloseButton
            ) {
                this.advertisingId = advertisingId;
                this.allowHeader = allowHeader;
                this.headerTitle = headerTitle;
                this.allowBackButton = allowBackButton;
                this.allowCloseButton = allowCloseButton;
            }

            public string ToDataString() 
            {
                StringBuilder builder = new();
                builder.Append("{");
                builder.Append("\"advertisingId\":\"" + advertisingId + "\", ");
                builder.Append("\"allowHeader\":\"" + allowHeader + "\", ");
                builder.Append("\"headerTitle\":\"" + headerTitle + "\", ");
                builder.Append("\"allowBackButton\":\"" + allowBackButton + "\", ");
                builder.Append("\"allowCloseButton\":\"" + allowCloseButton + "\" ");
                builder.Append("}");
                return builder.ToString();
            }
        }
    }
}