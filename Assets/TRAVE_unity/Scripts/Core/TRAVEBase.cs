using UnityEngine;
using System;


namespace TRAVE_unity 
{
    public abstract class TRAVEBase<SingletonType> 
        where SingletonType : TRAVEBase<SingletonType>, new()
    {
        protected static SingletonType _instance;
        protected TRAVELogger _logger = TRAVELogger.GetInstance;
        

        public static SingletonType GetDevice()
        {
            if(_instance == null)
            {
                _instance = new SingletonType();
            }
            
            return _instance;
        }

        protected TRAVEBase() {}
    }
}