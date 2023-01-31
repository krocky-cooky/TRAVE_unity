using TRAVE_unity;
using UnityEngine;
using System;

namespace TRAVE 
{
    public class TRAVEForceGauge : TRAVEBase<TRAVEForceGauge> 
    {
        private CommunicationType _communicationType;
        private CommunicationBase _communicationBase;

        public TRAVEReceivingFormat currentProfile{ get;set; } = new TRAVEReceivingFormat();

        public bool isConnected
        {
            get 
            {
                if(_communicationBase == null) return false;
                return _communicationBase.isConnected;
            }
        }

        public float force 
        {
            get 
            {
                return currentProfile.force;
            }
        }

        public override void _masterMethod_AllocateParams(SettingParams settingParams)
        {
            // allocation of parameters
            _communicationType = settingParams.deviceCommunicationType;
            TrainingDeviceType type = TrainingDeviceType.ForceGauge;
            switch(_communicationType)
            {
                case CommunicationType.Serial:
                    _communicationBase = new Serial(type);
                    break;
                case CommunicationType.WebSockets:
                    _communicationBase = new WebSockets(type);
                    break;
                case CommunicationType.Bluetooth:
                    _communicationBase = new Bluetooth(type);
                    break;
            }
            _communicationBase.AllocateParams(settingParams);
        }

        public override void _masterMethod_Awake()
        {
            _communicationBase.Awake();
        }

        public override void _masterMethod_Start()
        {
            _communicationBase.Start();
            
        }

        public override void _masterMethod_Update()
        {
            _communicationBase.Update();
            currentProfile = GetReceivedData();
        }

        public override void _masterMethod_OnApplicationQuit()
        {
            _communicationBase.OnApplicationQuit();
        }

        public bool ReConnectToDevice()
        {
            _communicationBase.Connect();
            return _communicationBase.isConnected;
        }

        public TRAVEReceivingFormat GetReceivedData()
        {
            string receivedString = _communicationBase.GetReceivedString();
            try
            {
                TRAVEReceivingFormat retval = JsonUtility.FromJson<TRAVEReceivingFormat>(receivedString);
                if(retval == null)
                {
                    return new TRAVEReceivingFormat();
                }
                return retval;
            }
            catch(System.Exception e)
            {
                string message = e.Message;
                _logger.writeLog(receivedString, TRAVELogger.LogLevel.Warn);
                return new TRAVEReceivingFormat();
            }
        }
    }
}