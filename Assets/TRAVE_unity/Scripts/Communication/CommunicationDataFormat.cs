using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace TRAVE 
{
    public class TRAVESendingFormat
    {
        
        //トルク指令か速度指令か
        public string target;
        public float trq;
        public float spd;
        public float spdLimit;
        public float trqLimit;

        public TRAVESendingFormat()
        {
            target = "trq";
            trq = -0.1f;
            spd = -0.1f;
            trqLimit = 6.0f;
            spdLimit = 6.0f;
        }


    }

    public class TRAVEReceivingFormat
    {
        public string target;
        public float trq;
        public float spd;
        public float pos;
        public float integrationAngle;

        public TRAVEReceivingFormat()
        {
            target = "trq";
            trq = 0.0f;
            spd = 0.0f;
            pos = 0.0f;
            integrationAngle = 0.0f;
        }
    }
}