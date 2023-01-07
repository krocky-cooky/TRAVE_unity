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

        SerializedProperty communicationType;
        SerializedProperty maxTorque;
        SerializedProperty maxSpeed;
        SerializedProperty printMessage;
        SerializedProperty sendingText;

        //For Serial.cs
        SerializedProperty portName;
        SerializedProperty portNameIndex;
        SerializedProperty baudRate;


        //For monitoring
        SerializedProperty isConnected;
        SerializedProperty motorMode;
        SerializedProperty torque;
        SerializedProperty speed;
        SerializedProperty position;
        SerializedProperty integrationAngle;


        int[] baudRateValues = { 9600, 115200 };
        string[] baudRateLabels;

        string[] portNames = {};

        private GUIStyle turnOnButtonStyle;
        private GUIStyle turnOffButtonStyle;
        private GUIStyle centeredLabelStyle;
        private GUIStyle monitoringConnectionValueStyle;
        private GUIStyle monitoringLabelStyle;
        private GUIStyle monitoringValueStyle;


        public void OnEnable()
        {
            settingParams = target as SettingParams;

            communicationType = serializedObject.FindProperty(nameof(settingParams.communicationType));
            printMessage = serializedObject.FindProperty(nameof(settingParams.printMessage));
            maxTorque = serializedObject.FindProperty(nameof(settingParams.maxTorque));
            maxSpeed = serializedObject.FindProperty(nameof(settingParams.maxSpeed));
            sendingText = serializedObject.FindProperty(nameof(settingParams.sendingText));

            portName = serializedObject.FindProperty(nameof(settingParams.portName));
            portNameIndex = serializedObject.FindProperty(nameof(settingParams.portNameIndex));
            baudRate = serializedObject.FindProperty(nameof(settingParams.baudRate));
            isConnected = serializedObject.FindProperty(nameof(settingParams.isConnected));
            motorMode = serializedObject.FindProperty(nameof(settingParams.motorMode));
            torque = serializedObject.FindProperty(nameof(settingParams.torque));
            speed = serializedObject.FindProperty(nameof(settingParams.speed));
            position = serializedObject.FindProperty(nameof(settingParams.position));
            integrationAngle = serializedObject.FindProperty(nameof(settingParams.integrationAngle));
            

            baudRateLabels = new string[baudRateValues.Length];
            for(int i = 0;i < baudRateValues.Length; ++i)
            {
                baudRateLabels[i] = baudRateValues[i].ToString();
            }

            portNames = SerialPortManager.GetInstance.portNames;
            List<string> portNameChoices = new List<string>(portNames);
            string divider = string.Empty;
            string unselected = "(Unselected)";
            portNameChoices.Add(divider);
            portNameChoices.Add(unselected);
            portNames = portNameChoices.ToArray();
            
            
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

            monitoringConnectionValueStyle = new GUIStyle(GUI.skin.label);
            monitoringConnectionValueStyle.fontStyle = FontStyle.Bold;
            monitoringConnectionValueStyle.normal.textColor = isConnected.boolValue ? Color.green : Color.red;
            monitoringConnectionValueStyle.alignment = TextAnchor.MiddleCenter;

            monitoringLabelStyle = new GUIStyle(GUI.skin.label);
            monitoringLabelStyle.fontStyle = FontStyle.Normal;
            monitoringLabelStyle.alignment = TextAnchor.MiddleCenter;

            monitoringValueStyle = new GUIStyle(GUI.skin.label);
            monitoringValueStyle.fontStyle = FontStyle.Normal;
            monitoringValueStyle.normal.textColor = Color.green;
            monitoringValueStyle.alignment = TextAnchor.MiddleCenter;

            serializedObject.Update();

            EditorGUILayout.LabelField("General Settings", centeredLabelStyle);
            GUIHelper.BeginVerticalPadded();
            EditorGUILayout.PropertyField(communicationType);
            EditorGUILayout.PropertyField(printMessage);
            EditorGUILayout.PropertyField(maxTorque);
            EditorGUILayout.PropertyField(maxSpeed);
            if(EditorApplication.isPlaying)
            {
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
            }
            GUIHelper.EndVerticalPadded();

            switch(settingParams.communicationType)
            {
                case CommunicationType.Serial:
                    EditorGUILayout.LabelField("Serial Communication Settings", centeredLabelStyle);
                    GUIHelper.BeginVerticalPadded();
                    if(portNames.Length <= portNameIndex.intValue || portNames[(portNameIndex.intValue+portNames.Length)%portNames.Length] != portName.stringValue)
                    {
                        portNameIndex.intValue = -1;
                    }
                    portNameIndex.intValue = EditorGUILayout.Popup("Port Name", portNameIndex.intValue, portNames);
                    portName.stringValue = portNames[(portNameIndex.intValue+portNames.Length)%portNames.Length];
                    baudRate.intValue = EditorGUILayout.IntPopup("Baud Rate", baudRate.intValue, baudRateLabels, baudRateValues);
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

            EditorGUILayout.LabelField("TRAVE Device Monitoring", centeredLabelStyle);
            GUIHelper.BeginVerticalPadded();
            RenderTableRow("State", isConnected.boolValue ? "Connected" : "Not connected", monitoringLabelStyle, monitoringConnectionValueStyle);
            RenderTableRow("Motor Mode", motorMode.stringValue, monitoringLabelStyle, monitoringValueStyle);
            RenderTableRow("Current Torque", torque.floatValue.ToString(), monitoringLabelStyle, monitoringValueStyle);
            RenderTableRow("Current Speed", speed.floatValue.ToString(), monitoringLabelStyle, monitoringValueStyle);
            RenderTableRow("Current Position", position.floatValue.ToString(), monitoringLabelStyle, monitoringValueStyle);
            RenderTableRow("Current Integration Angle", integrationAngle.floatValue.ToString(), monitoringLabelStyle, monitoringValueStyle);
            GUIHelper.EndVerticalPadded();

            serializedObject.ApplyModifiedProperties();
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