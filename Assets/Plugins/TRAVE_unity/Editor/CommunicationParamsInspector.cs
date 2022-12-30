using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;


namespace TRAVE_unity
{
    [CustomEditor(typeof(CommunicationParams))]
    public class CommunicationParamsInspector : Editor 
    {
        CommunicationParams communicationParams;

        SerializedProperty communicationType;
        //For Serial.cs
        SerializedProperty portName;
        SerializedProperty baudRate;

        int[] baudRateValues = { 9600, 115200 };
        string[] baudRateLabels;

        string[] portNames = {};

        private GUIStyle centeredLabel;


        public void OnEnable()
        {
            communicationParams = target as CommunicationParams;

            communicationType = serializedObject.FindProperty(nameof(communicationParams.communicationType));

            portName = serializedObject.FindProperty(nameof(communicationParams.portName));
            baudRate = serializedObject.FindProperty(nameof(communicationParams.baudRate));

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

            serializedObject.ApplyModifiedProperties();
        }


    }
}