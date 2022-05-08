using System;
using EventSystem.EventSystemByID;
using EventSystem.EventSystemByType;
using NUnit.Framework;
using UnityEngine;

namespace EventSystem
{
    public class EventSystemTest
    {
        private class TestEventArgs
        {
            public int x = 0;
            public string str;
        }
        [Test]
        public void TypeEventSystemTest()
        {
            var typeEventSystem = new TypeEventSystem();
            var unRegister = typeEventSystem.Register<TestEventArgs>(
                e =>
                {
                    e.x = 10;
                });
            
            var eventArgs = new TestEventArgs();
            typeEventSystem.Send(eventArgs);
            Assert.AreEqual(10,eventArgs.x);
            
            
            eventArgs.x = 2;
            unRegister.UnRegister();
            typeEventSystem.Send(eventArgs);
            Assert.AreEqual(2,eventArgs.x);
        }

        [Test]
        public void IdEventSystemTest()
        {
            TestEventArgs testEventArgs =new TestEventArgs();
            testEventArgs.str = "test";
            
            IdEventSystem.EventSystem.Register((uint)EventID.QuiteGame, OnQuiteGame);
            var player = new Entity();
            player.EventSystem.Register((uint)ComponentEventID.OnHandWeaponChanged, OnHandWeaponChanged);

            IdEventSystem.EventSystem.Send((uint) EventID.QuiteGame,testEventArgs);
            Assert.AreEqual("QuiteGame",testEventArgs.str);
            
            player.EventSystem.Send((uint) ComponentEventID.OnHandWeaponChanged,testEventArgs);
            Assert.AreEqual("OnHandWeaponChanged",testEventArgs.str);

            testEventArgs.str = "reset";
            IdEventSystem.EventSystem.UnRegister((uint)EventID.QuiteGame, OnQuiteGame);
            player.EventSystem.UnRegister((uint)ComponentEventID.OnHandWeaponChanged, OnHandWeaponChanged);
            
            IdEventSystem.EventSystem.Send((uint) EventID.QuiteGame,testEventArgs);
            Assert.AreEqual("reset",testEventArgs.str);
            
            player.EventSystem.Send((uint) ComponentEventID.OnHandWeaponChanged,testEventArgs);
            Assert.AreEqual("reset",testEventArgs.str);
        }

        private void OnQuiteGame(params object[] datas)
        {
            if (datas[0] is TestEventArgs args)
            {
                args.str = "QuiteGame";
            }
        }
        
        private void OnHandWeaponChanged(params object[] datas)
        {
            if (datas[0] is TestEventArgs args)
            {
                args.str = "OnHandWeaponChanged";
            }
        }
        public class Entity
        {
            private IIdEventSystem _eventSystem;
            public IIdEventSystem EventSystem
            {
                get
                {
                    if(_eventSystem ==null)
                        _eventSystem = new IdEventSystem();
                    return _eventSystem;
                }
            }
        }
    }
}