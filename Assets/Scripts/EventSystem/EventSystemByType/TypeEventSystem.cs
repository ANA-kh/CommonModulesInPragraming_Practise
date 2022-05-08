using System;
using System.Collections.Generic;

namespace EventSystem.EventSystemByType
{
    public interface ITypeEventSystem
    {
        void Send<T>() where T : new();
        void Send<T>(T e);
        IUnRegister Register<T>(Action<T> onEvent); //返回注销对象，避免忘记注销
        void UnRegister<T>(Action<T> onEvent);
    }

    public interface IUnRegister
    {
        void UnRegister();
    }
    
}