using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TRAVE_unity
{
    public class WebSocket : CommunicationBase 
    {
        private string _portName = "COM1";
        private int _baudRate = 115200;

        private SerialPort _serialPort;
        private Thread _thread;
        private ReceivingDataFormat receivedData;

        public override void Start()
        {
            
        }

        public override void Connect()
        {
            
        }

    }
}