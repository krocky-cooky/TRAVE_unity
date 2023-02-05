using UnityEngine;
using TRAVE;

namespace TRAVE_unity
{
    /// <summary>
    /// Communication method types.
    /// </summary>
    public enum CommunicationType
    {
        Serial,
        WebSockets,
        Bluetooth,
    }

    /// <summary>
    /// Operation type of the motor of training device.
    /// </summary>
    public enum DeviceOperationType
    {
        Torque,
        Speed
    }


    public class SettingParams : MonoBehaviour
    {

        /// <summary>
        /// Communication method type of TRAVE training device.
        /// <see cref="CommunicationType" />
        /// </summary>
        public CommunicationType deviceCommunicationType = CommunicationType.Serial;
        public bool printMessage = true;
        public bool printSerialMessage = false;
        public float maxTorque = 4.0f;
        public float maxSpeed = 5.0f;
        public string sendingText;
        public DeviceOperationType operationType = DeviceOperationType.Torque;
        public float torqueModeTorque = 0.0f;
        public float torqueModeSpeedLimit = 1.0f;
        public float torqueModeSpeedLimitLiftup = 10.0f;
        public float speedModeSpeed = 0.0f;
        public float speedModeTorqueLimit = 1.0f;

        public string devicePortName;
        public int devicePortNameIndex;
        public int deviceBaudRate;
        public int deviceBaudRateIndex;


        //モニタリング用変数群
        public bool deviceIsConnected = false;
        public string motorMode = "-";
        public float torque = 0.0f;
        public float speed = 0.0f;
        public float position = 0.0f;
        public float integrationAngle = 0.0f; 


        //TRAVEForceGauge用
        //セットアップ用変数群
        public CommunicationType forceGaugeCommunicationType = CommunicationType.Serial;
        public string forceGaugePortName;
        public int forceGaugePortNameIndex;
        public int forceGaugeBaudRate;
        public int forceGaugeBaudRateIndex;

        //モニタリング用変数群
        public bool forceGaugeIsConnected = false;
        public float force;


        private TRAVEDevice _device;
        private TRAVEForceGauge _forceGauge;

        void Awake()
        {
            _device = TRAVEDevice.GetDevice();
            _forceGauge = TRAVEForceGauge.GetDevice();
        }


        void LateUpdate()
        {
            AllocateParams();
        }

        private void AllocateParams()
        {
            {
                TRAVEReceivingFormat currentProfile = _device.currentProfile;
                deviceIsConnected = _device.isConnected;
                motorMode = _device.motorMode;
                torque = currentProfile.trq;
                speed = currentProfile.spd;
                position = currentProfile.pos;
                integrationAngle = currentProfile.integrationAngle;
            }

            {
                TRAVEReceivingFormat currentProfile = _forceGauge.currentProfile;
                forceGaugeIsConnected = _forceGauge.isConnected;
                force = currentProfile.force;
            }
        }

        public void sendFieldText()
        {
            _device.SendString(sendingText);
        }

        public void TurnOnMotor()
        {
            _device.TurnOnMotor();
        }

        public void TurnOffMotor()
        {
            _device.TurnOffMotor();
        }

        public void TurnOnConverter()
        {
            _device.TurnOnConverter();
        }

        public void TurnOffConverter()
        {
            _device.TurnOffConverter();
        }

        public void Apply()
        {
            if(operationType == DeviceOperationType.Torque)
            {
                _device.SetTorqueMode(torqueModeTorque, torqueModeSpeedLimit, torqueModeSpeedLimitLiftup);
            }
            else
            {
                _device.SetSpeedMode(speedModeSpeed, speedModeTorqueLimit);
            }
            _device.Apply();
        }
    }
}