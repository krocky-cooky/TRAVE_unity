using TRAVE_unity;
using UnityEngine;
using System;

namespace TRAVE 
{
    /// <summary>
    /// Operation interface for TRAVE force gauge.
    /// </summary>
    public class TRAVEForceGauge : TRAVEBase<TRAVEForceGauge> 
    {
        /// <summary>
        /// Which communication method to use.
        /// <see cref"CommunicationType" />
        /// </summary>
        private CommunicationType _communicationType;

        /// <summary>
        /// Instances corresponding to communication methods.
        /// <see cref="CommunicationBase" />
        /// </summary>
        private CommunicationBase _communicationBase;

        /// <summary>
        /// The latest data obtained from motor.
        /// </summary>
        public TRAVEReceivingFormat currentProfile{ get;set; } = new TRAVEReceivingFormat();

        /// <summary>
        /// Whether or not the connection is made.
        /// </summary>
        public bool isConnected
        {
            get 
            {
                if(_communicationBase == null) return false;
                return _communicationBase.isConnected;
            }
        }

        /// <summary>
        /// Current force value of force gauge.
        /// </summary>
        public float force 
        {
            get 
            {
                return currentProfile.force;
            }
        }

        /// <summary>
        /// Override of parameter asaigning method.
        /// <see cref="TRAVEBase"/>
        /// </summary>
        /// <param name="settingParams"><see cref="SettingParams"/></param>
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

        /// <summary>
        /// Override of 'Awake' method.
        /// <see cref="TRAVEBase"/>
        /// </summary>
        public override void _masterMethod_Awake()
        {
            _communicationBase.Awake();
        }

        /// <summary>
        /// Override of 'Start' method.
        /// <see cref="TRAVEBase"/>
        /// </summary>
        public override void _masterMethod_Start()
        {
            _communicationBase.Start();
            
        }

        /// <summary>
        /// Override of 'Update' method.
        /// <see cref="TRAVEBase"/>
        /// </summary>
        public override void _masterMethod_Update()
        {
            _communicationBase.Update();
            currentProfile = GetReceivedData();
        }

        /// <summary>
        /// Override of 'OnApplicationQuit' method.
        /// <see cref="TRAVEBase"/>
        /// </summary>
        public override void _masterMethod_OnApplicationQuit()
        {
            _communicationBase.OnApplicationQuit();
        }

        /// <summary>
        /// Try establishing connection again.
        /// </summary>
        /// <returns>Whether or not connection is made.</returns>
        public bool ReConnectToDevice()
        {
            _communicationBase.Connect();
            return _communicationBase.isConnected;
        }

        /// <summary>
        /// Retrieve received data in TRAVEReceivingFormat. <see cref="TRAVEReceivingFormat" />
        /// </summary>
        /// <returns>Received value.</returns>
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
                _logger.WriteLog(receivedString, TRAVELogger.LogLevel.Warn);
                return new TRAVEReceivingFormat();
            }
        }
    }
}