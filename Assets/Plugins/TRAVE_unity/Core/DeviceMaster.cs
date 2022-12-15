using UnityEngine;
using System.Collections;
using System.Collections.Generic;



namespace TRAVE_unity
{
    public enum CommunicationType
    {
        Serial,
        WebSocket,
        Bluetooth,
    }

    public class DeviceMaster : MonoBehaviour 
    {
        //デバイスとの通信タイプ
        [SerializeField]
        private CommunicationType communicationType = CommunicationType.Serial;

        //コネクションを管理する抽象クラス
        private CommunicationBase communicationBase;


        void Start()
        {
            switch(communicationType)
            {
                case CommunicationType.Serial:
                    communicationBase = new Serial();
                case CommunicationType.WebSocket:
                    communicationBase = new WebSocket();
                case communicationType.Bluetooth:
                    communicationBase = new Bluetooth();
            }
        }

        void Update()
        {
            //コネクションの更新処理
            communicationBase.Update();
        }
    }
}