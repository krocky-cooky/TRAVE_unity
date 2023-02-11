using UnityEngine;
using System.IO.Ports;
using System;
using System.Collections.Generic;


namespace TRAVE_unity
{
    /// <summary>
    /// Retrieve and store available serial port.
    /// </summary>
    public class SerialPortManager
    {
        /// <summary>
        /// Singleton instance.
        /// </summary>
        private static readonly SerialPortManager instance = new SerialPortManager();

        /// <summary>
        /// Getter for instance.
        /// </summary>
        public static SerialPortManager GetInstance
        {
            get {return instance;}
        }

        /// <summary>
        /// Availeble portNames.
        /// To get port names, you need to execute 'RetrievePortsAndDevices()' method.
        /// </summary>
        private string[] _portNames;

        /// <summary>
        /// With this property, you can access _portNames after getting port names.
        /// </summary>
        public string[] portNames
        {
            get
            {
                RetrievePortsAndDevices();
                return _portNames;
            }
        }

        public SerialPortManager()
        {
            RetrievePortsAndDevices();
        }

        /// <summary>
        /// Getting available port names.
        /// </summary>
        private void RetrievePortsAndDevices()
        {
            _portNames = SerialPort.GetPortNames();
            // string[] deviceNames = GetDeviceNames();
            // if(deviceNames == null)
            // {
            //     portName2DeviceName = new Dictionary<string,string>();
            // }
            // else
            // {
            //     foreach(string deviceName in deviceNames)
            //     {
            //         foreach(string portName in portNames)
            //         {
            //             string key = $"({portName})";
            //             if(deviceName.Contains(key))
            //             {
            //                 portName2DeviceName[portName] = deviceName;
            //             }
            //         }
            //     }
            // }
        }

        // private string[] GetDeviceNames()
        // {

        //     var check = new System.Text.RegularExpressions.Regex("(COM[1-9][0-9]?[0-9]?)");
        //     List<string> deviceList = new List<string>();

        //     ManagementClass mcPnPEntity = new ManagementClass("Win32_PnPEntity");
        //     ManagementObjectCollection manageObjCol = mcPnPEntity.GetInstances();

        //     foreach(ManagementObject manageObj in manageObjCol)
        //     {
        //         var namePropertyValue = manageObj.GetPropertyValue("Name");
        //         if(namePropertyValue == null)
        //             continue;

        //         string name = namePropertyValue.ToString();
        //         if(check.IsMatch(name))
        //             deviceList.Add(name);

        //         if(deviceList.Count > 0)
        //         {
        //             string[] deviceNames = new string[deviceNames.Count];
        //             int index = 0;
        //             foreach(string name in deviceList)
        //             {
        //                 deviceName[index++] = name;
        //             }
        //             return deviceNames;
        //         }
        //         else
        //         {
        //             return null;
        //         }

        //     }
        // }


    }
}