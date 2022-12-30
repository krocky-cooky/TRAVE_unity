using UnityEngine;

namespace TRAVE_unity
{

    public enum CommunicationType
    {
        Serial,
        WebSockets,
        Bluetooth,
    }


    public class CommunicationParams : MonoBehaviour
    {
        public CommunicationType communicationType = CommunicationType.Serial;

        public string portName;
        public int baudRate;
    }
}