using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace SimpleIOC
{
    public interface ISimpleIOC
    {
        /// <summary>
        /// 简单注册类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void Register<T>();

        /// <summary>
        /// 注册为单例
        /// </summary>
        /// <param name="instance"></param>
        /// <typeparam name="T"></typeparam>
        void RegisterInstance<T>(object instance);
        void RegisterInstance(object instance);
        
        
        /// <summary>
        /// 注册依赖
        /// </summary>
        /// <typeparam name="TBase"></typeparam>
        /// <typeparam name="TConcrete"></typeparam>
        void Register<TBase, TConcrete>() where TConcrete : TBase;
      
        /// <summary>
        ///  获取实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        T Resolve<T>();

        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="obj"></param>
        void Inject(object obj);

        /// <summary>
        /// 清空
        /// </summary>
        void Clear();
    }
    
    public class SimpleIOCInjectAttribute: Attribute {}

    public class SimpleIOC :ISimpleIOC
    {
        private HashSet<Type> _registeredTypes = new HashSet<Type>();
        
        private Dictionary<Type, object> _instances = new Dictionary<Type, object>();
        private Dictionary<Type,Type> _dependency = new Dictionary<Type, Type>();


        public void Register<T>()
        {
            _registeredTypes.Add(typeof(T));
        }

        public void RegisterInstance<T>(object instance)
        {
            _instances.Add(typeof(T),instance);
        }

        public void RegisterInstance(object instance)
        {
            var type = instance.GetType();
            
            _instances.Add(type,instance);
        }

        public void Register<TBase, TConcrete>() where TConcrete : TBase
        {
            var concreteObj = typeof(TConcrete);
            var baseObj = typeof(TBase);

            if (baseObj.IsAssignableFrom(concreteObj))
            {
                _dependency.Add(baseObj, concreteObj);   
            }
        }

        public T Resolve<T>()
        {
            var type = typeof(T);

            return (T)Resolve(type);
        }

        private object Resolve(Type type)
        {
            if (_instances.ContainsKey(type))
            {
                return _instances[type];
            }

            if (_dependency.ContainsKey(type))
            {
                return Activator.CreateInstance(_dependency[type]);
            }

            if (_registeredTypes.Contains(type))
            {
                return Activator.CreateInstance(type);
            }

            return default;
        }

        public void Inject(object obj)
        {
            foreach (var propertyInfo in obj.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(SimpleIOCInjectAttribute)).Any()))
            {
                var instance = Resolve(propertyInfo.PropertyType);

                if (instance != null)
                {
                    propertyInfo.SetValue(obj,instance);
                }
                else
                {
                    Debug.LogErrorFormat("不能获取类型为：{0}的对象",propertyInfo.PropertyType);
                }
            }
        }

        public void Clear()
        {
            _dependency.Clear();
            _instances.Clear();
            _registeredTypes.Clear();
        }
    }
}