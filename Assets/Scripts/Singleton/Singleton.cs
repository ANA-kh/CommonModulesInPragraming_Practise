using System;
using System.Reflection;
using UnityEngine;

namespace Singleton
{
    public interface ISingleton
    {
        void Init();
        void Cleanup();
    }

    public abstract class MonoSingleton<T> : MonoBehaviour,ISingleton where T : MonoSingleton<T>
    {
        private static T _instance;
        private bool _hasInit = false;

        //不能私有化构造函数。  unity会用到
        public static T Instance 
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType(typeof(T)) as T;
                    if (_instance == null)
                    {
                        var go = new GameObject("Singleton of " + typeof(T).ToString(), typeof(T))
                        {
                            //对象不会保存到场景中。加载新场景时不会被销毁。相当于HideFlags.DontSaveInBuild | HideFlags.DontSaveInEditor | HideFlags.DontUnloadUnusedAsset
                            hideFlags = HideFlags.DontSave
                        };
                        
                        _instance = go.GetComponent<T>();
                    }
                }

                return _instance;
            }
        }
        
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
            }
        }
        
        public void Init()
        {
            if (_hasInit == false)
            {
                OnInit();
                _hasInit = true;
            }
        }

        public void Cleanup()
        {
            OnCleanup();
            _hasInit = false;
        }

        protected abstract void OnInit();
        protected abstract void OnCleanup();
    }

    public abstract class Singleton<T> : ISingleton where T : Singleton<T> //非常死的约束，使得无法像List<int> list 这样直接当作类型使用； 必须新建一个继承自Singleton<T>的类来使用
    {
        private static T _instance;
        private bool _hasInit = false;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    var type = typeof(T);
                    // 获取私有构造函数
                    var constructorInfos = type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
                    
                    // 获取无参构造函数
                    var ctor = Array.Find(constructorInfos, c => c.GetParameters().Length == 0);
                    
                    if (ctor == null)
                    {
                        throw new Exception("Non-Public Constructor() not found! in " + type);
                    }

                    _instance = ctor.Invoke(null) as T;
                }
                return _instance;
            }
        }

        public void Init()
        {
            if (_hasInit == false)
            {
                OnInit();
                _hasInit = true;
            }
        }

        public void Cleanup()
        {
            OnCleanup();
            _hasInit = false;
        }

        protected abstract void OnInit();
        protected abstract void OnCleanup();
    }
}