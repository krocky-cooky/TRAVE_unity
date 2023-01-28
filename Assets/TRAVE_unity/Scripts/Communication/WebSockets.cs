using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using WebSocketSharp;
using TRAVE;


namespace TRAVE_unity
{
    public class WebSockets : CommunicationBase 
    {
        private WebSocket _socket;
        private string _targetPrivateIP;
        private string _receivedString = "";
        private TRAVEReceivingFormat _receivedData;

        private TRAVELogger _logger = TRAVELogger.GetInstance;

        public override bool isConnected
        {
            get
            {
                return false;
            }
        }

        public override void Awake()
        {
            
        }

        public override void Start()
        {
            _socket = new WebSocket(_targetPrivateIP);
            Connect();
            
        }

        public override void Connect()
        {
            if(isConnected)
            {
                _logger.writeLog("Websocket connection has already been established.", TRAVELogger.LogLevel.Info );
                return;
            }
            
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
            return _receivedString;
        }

        public override bool SendData(TRAVESendingFormat seningData)
        {
            return false;
        }

        public override bool SendString(string command)
        {
            return false;
        }

        public override void AllocateParams(Device.SettingParams settingParams)
        {
            
        }

        public override void OnApplicationQuit()
        {
            
        }

    }
}