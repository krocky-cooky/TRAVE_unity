using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace TRAVE_unity 
{
    public class SendingDataFormat
    {
        
        //トルク指令か速度指令か
        public string target;
        public float trq;
        public float spd;
        public float spdLimit;
        public float trqLimit;

        public SendingDataFormat()
        {
            target = "trq";
            trq = -0.1f;
            spd = -0.1f;
            trqLimit = 6.0f;
            spdLimit = 6.0f;
        }


    }

    public class ReceivingDataFormat
    {
        public string target;
        public float trq;
        public float spd;
        public float pos;
        public float integrationAngle;
    }
}