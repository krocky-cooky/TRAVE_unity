using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TRAVE;


namespace TRAVE_unity
{
    public class Bluetooth : CommunicationBase 
    {

        public Bluetooth(TrainingDeviceType type) : base(type) 
        {}

        

        public override bool isConnected
        {
            get
            {
                return true;
            }
        }

        public override void Awake()
        {
            
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

        public override TRAVEReceivingFormat GetReceivedData()
        {
            return new TRAVEReceivingFormat();
        }

        public override string GetReceivedString()
        {
            return "";
        }

        public override bool SendData(TRAVESendingFormat seningData)
        {
            return false;
        }

        public override bool SendString(string command)
        {
            return false;
        }

        public override void AllocateParams(SettingParams settingParams)
        {

        }

        public override void OnApplicationQuit()
        {
            
        }

    }
}