using UnityEngine;
using TRAVE;

namespace TRAVE_unity
{

    public enum CommunicationType
    {
        Serial,
        WebSockets,
        Bluetooth,
    }

    public enum DeviceOperationType
    {
        Torque,
        Speed
    }


    public class SettingParams : MonoBehaviour
    {
        //セットアップ用変数群
        public CommunicationType communicationType = CommunicationType.Serial;
        public bool printMessage = true;
        public bool printSerialMessage = false;
        public float maxTorque = 4.0f;
        public float maxSpeed = 5.0f;
        public string sendingText;
        public DeviceOperationType operationType = DeviceOperationType.Torque;
        public float torqueModeTorque = 0.0f;
        public float torqueModeSpeedLimit = 1.0f;
        public float speedModeSpeed = 0.0f;
        public float speedModeTorqueLimit = 1.0f;

        public string portName;
        public int portNameIndex;
        public int baudRate;
        public int baudRateIndex;


        //モニタリング用変数群
        public bool isConnected = false;
        public string motorMode = "-";
        public float torque = 0.0f;
        public float speed = 0.0f;
        public float position = 0.0f;
        public float integrationAngle = 0.0f; 


        private TRAVEDevice _device;

        void Start()
        {
            _device = TRAVEDevice.GetDevice();
        }

        void LateUpdate()
        {
            AllocateParams();
        }

        private void AllocateParams()
        {
            TRAVEReceivingFormat currentProfile = _device.currentProfile;
            isConnected = _device.isConnected;
            motorMode = _device.motorMode;
            torque = currentProfile.trq;
            speed = currentProfile.spd;
            position = currentProfile.pos;
            integrationAngle = currentProfile.integrationAngle;
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
                _device.SetTorqueMode(torqueModeTorque, torqueModeSpeedLimit);
            }
            else
            {
                _device.SetSpeedMode(speedModeSpeed, speedModeTorqueLimit);
            }
            _device.Apply();
        }
    }
}