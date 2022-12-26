using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TRAVE_unity
{
    public abstract class CommunicationBase : MonoBehaviour 
    {
        public abstract bool isConnected{get;}

        public abstract void Start();

        public abstract void Connect();

        public abstract void Disconnect();

        public abstract void OnConnect();

        public abstract void OnDisconnect();

        public abstract void Update();

        public abstract ReceivingDataFormat GetReceivedData();
        
        public abstract string GetReceivedString();

        public abstract bool SendData(SendingDataFormat sendingData);
    }
}