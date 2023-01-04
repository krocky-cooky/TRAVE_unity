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
        //For Serial.cs
        SerializedProperty portName;
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
        private GUIStyle monitoringLabel;


        public void OnEnable()
        {
            settingParams = target as SettingParams;

            communicationType = serializedObject.FindProperty(nameof(settingParams.communicationType));

            portName = serializedObject.FindProperty(nameof(settingParams.portName));
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
            if(portNames.Length == 0)
            {
                portNames = new string[] { "COM0" };
            }

            
            
        }

        public override void OnInspectorGUI()
        {

            centeredLabel = new GUIStyle(GUI.skin.label);
            centeredLabel.fontStyle = FontStyle.Bold;
            centeredLabel.alignment = TextAnchor.MiddleCenter;

            monitoringLabel = new GUIStyle(GUI.skin.label);
            monitoringLabel.fontStyle = FontStyle.Normal;
            monitoringLabel.alignment = TextAnchor.MiddleCenter;

            serializedObject.Update();

            EditorGUILayout.LabelField("General Settings", centeredLabel);
            GUIHelper.BeginVerticalPadded();
            EditorGUILayout.PropertyField(communicationType);
            GUIHelper.EndVerticalPadded();

            EditorGUILayout.LabelField("Serial Communication Settings", centeredLabel);
            GUIHelper.BeginVerticalPadded();
            portName.stringValue = portNames[EditorGUILayout.Popup("Port Name", 0, portNames)];
            baudRate.intValue = EditorGUILayout.IntPopup("Baud Rate", baudRateValues[0], baudRateLabels, baudRateValues);
            GUIHelper.EndVerticalPadded();

            EditorGUILayout.LabelField("Websocket Communication Settings", centeredLabel);
            GUIHelper.BeginVerticalPadded();
            GUIHelper.EndVerticalPadded();

            EditorGUILayout.LabelField("Bluetooth Communication Settings", centeredLabel);
            GUIHelper.BeginVerticalPadded();
            GUIHelper.EndVerticalPadded();

            EditorGUILayout.LabelField("TRAVE Device Monitoring", centeredLabel);
            EditorGUILayout.LabelField(ConvertLabel("Is Connected", isConnected.boolValue.ToString()), monitoringLabel);
            EditorGUILayout.LabelField(ConvertLabel("Motor Mode", motorMode.stringValue), monitoringLabel);
            EditorGUILayout.LabelField(ConvertLabel("Current Torque", torque.floatValue.ToString()), monitoringLabel);
            EditorGUILayout.LabelField(ConvertLabel("Current Speed", speed.floatValue.ToString()), monitoringLabel);
            EditorGUILayout.LabelField(ConvertLabel("Current position", position.floatValue.ToString()), monitoringLabel);
            EditorGUILayout.LabelField(ConvertLabel("Current integration angle", integrationAngle.floatValue.ToString()), monitoringLabel);


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