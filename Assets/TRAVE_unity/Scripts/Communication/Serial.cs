using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading.Tasks;
using TRAVE;

namespace TRAVE_unity
{
    public class Serial : CommunicationBase 
    {
        private string _portName;
        private int _baudRate;
        private bool _printSerialMessage = false;

        private SerialPort _serialPort;
        private Task _readingTask;
        private bool _cancellationToken = false;
        private string receivedString = "";
        private TRAVEReceivingFormat receivedData;

        private TRAVELogger _logger = TRAVELogger.GetInstance;

        public Serial(TrainingDeviceType type) : base(type) 
        {}

        public override bool isConnected
        {
            get 
            {
                return _serialPort.IsOpen;
            }
        }

        public override void Awake()
        {
            _serialPort = new SerialPort(_portName, _baudRate, Parity.None, 8, StopBits.One);
            Connect();
        }

        public override void Start()
        {
        }


        public override void Connect()
        {

            if(_readingTask != null)
            {
                _cancellationToken = true;
            }

            if(_serialPort != null && _serialPort.IsOpen)
            {
                _serialPort.Close();
                _serialPort.Dispose();
            }

            if(!_portName.Contains("COM"))
            {
                _logger.writeLog("Serial port is not selected.", TRAVELogger.LogLevel.Warn);
                return;
            }

            if(isConnected)
            {
                _logger.writeLog("Serial Port has already opened.", TRAVELogger.LogLevel.Info);
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
            _cancellationToken = false;
            _readingTask = Read();

            OnConnect();
        }


        public override void AllocateParams(SettingParams settingParams)
        {
            if(_deviceType == TrainingDeviceType.Device)
            {    _portName = settingParams.devicePortName;
                _baudRate = settingParams.deviceBaudRate;
                _printSerialMessage = settingParams.printSerialMessage;
            }
            else if(_deviceType == TrainingDeviceType.ForceGauge)
            {

            }
        }
        

        public override void Disconnect()
        {
            if(_readingTask != null)
            {
                _cancellationToken = true;
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

        private async Task<bool> Read()
        {
            return await Task.Run<bool>(() =>
            {
                while(!_cancellationToken && _serialPort != null && _serialPort.IsOpen)
                {
                    try
                    {
                        string message = _serialPort.ReadLine();
                        receivedString = message;
                        if(_printSerialMessage)
                        {
                            _logger.writeLog(receivedString, TRAVELogger.LogLevel.Info);
                        }
                    }
                    catch (System.Exception e)
                    {
                        if(!_cancellationToken)
                            _logger.writeLog(e.Message, TRAVELogger.LogLevel.Warn);
                    }
                }
                return true;
            });

        }

        public override void Update()
        {
        }

        public override void OnApplicationQuit()
        {
            Disconnect();
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
                string message = JsonUtility.ToJson(sendingData);
                return Write(message);
            }
            else
            {
                _logger.writeLog("Serial port not open.", TRAVELogger.LogLevel.Warn);
                return false;
            }
        }

        public override bool SendString(string command)
        {
            if(isConnected)
            {
                return Write(command);
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