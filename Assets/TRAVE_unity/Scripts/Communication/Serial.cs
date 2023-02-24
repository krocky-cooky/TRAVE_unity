using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading.Tasks;
using TRAVE;

namespace TRAVE_unity
{
    /// <summary>
    /// Class to control serial communication.
    /// </summary>
    public class Serial : CommunicationBase 
    {
        /// <summary>
        /// Serial port name.
        /// </summary>
        private string _portName;

        /// <summary>
        /// Transmission baud rate.
        /// </summary>
        private int _baudRate;

        /// <summary>
        /// Whether or not print all of received serial message.
        /// </summary>
        private bool _printSerialMessage = false;

        /// <summary>
        /// Object to controll serial port.
        /// </summary>
        private SerialPort _serialPort;

        /// <summary>
        /// Subthread task to read serial messages.
        /// </summary>
        private Task _readingTask;

        /// <summary>
        /// Flag to cancel reading task.
        /// </summary>
        private bool _cancellationToken = false;

        /// <summary>
        /// Currently received string.
        /// </summary>
        private string receivedString = "";

        /// <summary>
        /// received string formatted to "TRAVEReceivingFormat"
        /// <see cref="TRAVEReceivingFormat" />
        /// </summary>
        private TRAVEReceivingFormat receivedData;

        /// <summary>
        /// Log printing.
        /// </summary>
        private TRAVELogger _logger = TRAVELogger.GetInstance;

        public Serial(TrainingDeviceType type) : base(type) 
        {}

        /// <summary>
        /// Whether or not connection is established.
        /// </summary>
        public override bool isConnected
        {
            get 
            {
                return _serialPort.IsOpen;
            }
        }

        /// <summary>
        /// Called by Monobehaviour 'Awake' method.
        /// </summary>
        public override void Awake()
        {
            _serialPort = new SerialPort(_portName, _baudRate, Parity.None, 8, StopBits.One);
            Connect();
        }

        /// <summary>
        /// Called by Monobehaviour 'Start' method.
        /// </summary>
        public override void Start()
        {
        }

        /// <summary>
        /// Called by Monobehaviour 'Update' method.
        /// </summary>
        public override void Update()
        {
        }

        /// <summary>
        /// Called by Monobehaviour 'OnApplicationQuit' method.
        /// </summary>
        public override void OnApplicationQuit()
        {
            Disconnect();
        }

        /// <summary>
        /// Establishing serial connection.
        /// </summary>
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
                _logger.WriteLog("Serial port is not selected.", TRAVELogger.LogLevel.Warn);
                return;
            }

            if(isConnected)
            {
                _logger.WriteLog("Serial Port has already opened.", TRAVELogger.LogLevel.Info);
                return;
            }

            try
            {
                _serialPort.Open();
            }
            catch (System.Exception e)
            {
                _logger.WriteLog(e.Message, TRAVELogger.LogLevel.Warn);
                return;
            }
            


            //Start thread for reading serial messges.
            _cancellationToken = false;
            _readingTask = Read();

            OnConnect();
        }

        /// <summary>
        /// Allocate required parameters.
        /// Called when script is started.
        /// </summary>
        /// <param name="settingParams"><see cref="SettingPrams"/></param>
        public override void AllocateParams(SettingParams settingParams)
        {
            if(_deviceType == TrainingDeviceType.Device)
            {    _portName = settingParams.devicePortName;
                _baudRate = settingParams.deviceBaudRate;
                _printSerialMessage = settingParams.printSerialMessage;
            }
            else if(_deviceType == TrainingDeviceType.ForceGauge)
            {
                _portName = settingParams.forceGaugePortName;
                _baudRate = settingParams.forceGaugeBaudRate;
                _printSerialMessage = settingParams.printSerialMessage;
            }
        }

        /// <summary>
        /// Disconnect serial.
        /// </summary>
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

        /// <summary>
        /// Method called when connection is established.
        /// </summary>
        public override void OnConnect()
        {
            _logger.WriteLog("Serial port opened.", TRAVELogger.LogLevel.Info);
        }

        /// <summary>
        /// Method called when connection is aborted.
        /// </summary>
        public override void OnDisconnect()
        {
            _logger.WriteLog("Serial port closed.", TRAVELogger.LogLevel.Info);
        }

        /// <summary>
        /// Serial reading thread task.
        /// </summary>
        /// <returns>Thread task for reading serial messages.</returns>
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
                            _logger.WriteLog(receivedString, TRAVELogger.LogLevel.Info);
                        }
                    }
                    catch (System.Exception e)
                    {
                        if(!_cancellationToken)
                            _logger.WriteLog(e.Message, TRAVELogger.LogLevel.Warn);
                    }
                }
                return true;
            });

        }

        /// <summary>
        /// Retrieve received data in TRAVEReceivingFormat.
        /// <see cref="TRAVEReceivingFormat" />
        /// </summary>
        /// <returns>Received data in TRAVEReceivingFormat.</returns>
        public override TRAVEReceivingFormat GetReceivedData()
        {
            receivedData = JsonUtility.FromJson<TRAVEReceivingFormat>(receivedString);
            return receivedData;
        }

        /// <summary>
        /// Get row string of received data.
        /// </summary>
        /// <returns>Received string.</returns>
        public override string GetReceivedString()
        {
            return receivedString;
        }

        /// <summary>
        /// Send data to device in TRAVESendingFormat.
        /// <see cref="TRAVESendingFormat" />
        /// </summary>
        /// <param name="sendingData">Data to be sent.</param>
        /// <returns>Whether or not data is successfully transmitted.</returns>
        public override bool SendData(TRAVESendingFormat sendingData)
        {
            if(isConnected)
            {
                string message = JsonUtility.ToJson(sendingData);
                return Write(message);
            }
            else
            {
                _logger.WriteLog("Serial port not open.", TRAVELogger.LogLevel.Warn);
                return false;
            }
        }

        /// <summary>
        /// Send row string to device.
        /// </summary>
        /// <param name="command">String value to be sent.</param>
        /// <returns>Whether or not data is successfully transmitted.</returns>
        public override bool SendString(string command)
        {
            if(isConnected)
            {
                return Write(command);
            }
            else
            {
                _logger.WriteLog("Serial port not open.", TRAVELogger.LogLevel.Warn);
                return false;
            }
        }

        /// <summary>
        /// Writing value to serial port.
        /// </summary>
        /// <param name="message">Messge to be written,.</param>
        /// <returns>Whether or not message is successfully written.</returns>
        private bool Write(string message)
        {
            message += '\n';
            try
            {
                _serialPort.Write(message);
            }
            catch (System.Exception e)
            {
                _logger.WriteLog(e.Message, TRAVELogger.LogLevel.Warn);
                return false;
            }
            return true;
        }

        

    }
}