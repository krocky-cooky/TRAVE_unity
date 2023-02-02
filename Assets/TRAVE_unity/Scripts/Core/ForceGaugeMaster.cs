using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TRAVE;



namespace TRAVE_unity
{
    /// <summary>
    /// This script is attatched to TRAVEForceGauge object.
    /// Updating instance of TRAVEForceGauge.
    /// <see cref="TRAVEForceGauge"/>
    /// </summary>
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

        }


        void Start()
        {
            _forceGauge._masterMethod_Start();  
        }

        void Update()
        {
            _forceGauge._masterMethod_Update();
        }

        void OnApplicationQuit()
        {
            _forceGauge._masterMethod_OnApplicationQuit();
        }

         
    }
}