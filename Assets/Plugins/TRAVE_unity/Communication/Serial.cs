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

        private Logger _logger = Logger.GetInstance;

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
                _logger.writeLog("Serial Port has Already Opened.", Logger.LogLevel.Info);
                return;
            }

            //シリアルポートの開通
            try
            {
                _serialPort = new SerialPort(_portName, _baudRate, Parity.None, 8, StopBits.One);
                _serialPort.Open();
            }
            catch (System.Exception e)
            {
                _logger.writeLog(e.Message, Logger.LogLevel.Warn);
            }
            


            //シリアル読み取りスレッドの開始
            _thread = new Thread(Read);
            _thread.Start();

            OnConnect();
        }

        public override void Disconnect()
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

        public override void OnConnect()
        {
            _logger.writeLog("Serial port opened.", Logger.LogLevel.Info);
        }

        public override void OnDisconnect()
        {
            _logger.writeLog("Serial port closed.", Logger.LogLevel.Info);
        }

        private void Read()
        {
            while(isConnected && _serialPort != null && _serialPort.IsOpen)
            {
                try
                {
                    string message = _serialPort.ReadLine();
                    receivedString = message;
                }
                catch (System.Exception e)
                {
                    _logger.writeLog(e.Message, Logger.LogLevel.Warn);
                }
            }
        }

        public override void Update()
        {

        }

        public override ReceivingDataFormat GetReceivedData()
        {
            receivedData = JsonUtility.FromJson<ReceivingDataFormat>(receivedString);
            return receivedData;
        }

        public override string GetReceivedString()
        {
            return receivedString;
        }

        public override bool SendData(SendingDataFormat sendingData)
        {
            if(isConnected)
            {
                string message = JsonUtility.ToJson(sendingData, true);
                return Write(message);
            }
            else
            {
                _logger.writeLog("Serial port not open.", Logger.LogLevel.Warn);
                return false;
            }
        }

        private bool Write(string message)
        {
            message += '\n';
            try
            {
                _serialPort.Write(message);
            }
            catch (System.Exception e)
            {
                _logger.writeLog(e.Message, Logger.LogLevel.Warn);
                return false;
            }
            return true;
        }

        

    }
}