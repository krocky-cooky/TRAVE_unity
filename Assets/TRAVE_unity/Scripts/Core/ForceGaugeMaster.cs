using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TRAVE;



namespace TRAVE_unity
{
    public class ForceGaugeMaster : MonoBehaviour 
    {
        private TRAVEForceGauge _forceGauge = TRAVEForceGauge.GetDevice();
        private TRAVELogger _logger = TRAVELogger.GetInstance;


        void Awake()
        {
            SettingParams settingParams = GetComponent<SettingParams>();
           _forceGauge._masterMethod_AllocateParams(settingParams);
           _logger.printMessage = settingParams.printMessage;
           _forceGauge._masterMethod_Awake();
           Debug.Log(settingParams.printMessage);

        }


        // void Start()
        // {
        //     _trave._masterMethod_Start();  
        // }

        // void Update()
        // {
        //     _trave._masterMethod_Update();
        // }

        // void OnApplicationQuit()
        // {
        //     _trave._masterMethod_OnApplicationQuit();
        // }

         
    }
}