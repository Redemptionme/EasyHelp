﻿using System;

namespace HHL.Common
{
    public abstract class Singleton<T> where T : class, new()
    {
        private static T m_instance;
        public static T Inst
        {
            get
            {
                if (Singleton<T>.m_instance == null)
                {
                    Singleton<T>.m_instance = Activator.CreateInstance<T>();
                    if (Singleton<T>.m_instance != null)
                    {
                        (Singleton<T>.m_instance as Singleton<T>).Init();
                    }
                }

                return Singleton<T>.m_instance;
            }
        }

        public static void Release()
        {
            if (Singleton<T>.m_instance != null)
            {
                Singleton<T>.m_instance = (T)((object)null);
            }
        }

        public abstract void Init();

        public abstract void Dispose();
    }
}