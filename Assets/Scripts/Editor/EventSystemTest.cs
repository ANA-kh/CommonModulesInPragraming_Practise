﻿using System;
using EventSystem.EventSystemByType;
using NUnit.Framework;
using UnityEngine;

namespace EventSystem
{
    public class EventSystemTest
    {
        [Test]
        public void TypeSystemTest()
        {
            TypeEventSystem typeEventSystem = new TypeEventSystem();
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
        public class TestEventArgs
        {
            public int x = 0;
        }
        
    }
}