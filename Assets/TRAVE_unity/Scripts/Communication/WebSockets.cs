using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TRAVE;


namespace TRAVE_unity
{
    public class WebSockets : CommunicationBase 
    {

        public override bool isConnected
        {
            get
            {
                return true;
            }
        }

        public override void Start()
        {
            
        }

        public override void Connect()
        {
            
        }

        public override void Disconnect()
        {

        }

        public override void OnConnect()
        {

        }

        public override void OnDisconnect()
        {

        }

        public override void Update()
        {

        }

        public override ReceivingDataFormat GetReceivedData()
        {
            return new ReceivingDataFormat();
        }

        public override string GetReceivedString()
        {
            return "";
        }

        public override bool SendData(SendingDataFormat seningData)
        {
            return false;
        }

        public override void AllocateParams(SettingParams settingParams)
        {
            
        }


    }
}