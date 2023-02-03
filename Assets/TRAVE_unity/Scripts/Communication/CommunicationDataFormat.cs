using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace TRAVE_unity
{
    /// <summary>
    /// Device interface type.
    /// </summary>
    public enum TrainingDeviceType 
    {
        Device,
        ForceGauge,
    }

    /// <summary>
    /// Format protocol for sending.
    /// </summary>
    public class TRAVESendingFormat
    {
        /// <summary>
        /// Motor operation mode. "spd" or "trq"
        /// if "trq", trq,spdLimit and spdLimitLiftup is valid.
        /// if "spd", spd and trqLimit is valid.
        /// </summary>
        public string target;

        /// <summary>
        /// Torque value to be commanded.
        /// Available when "trq" mode. 
        /// </summary>
        public float trq;

        /// <summary>
        /// Speed value to be commanded. 
        /// Available when "spd" mode.
        /// </summary>
        public float spd;

        /// <summary>
        /// Maximum value of speed when torque is specified.
        /// Available when "trq" mode.
        /// </summary>
        public float spdLimit;

        /// <summary>
        /// Maximum value of torque when speed is specified.
        /// Available when "spd" mode.
        /// </summary>
        public float trqLimit;

        /// <summary>
        /// 
        /// </summary>
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
        
        /// <summary>
        /// Motor operation mode. "spd" or "trq"
        /// </summary>
        public string target;

        /// <summary>
        /// Current torque value of the motor.
        /// </summary>
        public float trq;

        /// <summary>
        /// Current speed value of the motor.
        /// </summary>
        public float spd;

        /// <summary>
        /// Current rotation position of the motor.
        /// </summary>
        public float pos;

        /// <summary>
        /// Current angle of the motor rotation.
        /// </summary>
        public float integrationAngle;

        /// <summary>
        /// Current force value of the force gauge.
        /// </summary>
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