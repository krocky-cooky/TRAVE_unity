using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TRAVE;



namespace TRAVE_unity
{
    public class DeviceMaster : MonoBehaviour 
    {
        private TRAVEDevice _trave = TRAVEDevice.GetDevice();


        void Awake()
        {
            SettingParams settingParams = GetComponent<SettingParams>();
           _trave._masterMethod_AllocateParams(settingParams);
        }


        void Start()
        {
            _trave._masterMethod_Start();
            
        }

        void Update()
        {
            _trave._masterMethod_Update();
        }

         
    }
}