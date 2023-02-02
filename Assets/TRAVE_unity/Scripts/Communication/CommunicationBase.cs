using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TRAVE;

namespace TRAVE_unity
{
    /// <summary>
    /// Abstract class for communication methods.
    /// </summary>
    public abstract class CommunicationBase
    {
        /// <summary>
        /// Target device type. (ex ForceGauge
        /// </summary>
        protected TrainingDeviceType _deviceType;

        /// <summary>
        /// Constractor for communication class.
        /// </summary>
        /// <param name="type">device type</param>
        public CommunicationBase(TrainingDeviceType type = TrainingDeviceType.Device)
        {
            _deviceType = type;
        }

        /// <summary>
        /// Wherther or not connection is established.
        /// </summary>
        public abstract bool isConnected{get;}

        /// <summary>
        /// Called by 'Awake' method of Monobehaviour
        /// </summary>
        public abstract void Awake();

        /// <summary>
        /// Called by 'Start' method of Monobehaviour
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// called by 'Update' method of Monobehaviour
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// called by 'OnApplicationQuit' method of Monobehaviour
        /// </summary>
        public abstract void OnApplicationQuit();

        /// <summary>
        /// Establishing connection.
        /// </summary>
        public abstract void Connect();

        /// <summary>
        /// Abort connection.
        /// </summary>
        public abstract void Disconnect();

        /// <summary>
        /// This method has to be called when connection is established.
        /// </summary>
        public abstract void OnConnect();

        /// <summary>
        /// This method has to be called when connection is aborted.
        /// </summary>
        public abstract void OnDisconnect();

        /// <summary>
        /// Get received data.
        /// </summary>
        /// <returns>Received data parsed to TRAVEReceivingFormat <see cref="TRAVEReceivingFormat"/></returns>
        public abstract TRAVEReceivingFormat GetReceivedData();
        
        /// <summary>
        /// Returns row string data of received data.
        /// </summary>
        /// <returns>Row string data of received data</returns>
        public abstract string GetReceivedString();

        /// <summary>
        /// Send data via TRAVESendingFormat. <see cref"TRAVESendingFormat"/>
        /// </summary>
        /// <param name="sendingData">Data to be sent</param>
        /// <returns>Wether or not data was successfully sent.</returns>
        public abstract bool SendData(TRAVESendingFormat sendingData);

        /// <summary>
        /// Send string data.
        /// </summary>
        /// <param name="command">String data to be sent.</param>
        /// <returns>Wether or not data was successfully sent.</returns>
        public abstract bool SendString(string command);

        /// <summary>
        /// Allocate required parameters.
        /// Called when script is started.
        /// </summary>
        /// <param name="settingParams"><see cref="SettingPrams"/></param>
        public abstract void AllocateParams(SettingParams settingParams);


    }
}