using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace TRAVE_unity
{
    public enum TrainingDeviceType 
    {
        Device,
        ForceGauge,
    }

    public class TRAVESendingFormat
    {
        //device用
        //トルク指令か速度指令か
        public string target;
        public float trq;
        public float spd;
        public float spdLimit;
        public float trqLimit;
        public float spdLimitLiftup;

        

        public TRAVESendingFormat()
        {
            target = "trq";
            trq = -0.1f;
            spd = -0.1f;
            trqLimit = 6.0f;
            spdLimit = 6.0f;
            spdLimitLiftup = 2.0f;
        }


    }

    public class TRAVEReceivingFormat
    {
        public string target;
        public float trq;
        public float spd;
        public float pos;
        public float integrationAngle;

        //forceGauge用
        public float force;

        public TRAVEReceivingFormat()
        {
            target = "trq";
            trq = 0.0f;
            spd = 0.0f;
            pos = 0.0f;
            integrationAngle = 0.0f;
            force = 0.0f;
        }
    }
}