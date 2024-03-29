using TRAVE_unity;
using UnityEngine;
using System;

namespace TRAVE
{
    /// <summary>
    /// Operation interface for TRAVE training machine.
    /// </summary>
    public class TRAVEDevice : TRAVEBase<TRAVEDevice>
    {
        /// <summary>
        /// Minimum time interval for continuous transmission.
        /// </summary>
        private double MIN_SENDING_INTERVAL_MS = 200.0;

        /// <summary>
        /// Prefix required for commands to the converter.
        /// </summary>
        private string MOTOR_COMMAND_PREFIX = "m";

        /// <summary>
        /// Prefix required for commands to the motor.
        /// </summary>
        private string COVNVERTER_COMMAND_PREFIX = "p";

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
        private TRAVESendingFormat _currentMotorState = new TRAVESendingFormat();

        /// <summary>
        /// Storing the data to be sent.
        /// </summary>
        private TRAVESendingFormat _dataToBeSent = new TRAVESendingFormat();

        /// <summary>
        /// Time of last transmission.
        /// </summary>
        private DateTime _timeOfPreviousSend;

        /// <summary>
        /// Maximum input torque.
        /// </summary>
        private float _maxTorque;

        /// <summary>
        /// Maximum input speed.
        /// </summary>
        private float _maxSpeed;

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
        /// Motor operation mode.
        /// </summary>
        public string motorMode
        {
            get
            {
                return currentProfile.target == "trq" ? "Torque Mode" : "Speed Mode";
            }   
        }

        /// <summary>
        /// Current torque of motor.
        /// </summary>
        public float torque
        {
            get
            {
                return currentProfile.trq;
            }
        }

        /// <summary>
        /// Current speed of motor.
        /// </summary>
        public float speed
        {
            get
            {
                return currentProfile.spd;
            }
        }

        /// <summary>
        /// Current position of motor.
        /// </summary>
        public float position 
        {
            get 
            {
                return currentProfile.pos;
            }
        }

        /// <summary>
        /// Current integration angle of motor.
        /// </summary>
        public float integrationAngle 
        {
            get 
            {
                return currentProfile.integrationAngle;
            }
        }

        /// <summary>
        /// Checking if transmission interval is too short.
        /// </summary>
        /// <returns>Whther or no transmission interval is enough.</returns>
        private bool CheckSendingInterval()
        {
            int comparison = DateTime.Now.CompareTo(_timeOfPreviousSend.AddMilliseconds(MIN_SENDING_INTERVAL_MS));
            return comparison > 0;
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
            TrainingDeviceType type = TrainingDeviceType.Device;
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
            _maxTorque = settingParams.maxTorque;
            _maxSpeed = settingParams.maxSpeed;
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
            _timeOfPreviousSend = DateTime.Now;
            
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
        /// Set motor to torque mode and enter torque.
        /// (Change will not be applied without execution of Apply() method)
        /// </summary>
        /// <param name="torque">Torque value.</param>
        /// <param name="spdLimit">Maximum speed.</param>
        /// <param name="spdLimitLiftup"></param>
        public void SetTorqueMode(float torque, float spdLimit = 10.0f, float spdLimitLiftup = 10.0f)
        {
            _dataToBeSent.target = "trq";
            if(torque > _maxTorque)
            {
                _logger.WriteLog("Input torque limit exceeded.", TRAVELogger.LogLevel.Warn);
                torque = _maxTorque;
            }
            _dataToBeSent.trq = torque;
            _dataToBeSent.spdLimit = spdLimit;
            _dataToBeSent.spdLimitLiftup = spdLimitLiftup;
        }

        /// <summary>
        /// Set motor to speed mode and enter speed.
        /// (Change will not be applied without execution of Apply() method)
        /// </summary>
        /// <param name="speed">Speed value.</param>
        /// <param name="trqLimit">Maximum torque.</param>
        public void SetSpeedMode(float speed, float trqLimit = 6.0f)
        {
            _dataToBeSent.target = "spd";
            if(speed > _maxSpeed)
            {
                _logger.WriteLog("Input speed limit exceeded.", TRAVELogger.LogLevel.Warn);
                speed = _maxSpeed;
            }
            _dataToBeSent.spd = speed;
            _dataToBeSent.trqLimit = trqLimit;
        }

        /// <summary>
        /// Turn on motor.
        /// </summary>
        /// <returns>Whether or not successfully transmitted.</returns>
        public bool TurnOnMotor()
        {
            string command = MOTOR_COMMAND_PREFIX + "1";
            return _communicationBase.SendString(command);
        }

        /// <summary>
        /// Turn off motor.
        /// </summary>
        /// <returns>Whether or not successfully transmitted.</returns>
        public bool TurnOffMotor()
        {
            string command = MOTOR_COMMAND_PREFIX + "0";
            return _communicationBase.SendString(command);
        }

        /// <summary>
        /// Turn on converter.
        /// </summary>
        /// <returns>Whether or not successfully transmitted.</returns>
        public bool TurnOnConverter()
        {
            string command = COVNVERTER_COMMAND_PREFIX + "1";
            return _communicationBase.SendString(command);
        }

        /// <summary>
        /// Turn off converter.
        /// </summary>
        /// <returns>Whether or not successfully transmitted.</returns>
        public bool TurnOffConverter()
        {
            string command = COVNVERTER_COMMAND_PREFIX + "0";
            return _communicationBase.SendString(command);
        }

        /// <summary>
        /// Send device row string.
        /// </summary>
        /// <param name="command">String to be sent.</param>
        /// <returns>Whether or not successfully transmitted.</returns>
        public bool SendString(string command)
        {
            return _communicationBase.SendString(command);
        }

        /// <summary>
        /// Set motor to speed zero mode, which is the safest state.
        /// </summary>
        /// <returns>Whether or not successfully transmitted.</returns>
        public bool RestoreMotor()
        {
            SetSpeedMode(0.0f);
            return Apply();
        }


        /// <summary>
        /// Apply the settings on the device.
        /// This method failed if the last execution is within MIN_SENDING_INTERVAL_MS.
        /// </summary>
        /// <param name="forceChange">If this value is true, transmission interval will not be considered.</param>
        /// <returns>If application is successfully made.</returns>
        public bool Apply(bool forceChange = false)
        {
            if(CheckSendingInterval() || forceChange)
            {
                _currentMotorState = _dataToBeSent;
                if(_communicationBase.SendData(_dataToBeSent))
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
                _logger.WriteLog("Transmission interval is too short.", TRAVELogger.LogLevel.Warn);
                return false;
            }
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