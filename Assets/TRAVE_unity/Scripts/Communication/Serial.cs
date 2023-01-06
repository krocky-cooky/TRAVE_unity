using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using TRAVE;

namespace TRAVE_unity
{
    public class Serial : CommunicationBase 
    {
        private string _portName;
        private int _baudRate;

        private SerialPort _serialPort;
        private Thread _thread;
        private string receivedString = "";
        private TRAVEReceivingFormat receivedData;

        private TRAVELogger _logger = TRAVELogger.GetInstance;

        public override bool isConnected
        {
            get 
            {
                return _serialPort.IsOpen;
            }
        }

        public override void Start()
        {
            _serialPort = new SerialPort(_portName, _baudRate, Parity.None, 8, StopBits.One);
            Connect();
        }


        public override void Connect()
        {
            if(!_portName.Contains("COM"))
            {
                _logger.writeLog("Serial port is not selected.", TRAVELogger.LogLevel.Warn);
                return;
            }

            if(isConnected)
            {
                _logger.writeLog("Serial Port has Already Opened.", TRAVELogger.LogLevel.Info);
                return;
            }

            //シリアルポートの開通
            try
            {
                _serialPort.Open();
            }
            catch (System.Exception e)
            {
                _logger.writeLog(e.Message, TRAVELogger.LogLevel.Warn);
                return;
            }
            


            //シリアル読み取りスレッドの開始
            _thread = new Thread(Read);
            _thread.Start();

            OnConnect();
        }


        public override void AllocateParams(SettingParams settingParams)
        {
            _portName = settingParams.portName;
            _baudRate = settingParams.baudRate;
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
            _logger.writeLog("Serial port opened.", TRAVELogger.LogLevel.Info);
        }

        public override void OnDisconnect()
        {
            _logger.writeLog("Serial port closed.", TRAVELogger.LogLevel.Info);
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
                    _logger.writeLog(e.Message, TRAVELogger.LogLevel.Warn);
                }
            }
        }

        public override void Update()
        {
        }

        public override TRAVEReceivingFormat GetReceivedData()
        {
            receivedData = JsonUtility.FromJson<TRAVEReceivingFormat>(receivedString);
            return receivedData;
        }

        public override string GetReceivedString()
        {
            return receivedString;
        }

        public override bool SendData(TRAVESendingFormat sendingData)
        {
            if(isConnected)
            {
                string message = JsonUtility.ToJson(sendingData, true);
                return Write(message);
            }
            else
            {
                _logger.writeLog("Serial port not open.", TRAVELogger.LogLevel.Warn);
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
                _logger.writeLog(e.Message, TRAVELogger.LogLevel.Warn);
                return false;
            }
            return true;
        }

        

    }
}