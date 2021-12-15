using System;

namespace HHL.Common
{
    public abstract class BaseInstance<T> where T : class//new()，new不支持非公共的无参构造函数 
    {
        /*
         * 单线程测试通过！
         * 多线程测试通过！
         * 根据需要在调用的时候才实例化单例类！
          */
        private static T _instance;
        private static readonly object SyncObject = new object();
        public static T Inst
        {
            get
            {
                if (_instance == null)//没有第一重 singleton == null 的话，每一次有线程进入 GetInstance()时，均会执行锁定操作来实现线程同步，
                    //非常耗费性能 增加第一重singleton ==null 成立时的情况下执行一次锁定以实现线程同步
                {
                    lock (SyncObject)
                    {
                        if (_instance == null)//Double-Check Locking 双重检查锁定
                        {
                            //_instance = new T();
                            //需要非公共的无参构造函数，不能使用new T() ,new不支持非公共的无参构造函数 
                            _instance = (T)Activator.CreateInstance(typeof(T), true); //第二个参数防止异常：“没有为该对象定义无参数的构造函数。”
                            if (BaseInstance<T>._instance != null)
                            {
                                (BaseInstance<T>._instance as BaseInstance<T>).Init();
                            }
                        }
                    }
                }
                return _instance;
            }
        }
        public abstract void Init();

        public abstract void Dispose();
         
    }

}