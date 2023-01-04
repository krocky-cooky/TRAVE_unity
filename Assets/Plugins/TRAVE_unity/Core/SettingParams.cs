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


    public class SettingParams : MonoBehaviour
    {
        //セットアップ用変数群
        public CommunicationType communicationType = CommunicationType.Serial;

        public string portName;
        public int baudRate;

        private TRAVEDevice _device;

        //モニタリング用変数群
        public bool isConnected = false;
        public string motorMode = "not started";
        public float torque = 0.0f;
        public float speed = 0.0f;
        public float position = 0.0f;
        public float integrationAngle = 0.0f; 
        

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
            isConnected = _device.isConnected;
            motorMode = _device.motorMode;
            torque = _device.torque;
            speed = _device.speed;
            position = _device.position;
            integrationAngle = _device.integrationAngle;
        }
    }
}