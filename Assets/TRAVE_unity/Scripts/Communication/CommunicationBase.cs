using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TRAVE;

namespace TRAVE_unity
{
    public abstract class CommunicationBase
    {
        public abstract bool isConnected{get;}

        public abstract void Start();

        public abstract void Connect();

        public abstract void Disconnect();

        public abstract void OnConnect();

        public abstract void OnDisconnect();

        public abstract void Update();

        public abstract void OnApplicationQuit();

        public abstract TRAVEReceivingFormat GetReceivedData();
        
        public abstract string GetReceivedString();

        public abstract bool SendData(TRAVESendingFormat sendingData);

        public abstract bool SendString(string command);

        public abstract void AllocateParams(SettingParams settingParams);


    }
}