using UnityEngine;
using System.Collections;
using System.Collections.Generic;



namespace TRAVE_unity
{
    

    public class DeviceMaster : MonoBehaviour 
    {
        //デバイスとの通信タイプ
        [HideInInspector]
        public CommunicationType communicationType = CommunicationType.Serial;

        //コネクションを管理する抽象クラス
        private CommunicationBase _communicationBase;

        private SendingDataFormat _motorcurrentMotorState;
        private SendingDataFormat _dataToSend = new SendingDataFormat();


        void Start()
        {
            switch(communicationType)
            {
                case CommunicationType.Serial:
                    _communicationBase = gameObject.AddComponent<Serial>();
                    break;
                case CommunicationType.WebSockets:
                    _communicationBase = gameObject.AddComponent<WebSockets>();
                    break;
                case CommunicationType.Bluetooth:
                    _communicationBase = gameObject.AddComponent<Bluetooth>();
                    break;
            }
            
        }

        void Update()
        {
        }

        //トルク指令モードに変更し、トルク値をセットする
        public void SetTorqueMode(float torque, float spdLimit = 10.0f)
        {
            _dataToSend.target = "trq";
            _dataToSend.trq = torque;
            _dataToSend.spdLimit = spdLimit;
        }

        //<sammary> 速度指令モードに変更し、スピード値をセットする </sammary>
        public void SetSpeedMode(float speed, float trqLimit = 2.0f)
        {
            _dataToSend.target = "spd";
            _dataToSend.spd = speed;
            _dataToSend.trqLimit = trqLimit;
        }


        //モーターの開始状態を作る(速度ゼロ指令)
        public bool RestoreMotor()
        {
            SetSpeedMode(0.0f);
            return Apply();
        }

        //<sammary> モーターに変更を適用する </sammary>
        public bool Apply()
        {
            return _communicationBase.SendData(_dataToSend);
        }

        public ReceivingDataFormat GetReceivedData()
        {
            string receivedString = _communicationBase.GetReceivedString();
            return JsonUtility.FromJson<ReceivingDataFormat>(receivedString);
        }

         
    }
}