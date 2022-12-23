using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;

namespace TRAVE_unity
{
    public class Serial : CommunicationBase 
    {
        private string _portName = "COM1";
        private int _baudRate = 115200;

        private SerialPort _serialPort;
        private Thread _thread;
        private string receivedString = "";
        private ReceivingDataFormat receivedData;

        public override bool isConnected
        {
            get {return _serialPort.IsOpen;}
        }

        public override void Start()
        {
            Connect();
        }

        public override void Connect()
        {
            if(isConnected)
            {
                Debug.Log("Serial Port Already Opened");
                return;
            }

            //シリアルポートの開通
            _serialPort = new SerialPort(_portName, baudRate, Parity.None, 8, StopBits.One);
            _serialPort.Open();


            //シリアル読み取りスレッドの開始
            _thread = new Thread(Read);
            _thread.Start();

            OnConnect();
        }

        public override void Close()
        {
            if(_thread != null && _thread.IsAlive)
            {
                _thread.Join();
            }

            if(_serialPort != null && _serialPort.IsOpen)
            {
                _serialPort.Close();
                _serialPort.Dispose();
            }

            OnDisconnect();
        }

        private override void OnConnect()
        {

        }

        private override void OnDisconnect()
        {

        }

        private void Read()
        {
            while(isConnected && _serialPort != null && _serialPort.IsOpen)
            {
                try
                {
                    String message = _serialPort.ReadLine();
                }
                catch (System.Exception e)
                {
                    Debug.logWarning(e.Message);
                }
                
                receivedString = message;
            }
        }

        public override void Update()
        {

        }

        public override ReceivingDataFormat GetReceivedData()
        {
            receivedData = JsonUtility.FromJson<ReceivingDataFormat>();
            return receivedData;
        }

        public override string GetReceivedString()
        {
            return receivedString;
        }

        public override void SendData(SendingDataFormat sendingData)
        {
            if(isConnected)
            {
            }
            else
            {
                Debug.Log("serial port is not opened");
            }
        }

        private void Write(string message)
        {
            message += '\n';
            try
            {
                _serialPort.Write(message);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e.Message);
            }
        }

        

    }
}