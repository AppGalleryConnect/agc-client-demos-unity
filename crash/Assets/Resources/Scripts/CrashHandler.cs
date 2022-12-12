using System;
using System.IO;
using Huawei.Agconnect;
using Huawei.Agconnect.Auth;
using Huawei.Agconnect.Core.Service.Auth;
using Huawei.Agconnect.Crash;
using Huawei.Agconnect.Crash.Internal.Bean;
using Huawei.Agconnect.Crash.Internal.Util;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

public class CrashHandler : MonoBehaviour
{
    [SerializeField] private Text key, value;

    // Start is called before the first frame update
    void Start()
    {
        Stream inputStream = new MemoryStream(Resources.Load<TextAsset>("agconnect-services").bytes);
        AGConnectInstance.Initialize(new AGConnectOptionsBuilder()
            .SetInputStream(inputStream).SetPersistPath(new DirectoryInfo(Application.persistentDataPath)));
        AGConnectCrash.Instance.InitSystemInfo(new SystemInfoProperties());
        Application.logMessageReceived += HandleLog;
        Debug.Log("Application Started");
    }


    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Exception)
        {
            var ex = new AGConnectCrashException(logString, stackTrace);
            AGConnectCrash.Instance.RecordCrash(ex);
        }
        else
        {
            int logtype;
            switch (type)
            {
                case LogType.Warning:
                    logtype = LogLevel.Warn;
                    break;
                case LogType.Assert:
                    logtype = LogLevel.Assert;
                    break;
                case LogType.Error:
                    logtype = LogLevel.Error;
                    break;
                case LogType.Log:
                    logtype = LogLevel.Debug;
                    break;
                default:
                    logtype = LogLevel.Verbose;
                    break;
            }

            AGConnectCrash.Instance.Log(logtype, logString);
        }
    }

    public async void SignIn()
    {
        var authinstance = AGConnectAuth.Instance;
        
        if (authinstance.GetCurrentUser()==null)
        {
            var signInTask = authinstance.SignInAnonymouslyAsync();
            await signInTask;
            if (signInTask.IsCompleted && !signInTask.IsFaulted)
            {
                AGConnectCrash.Instance.SetUserId(AGConnectAuth.Instance.GetCurrentUser().GetUid());
            }
        }
        else
        {
            AGConnectCrash.Instance.SetUserId(authinstance.GetCurrentUser().GetUid());
        }
        AGConnectCrash.Instance.RecordException(new NullReferenceException("record exception"));
    }

    public void AddCustomKey()
    {
        AGConnectCrash.Instance.SetCustomKey(key.text, value.text);
    }
    
    public void RecordException()
    {
        AGConnectCrash.Instance.RecordException(new NullReferenceException("record exception"));
    }

    public void TestIt()
    {
        AGConnectCrash.Instance.TestIt();
    }

    public class SystemInfoProperties : ISystemInfo
    {
#if UNITY_ANDROID
        public string manufacturer{
            get
            {
                AndroidJavaClass ajc = new AndroidJavaClass("android.os.Build");

                return ajc.GetStatic<string>("MANUFACTURER");
            }

        }
        
#elif UNITY_IPHONE
            public string manufacturer => "Apple";
#else
            public string manufacturer => SystemInfo.deviceModel;
#endif
        public string app_ver => Application.version;

        public double disk_space => (double)SimpleDiskUtils.DiskUtils.CheckAvailableSpace()/1024;
        

        public double memory => (double)Profiler.GetTotalUnusedReservedMemoryLong()/(1024*1024);

        public string model => SystemInfo.deviceModel;

        public NetworkType network => Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork ? NetworkType.TypeMobile : NetworkType.TypeWifi ;

        public string os => SystemInfo.operatingSystem.Substring(0,SystemInfo.operatingSystem.IndexOf(" "));

        public string os_ver => SystemInfo.operatingSystem;
        
        public string screen_direction => Screen.orientation == ScreenOrientation.Portrait ? "1" : "0";

        public string screen_height => Screen.height.ToString();

        public string screen_width => Screen.width.ToString();
        
    }

   
}