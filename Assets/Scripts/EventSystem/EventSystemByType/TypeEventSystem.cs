using System;
using System.Collections.Generic;

namespace EventSystem.EventSystemByType
{
    public interface ITypeEventSystem
    {
        void Send<T>() where T : new();
        void Send<T>(T e);
        void Register<T>(IEvent onEvent);
        void UnRegister<T>(IEvent onEvent);
    }

    public interface IUnRegister
    {
        void UnRegister();
    }
    
    public interface IEvent
    {
        void OnEvent();
    }
    
    
}

