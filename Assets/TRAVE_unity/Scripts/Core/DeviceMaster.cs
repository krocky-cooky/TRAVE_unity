using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TRAVE;



namespace TRAVE_unity
{
    /// <summary>
    /// This script is attatched to TRAVEDevice object.
    /// Updating instance of TRAVEDevice.
    /// <see cref="TRAVEDevice"/>
    /// </summary>
    public class DeviceMaster : MonoBehaviour 
    {
        private TRAVEDevice _trave = TRAVEDevice.GetDevice();
        private TRAVELogger _logger = TRAVELogger.GetInstance;


        void Awake()
        {
            SettingParams settingParams = GetComponent<SettingParams>();
           _trave._masterMethod_AllocateParams(settingParams);
           _logger.printMessage = settingParams.printMessage;
           _trave._masterMethod_Awake();

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