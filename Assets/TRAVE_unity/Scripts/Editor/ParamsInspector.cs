using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;


namespace TRAVE_unity
{
    [CustomEditor(typeof(SettingParams))]
    public class ParamsInspector : Editor 
    {
        SettingParams settingParams;

        SerializedProperty deviceCommunicationType;
        SerializedProperty forceGaugeCommunicationType;
        SerializedProperty maxTorque;
        SerializedProperty maxSpeed;
        SerializedProperty printMessage;
        SerializedProperty printSerialMessage;
        SerializedProperty sendingText;
        SerializedProperty operationType;
        SerializedProperty torque;
        SerializedProperty speed;
        SerializedProperty speedLimit;
        SerializedProperty speedLimitLiftup;
        SerializedProperty torqueLimit;

        //For Serial.cs
        SerializedProperty devicePortName;
        SerializedProperty devicePortNameIndex;
        SerializedProperty deviceBaudRate;
        
        SerializedProperty forceGaugePortName;
        SerializedProperty forceGaugePortNameIndex;
        SerializedProperty forceGaugeBaudRate;

        private DeviceMaster _deviceMaster;
        private ForceGaugeMaster _forceGaugeMaster;


        int[] baudRateValues = { 9600, 115200 };
        string[] baudRateLabels;

        string[] portNames = {};

        private GUIStyle turnOnButtonStyle;
        private GUIStyle turnOffButtonStyle;
        private GUIStyle centeredLabelStyle;
        private GUIStyle monitoringDeviceConnectionValueStyle;
        private GUIStyle monitoringForceGaugeConnectionValueStyle;
        private GUIStyle monitoringLabelStyle;
        private GUIStyle monitoringValueStyle;


        public void OnEnable()
        {
            settingParams = target as SettingParams;

            deviceCommunicationType = serializedObject.FindProperty(nameof(settingParams.deviceCommunicationType));
            forceGaugeCommunicationType = serializedObject.FindProperty(nameof(settingParams.forceGaugeCommunicationType));
            printMessage = serializedObject.FindProperty(nameof(settingParams.printMessage));
            printSerialMessage = serializedObject.FindProperty(nameof(settingParams.printSerialMessage));
            maxTorque = serializedObject.FindProperty(nameof(settingParams.maxTorque));
            maxSpeed = serializedObject.FindProperty(nameof(settingParams.maxSpeed));
            sendingText = serializedObject.FindProperty(nameof(settingParams.sendingText));
            operationType = serializedObject.FindProperty(nameof(settingParams.operationType));
            torque = serializedObject.FindProperty(nameof(settingParams.torqueModeTorque));
            speed = serializedObject.FindProperty(nameof(settingParams.speedModeSpeed));
            speedLimit = serializedObject.FindProperty(nameof(settingParams.torqueModeSpeedLimit));
            speedLimitLiftup = serializedObject.FindProperty(nameof(settingParams.torqueModeSpeedLimitLiftup));
            torqueLimit = serializedObject.FindProperty(nameof(settingParams.speedModeTorqueLimit));

            devicePortName = serializedObject.FindProperty(nameof(settingParams.devicePortName));
            devicePortNameIndex = serializedObject.FindProperty(nameof(settingParams.devicePortNameIndex));
            deviceBaudRate = serializedObject.FindProperty(nameof(settingParams.deviceBaudRate));
            forceGaugePortName = serializedObject.FindProperty(nameof(settingParams.forceGaugePortName));
            forceGaugePortNameIndex = serializedObject.FindProperty(nameof(settingParams.forceGaugePortNameIndex));
            forceGaugeBaudRate = serializedObject.FindProperty(nameof(settingParams.forceGaugeBaudRate));
            // isConnected = serializedObject.FindProperty(nameof(settingParams.isConnected));
            // motorMode = serializedObject.FindProperty(nameof(settingParams.motorMode));
            // torque = serializedObject.FindProperty(nameof(settingParams.torque));
            // speed = serializedObject.FindProperty(nameof(settingParams.speed));
            // position = serializedObject.FindProperty(nameof(settingParams.position));
            // integrationAngle = serializedObject.FindProperty(nameof(settingParams.integrationAngle));
            
            baudRateLabels = new string[baudRateValues.Length];
            for(int i = 0;i < baudRateValues.Length; ++i)
            {
                baudRateLabels[i] = baudRateValues[i].ToString();
            }  


            _deviceMaster = settingParams.GetComponent<DeviceMaster>();
            _forceGaugeMaster = settingParams.GetComponent<ForceGaugeMaster>();

            
        }

        public override void OnInspectorGUI()
        {
            turnOnButtonStyle = new GUIStyle(GUI.skin.button);
            turnOnButtonStyle.normal.textColor = Color.green;

            turnOffButtonStyle = new GUIStyle(GUI.skin.button);
            turnOffButtonStyle.normal.textColor = Color.red;

            centeredLabelStyle = new GUIStyle(GUI.skin.label);
            centeredLabelStyle.fontStyle = FontStyle.Bold;
            centeredLabelStyle.alignment = TextAnchor.MiddleCenter;

            monitoringDeviceConnectionValueStyle = new GUIStyle(GUI.skin.label);
            monitoringDeviceConnectionValueStyle.fontStyle = FontStyle.Bold;
            monitoringDeviceConnectionValueStyle.normal.textColor = settingParams.deviceIsConnected ? Color.green : Color.red;
            monitoringDeviceConnectionValueStyle.alignment = TextAnchor.MiddleCenter;

            monitoringForceGaugeConnectionValueStyle = new GUIStyle(GUI.skin.label);
            monitoringForceGaugeConnectionValueStyle.fontStyle = FontStyle.Bold;
            monitoringForceGaugeConnectionValueStyle.normal.textColor = settingParams.forceGaugeIsConnected ? Color.green : Color.red;
            monitoringForceGaugeConnectionValueStyle.alignment = TextAnchor.MiddleCenter;

            monitoringLabelStyle = new GUIStyle(GUI.skin.label);
            monitoringLabelStyle.fontStyle = FontStyle.Normal;
            monitoringLabelStyle.alignment = TextAnchor.MiddleCenter;

            monitoringValueStyle = new GUIStyle(GUI.skin.label);
            monitoringValueStyle.fontStyle = FontStyle.Normal;
            monitoringValueStyle.normal.textColor = Color.green;
            monitoringValueStyle.alignment = TextAnchor.MiddleCenter;

            portNames = SerialPortManager.GetInstance.portNames;
            List<string> portNameChoices = new List<string>(portNames);
            string divider = string.Empty;
            string unselected = "(Unselected)";
            portNameChoices.Add(divider);
            portNameChoices.Add(unselected);
            portNames = portNameChoices.ToArray();

            serializedObject.Update();

            // if(GUILayout.Button("refresh")) 
            // {
            //     EditorUtility.SetDirty(target);
            // }
            if(_deviceMaster != null)
            {
                RenderDeviceInspector();
                
            }
            if(_forceGaugeMaster != null)
            {
                RenderForceGaugeInspector();
            }
            serializedObject.ApplyModifiedProperties();
        }

        private void RenderDeviceInspector()
        {
            EditorGUILayout.LabelField("General Settings", centeredLabelStyle);
            GUIHelper.BeginVerticalPadded();
            EditorGUILayout.PropertyField(deviceCommunicationType);
            EditorGUILayout.BeginHorizontal();
            // EditorGUILayout.PropertyField(printMessage);
            if(settingParams.printMessage)
            {
                EditorGUILayout.PropertyField(printSerialMessage);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.PropertyField(maxTorque);
            EditorGUILayout.PropertyField(maxSpeed);
            GUIHelper.EndVerticalPadded();
            
            

            switch(settingParams.deviceCommunicationType)
            {
                case CommunicationType.Serial:
                    EditorGUILayout.LabelField("Serial Communication Settings", centeredLabelStyle);
                    GUIHelper.BeginVerticalPadded();
                    if(portNames.Length <= devicePortNameIndex.intValue || portNames[(devicePortNameIndex.intValue+portNames.Length)%portNames.Length] != devicePortName.stringValue)
                    {
                        devicePortNameIndex.intValue = -1;
                    }
                    devicePortNameIndex.intValue = EditorGUILayout.Popup("Port Name", devicePortNameIndex.intValue, portNames);
                    
                    devicePortName.stringValue = portNames[(devicePortNameIndex.intValue+portNames.Length)%portNames.Length];
                    deviceBaudRate.intValue = EditorGUILayout.IntPopup("Baud Rate", deviceBaudRate.intValue, baudRateLabels, baudRateValues);
                    GUIHelper.EndVerticalPadded();
                    break;
                case CommunicationType.WebSockets:
                    EditorGUILayout.LabelField("Websocket Communication Settings", centeredLabelStyle);
                    GUIHelper.BeginVerticalPadded();
                    GUIHelper.EndVerticalPadded();
                    break;
                case CommunicationType.Bluetooth:
                    EditorGUILayout.LabelField("Bluetooth Communication Settings", centeredLabelStyle);
                    GUIHelper.BeginVerticalPadded();
                    GUIHelper.EndVerticalPadded();
                    break;

            }

            if(EditorApplication.isPlaying)
            {
                EditorGUILayout.LabelField("Realtime operation", centeredLabelStyle);
                GUIHelper.BeginVerticalPadded();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(sendingText);
                if(GUILayout.Button("Send Text"))
                {
                    settingParams.sendFieldText();
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                if(GUILayout.Button("Turn On Motor", turnOnButtonStyle))
                {
                    settingParams.TurnOnMotor();
                }
                if(GUILayout.Button("Turn Off Motor", turnOffButtonStyle))
                {
                    settingParams.TurnOffMotor();
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                if(GUILayout.Button("Turn On Converter", turnOnButtonStyle))
                {
                    settingParams.TurnOnConverter();
                }
                if(GUILayout.Button("Turn Off Converter", turnOffButtonStyle))
                {
                    settingParams.TurnOffConverter();
                }
                EditorGUILayout.EndHorizontal();

                //motor operation command
                EditorGUILayout.Space(10);
                GUIHelper.BeginVerticalPadded();
                EditorGUILayout.PropertyField(operationType);
                if(settingParams.operationType == DeviceOperationType.Torque)
                {
                    EditorGUILayout.PropertyField(torque,new GUIContent("Torque"));
                    EditorGUILayout.PropertyField(speedLimit,new GUIContent("Speed Limit"));
                    EditorGUILayout.PropertyField(speedLimitLiftup,new GUIContent("Speed Limit Liftup"));
                    if(GUILayout.Button("Apply", turnOnButtonStyle))
                    {
                        settingParams.Apply();
                    }
                }
                else
                {
                    EditorGUILayout.PropertyField(speed,new GUIContent("Speed"));
                    EditorGUILayout.PropertyField(torqueLimit, new GUIContent("Torque Limit"));
                    if(GUILayout.Button("Apply", turnOnButtonStyle))
                    {
                        settingParams.Apply();
                    }
                }
                GUIHelper.EndVerticalPadded();
                GUIHelper.EndVerticalPadded();
            }

            EditorGUILayout.LabelField("TRAVE Device Monitoring", centeredLabelStyle);
            GUIHelper.BeginVerticalPadded();
            RenderTableRow("State", settingParams.deviceIsConnected ? "Connected" : "Not connected", monitoringLabelStyle, monitoringDeviceConnectionValueStyle);
            RenderTableRow("Motor Mode", settingParams.motorMode, monitoringLabelStyle, monitoringValueStyle);
            RenderTableRow("Current Torque", settingParams.torque.ToString(), monitoringLabelStyle, monitoringValueStyle);
            RenderTableRow("Current Speed", settingParams.speed.ToString(), monitoringLabelStyle, monitoringValueStyle);
            RenderTableRow("Current Position", settingParams.position.ToString(), monitoringLabelStyle, monitoringValueStyle);
            RenderTableRow("Current Integration Angle", settingParams.integrationAngle.ToString(), monitoringLabelStyle, monitoringValueStyle);
            GUIHelper.EndVerticalPadded();
        }

        private void RenderForceGaugeInspector()
        {
            EditorGUILayout.LabelField("General Settings", centeredLabelStyle);
            GUIHelper.BeginVerticalPadded();
            EditorGUILayout.PropertyField(forceGaugeCommunicationType);
            EditorGUILayout.BeginHorizontal();
            // EditorGUILayout.PropertyField(printMessage);
            if(settingParams.printMessage)
            {
                EditorGUILayout.PropertyField(printSerialMessage);
            }
            EditorGUILayout.EndHorizontal();
            GUIHelper.EndVerticalPadded();
            
            

            switch(settingParams.forceGaugeCommunicationType)
            {
                case CommunicationType.Serial:
                    EditorGUILayout.LabelField("Serial Communication Settings", centeredLabelStyle);
                    GUIHelper.BeginVerticalPadded();
                    if(portNames.Length <= forceGaugePortNameIndex.intValue || portNames[(forceGaugePortNameIndex.intValue+portNames.Length)%portNames.Length] != forceGaugePortName.stringValue)
                    {
                        forceGaugePortNameIndex.intValue = -1;
                    }
                    forceGaugePortNameIndex.intValue = EditorGUILayout.Popup("Port Name", forceGaugePortNameIndex.intValue, portNames);
                    
                    forceGaugePortName.stringValue = portNames[(forceGaugePortNameIndex.intValue+portNames.Length)%portNames.Length];
                    forceGaugeBaudRate.intValue = EditorGUILayout.IntPopup("Baud Rate", forceGaugeBaudRate.intValue, baudRateLabels, baudRateValues);
                    GUIHelper.EndVerticalPadded();
                    break;
                case CommunicationType.WebSockets:
                    EditorGUILayout.LabelField("Websocket Communication Settings", centeredLabelStyle);
                    GUIHelper.BeginVerticalPadded();
                    GUIHelper.EndVerticalPadded();
                    break;
                case CommunicationType.Bluetooth:
                    EditorGUILayout.LabelField("Bluetooth Communication Settings", centeredLabelStyle);
                    GUIHelper.BeginVerticalPadded();
                    GUIHelper.EndVerticalPadded();
                    break;
            }

            EditorGUILayout.LabelField("TRAVE ForceGauge Monitoring", centeredLabelStyle);
            GUIHelper.BeginVerticalPadded();
            RenderTableRow("State", settingParams.forceGaugeIsConnected ? "Connected" : "Not connected", monitoringLabelStyle, monitoringForceGaugeConnectionValueStyle);
            RenderTableRow("Force", settingParams.force.ToString(), monitoringLabelStyle, monitoringValueStyle);
            
            GUIHelper.EndVerticalPadded();
        }

        private void RenderTableRow(string label, string value, GUIStyle labelStyle, GUIStyle valueStyle)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, labelStyle);
            EditorGUILayout.LabelField(value, valueStyle);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(0.5f));
            EditorGUILayout.EndHorizontal();
        }

    }
}