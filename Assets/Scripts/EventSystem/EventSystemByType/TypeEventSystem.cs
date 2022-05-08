using System;
using System.Collections.Generic;

namespace EventSystem.EventSystemByType
{
    public interface ITypeEventSystem
    {
        void Send(TypeEventArgs e);
        IUnRegister Register(Action<TypeEventArgs> onEvent);
        void UnRegister(Action<TypeEventArgs> onEvent);
    }
    
    public abstract class TypeEventArgs
    {
            
    }

    public interface IUnRegister
    {
        void UnRegister();
    }
    
    public struct TypeEventSystemUnRegister<T> : IUnRegister
    {
        public ITypeEventSystem TypeEventSystem;
        public Action<TypeEventArgs> OnEvent;
        public void UnRegister()
        {
            TypeEventSystem.UnRegister(OnEvent);
            TypeEventSystem = null;
            OnEvent = null;
        }
    }
    
    public class TypeEventSystem : ITypeEventSystem
    {
        

        private Dictionary<Type, Action<TypeEventArgs>> _eventDic;


        public void Send(TypeEventArgs e)
        {
            throw new NotImplementedException();
        }

        public IUnRegister Register(Action<TypeEventArgs> onEvent)
        {
            throw new NotImplementedException();
        }

        public void UnRegister(Action<TypeEventArgs> onEvent)
        {
            throw new NotImplementedException();
        }
    }
}

