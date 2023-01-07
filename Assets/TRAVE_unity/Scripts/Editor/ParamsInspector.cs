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


        private GUIStyle centeredLabel;
        private GUIStyle monitoringConnection;
        private GUIStyle monitoringLabel;
        private GUIStyle monitoringValue;


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

            centeredLabel = new GUIStyle(GUI.skin.label);
            centeredLabel.fontStyle = FontStyle.Bold;
            centeredLabel.alignment = TextAnchor.MiddleCenter;

            monitoringConnection = new GUIStyle(GUI.skin.label);
            monitoringConnection.fontStyle = FontStyle.Bold;
            monitoringConnection.normal.textColor = isConnected.boolValue ? Color.green : Color.red;
            monitoringConnection.alignment = TextAnchor.MiddleCenter;

            monitoringLabel = new GUIStyle(GUI.skin.label);
            monitoringLabel.fontStyle = FontStyle.Normal;
            monitoringLabel.alignment = TextAnchor.MiddleCenter;

            monitoringValue = new GUIStyle(GUI.skin.label);
            monitoringValue.fontStyle = FontStyle.Normal;
            monitoringValue.normal.textColor = Color.green;
            monitoringValue.alignment = TextAnchor.MiddleCenter;

            serializedObject.Update();

            EditorGUILayout.LabelField("General Settings", centeredLabel);
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
                if(GUILayout.Button("Turn On Motor"))
                {
                    settingParams.TurnOnMotor();
                }
                if(GUILayout.Button("Turn Off Motor"))
                {
                    settingParams.TurnOffMotor();
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                if(GUILayout.Button("Turn On Converter"))
                {
                    settingParams.TurnOnConverter();
                }
                if(GUILayout.Button("Turn Off Converter"))
                {
                    settingParams.TurnOffConverter();
                }
                EditorGUILayout.EndHorizontal();
            }
            GUIHelper.EndVerticalPadded();

            EditorGUILayout.LabelField("Serial Communication Settings", centeredLabel);
            GUIHelper.BeginVerticalPadded();
            if(portNames.Length <= portNameIndex.intValue || portNames[(portNameIndex.intValue+portNames.Length)%portNames.Length] != portName.stringValue)
            {
                portNameIndex.intValue = -1;
            }
            portNameIndex.intValue = EditorGUILayout.Popup("Port Name", portNameIndex.intValue, portNames);
            portName.stringValue = portNames[(portNameIndex.intValue+portNames.Length)%portNames.Length];
            baudRate.intValue = EditorGUILayout.IntPopup("Baud Rate", baudRate.intValue, baudRateLabels, baudRateValues);
            GUIHelper.EndVerticalPadded();

            EditorGUILayout.LabelField("Websocket Communication Settings", centeredLabel);
            GUIHelper.BeginVerticalPadded();
            GUIHelper.EndVerticalPadded();

            EditorGUILayout.LabelField("Bluetooth Communication Settings", centeredLabel);
            GUIHelper.BeginVerticalPadded();
            GUIHelper.EndVerticalPadded();

            EditorGUILayout.LabelField("TRAVE Device Monitoring", centeredLabel);
            GUIHelper.BeginVerticalPadded();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("State", monitoringLabel);
            EditorGUILayout.LabelField(isConnected.boolValue ? "Connected" : "Not connected", monitoringConnection);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(0.5f));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Motor Mode", monitoringLabel);
            EditorGUILayout.LabelField(motorMode.stringValue, monitoringValue);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(0.5f));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Current Torque", monitoringLabel);
            EditorGUILayout.LabelField(torque.floatValue.ToString(), monitoringValue);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(0.5f));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Current Speed", monitoringLabel);
            EditorGUILayout.LabelField(speed.floatValue.ToString(), monitoringValue);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(0.5f));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Current Position", monitoringLabel);
            EditorGUILayout.LabelField(position.floatValue.ToString(), monitoringValue);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(0.5f));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Current integration angle", monitoringLabel);
            EditorGUILayout.LabelField(integrationAngle.floatValue.ToString(), monitoringValue);
            EditorGUILayout.EndHorizontal();

            GUIHelper.EndVerticalPadded();


            serializedObject.ApplyModifiedProperties();
        }

        private string ConvertLabel(string name, string data)
        {
            if(data == null)
            {
                return $"{name}";
            }
            else
            {
                string text = $"{name}    {data}";
                return text;
            }
            
        }


    }
}