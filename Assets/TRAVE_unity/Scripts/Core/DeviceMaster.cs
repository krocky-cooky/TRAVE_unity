using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TRAVE;



namespace TRAVE_unity
{
    public class DeviceMaster : MonoBehaviour 
    {
        private TRAVEDevice _trave = TRAVEDevice.GetDevice();
        private TRAVELogger _logger = TRAVELogger.GetInstance;


        void Awake()
        {
            SettingParams settingParams = GetComponent<SettingParams>();
           _trave._masterMethod_AllocateParams(settingParams);
           _logger.printMessage = settingParams.printMessage;

        }


        void Start()
        {
            _trave._masterMethod_Start();
            
        }

        void Update()
        {
            _trave._masterMethod_Update();
        }

        void OnApplicationQuit()
        {
            _trave._masterMethod_OnApplicationQuit();
        }

         
    }
}