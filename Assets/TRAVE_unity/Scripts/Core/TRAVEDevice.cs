using TRAVE_unity;
using UnityEngine;
using System;

namespace TRAVE
{
    public class TRAVEDevice
    {
        private static TRAVEDevice _device = new TRAVEDevice();

        private double MIN_SENDING_INTERVAL = 200.0;
        private string MOTOR_COMMAND_PREFIX = "m";
        private string COVNVERTER_COMMAND_PREFIX = "p";

        private CommunicationType _communicationType;
        private CommunicationBase _communicationBase;
        private TRAVESendingFormat _currentMotorState = new TRAVESendingFormat();
        private TRAVESendingFormat _dataToSend = new TRAVESendingFormat();
        private DateTime _timeOfPreviousSend;
        private float _maxTorque;
        private float _maxSpeed;
    
        private TRAVELogger _logger = TRAVELogger.GetInstance;

        public TRAVEReceivingFormat currentProfile{ get;set; } = new TRAVEReceivingFormat();

        public bool isConnected
        {
            get
            {
                return _communicationBase.isConnected;
            }
        }

        public string motorMode
        {
            get
            {
                return currentProfile.target == "trq" ? "Torque Mode" : "Speed Mode";
            }   
        }

        public float torque
        {
            get
            {
                return currentProfile.trq;
            }
        }

        public float speed
        {
            get
            {
                return currentProfile.spd;
            }
        }

        public float position 
        {
            get 
            {
                return currentProfile.pos;
            }
        }

        public float integrationAngle 
        {
            get 
            {
                return currentProfile.integrationAngle;
            }
        }



        private TRAVEDevice()
        {  
        }

        private bool CheckSendingInterval()
        {
            int comparison = DateTime.Now.CompareTo(_timeOfPreviousSend.AddMilliseconds(MIN_SENDING_INTERVAL));
            return comparison > 0;
        }

        public static TRAVEDevice GetDevice()
        {
            return _device;
        }

        internal void _masterMethod_AllocateParams(SettingParams settingParams)
        {
            // allocation of parameters
            _communicationType = settingParams.communicationType;
            switch(_communicationType)
            {
                case CommunicationType.Serial:
                    _communicationBase = new Serial();
                    break;
                case CommunicationType.WebSockets:
                    _communicationBase = new WebSockets();
                    break;
                case CommunicationType.Bluetooth:
                    _communicationBase = new Bluetooth();
                    break;
            }
            _maxTorque = settingParams.maxTorque;
            _maxSpeed = settingParams.maxSpeed;
            _communicationBase.AllocateParams(settingParams);
        }

        internal void _masterMethod_Start()
        {
            _communicationBase.Start();
            _timeOfPreviousSend = DateTime.Now;
            
        }

        internal void _masterMethod_Update()
        {
            _communicationBase.Update();
            currentProfile = GetReceivedData();
        }


        internal void _masterMethod_OnApplicationQuit()
        {
            _communicationBase.OnApplicationQuit();
        }

        public bool ReConnectToDevice()
        {
            _communicationBase.Connect();
            return _communicationBase.isConnected;
        }
        

        public void SetTorqueMode(float torque, float spdLimit = 10.0f, float spdLimitLiftup = 10.0f)
        {
            _dataToSend.target = "trq";
            if(torque > _maxTorque)
            {
                _logger.writeLog("Input torque limit exceeded.", TRAVELogger.LogLevel.Warn);
                torque = _maxTorque;
            }
            _dataToSend.trq = torque;
            _dataToSend.spdLimit = spdLimit;
            _dataToSend.spdLimitLiftup = spdLimitLiftup;
        }

        //<sammary> 速度指令モードに変更し、スピード値をセットする </sammary>
        public void SetSpeedMode(float speed, float trqLimit = 6.0f)
        {
            _dataToSend.target = "spd";
            if(speed > _maxSpeed)
            {
                _logger.writeLog("Input speed limit exceeded.", TRAVELogger.LogLevel.Warn);
                speed = _maxSpeed;
            }
            _dataToSend.spd = speed;
            _dataToSend.trqLimit = trqLimit;
        }

        public bool TurnOnMotor()
        {
            string command = MOTOR_COMMAND_PREFIX + "1";
            return _communicationBase.SendString(command);
        }

        public bool TurnOffMotor()
        {
            string command = MOTOR_COMMAND_PREFIX + "0";
            return _communicationBase.SendString(command);
        }

        public bool TurnOnConverter()
        {
            string command = COVNVERTER_COMMAND_PREFIX + "1";
            return _communicationBase.SendString(command);
        }

        public bool TurnOffConverter()
        {
            string command = COVNVERTER_COMMAND_PREFIX + "0";
            return _communicationBase.SendString(command);
        }

        public bool SendString(string command)
        {
            return _communicationBase.SendString(command);
        }


        public bool RestoreMotor()
        {
            SetSpeedMode(0.0f);
            return Apply();
        }

        //<sammary> モーターに変更を適用する </sammary>
        public bool Apply()
        {
            if(CheckSendingInterval())
            {
                _currentMotorState = _dataToSend;
                if(_communicationBase.SendData(_dataToSend))
                {
                    _timeOfPreviousSend = DateTime.Now;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                _logger.writeLog("Transmission interval is too short.", TRAVELogger.LogLevel.Warn);
                return false;
            }
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