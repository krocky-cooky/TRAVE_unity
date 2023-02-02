using UnityEngine;
using System;


namespace TRAVE_unity 
{
    /// <summary>
    /// Devices are Operated via singleton instance.
    /// Any TRAVE devices class have to inherit this class.
    /// </summary>
    /// <typeparam name="SingletonType">Type of singleton instance.</typeparam>
    public abstract class TRAVEBase<SingletonType> 
        where SingletonType : TRAVEBase<SingletonType>, new()
    {
        /// <summary>
        /// Singleton instance.
        /// </summary>
        protected static SingletonType _instance;

        protected TRAVELogger _logger = TRAVELogger.GetInstance;

        /// <summary>
        /// Getter of the singleton instance.
        /// </summary>
        /// <returns>Singleton instance.</returns>
        public static SingletonType GetDevice()
        {
            if(_instance == null)
            {
                _instance = new SingletonType();
            }
            
            return _instance;
        }

        protected TRAVEBase() {}

        /// <summary>
        /// This method is called from 'Awake' method of Monobehaviour.
        /// </summary>
        public abstract void _masterMethod_Awake();

        /// <summary>
        /// This method is called from 'Start' method of Monobehaviour.
        /// </summary>
        public abstract void _masterMethod_Start();

        /// <summary>
        /// This method is called from 'Update' method of Monobehaviour.
        /// </summary>
        public abstract void _masterMethod_Update();

        /// <summary>
        /// This method is called from 'OnApplicationQuit' method of Monobehaviour.
        /// </summary>
        public abstract void _masterMethod_OnApplicationQuit();

        /// <summary>
        /// Asigning required variables via SettingParams.
        /// <see cref="SettingParams"/>
        /// </summary>
        /// <param name="settingParams">Parameters set from the unity editor.<see cref="ParamsInspector"/></param>
        public abstract void _masterMethod_AllocateParams(SettingParams settingParams);
    }
}