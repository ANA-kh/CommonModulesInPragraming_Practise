using NUnit.Framework;
using UnityEngine;

namespace SimpleIOC
{
    public class SimpleIOCTest
    {

        [Test]
        public void SimpleIOCRegisterResolveTest()
        {
            var simpleIOC = new SimpleIOC();

            simpleIOC.Register<SimpleIOC>();


            var obj = simpleIOC.Resolve<SimpleIOC>();

            // 是否创建了实例
            Assert.IsNotNull(obj);

            // 不相同 说明是 创建了实例
            Assert.AreNotEqual(simpleIOC, obj);

        }

        [Test]
        public void SimpleIOCResolveRegisteredType()
        {
            var simpleIOC = new SimpleIOC();

            var obj = simpleIOC.Resolve<SimpleIOC>();
            
            Assert.IsNull(obj);
        }

        [Test]
        public void SimpleIOCRegidterTwice()
        {
            var simpleIOC = new SimpleIOC();
            
            simpleIOC.Register<SimpleIOC>();
            simpleIOC.Register<SimpleIOC>();
            
            Assert.IsTrue(true);
        }

        [Test]
        public void SimpleIOCRegidterInstance()
        {
            var simpleIOC = new SimpleIOC();
            
            simpleIOC.RegisterInstance(new SimpleIOC());

            var instanceA = simpleIOC.Resolve<SimpleIOC>();
            var instanceB = simpleIOC.Resolve<SimpleIOC>();
            
            Assert.AreEqual(instanceA,instanceB);
        }

        [Test]
        public void SimpleIOCRegiterDependency()
        {
            var simpleIOC = new SimpleIOC();
            
            simpleIOC.Register<ISimpleIOC,SimpleIOC>();

            var ioc = simpleIOC.Resolve<ISimpleIOC>();
            
            Assert.AreEqual(ioc.GetType(),typeof(SimpleIOC));
        }

        [Test]
        public void SimpleIOCRegisterInstansceDependency()
        {
            var simpleIOC = new SimpleIOC();
            
            simpleIOC.RegisterInstance<ISimpleIOC>(simpleIOC);

            var simpleIOCA = simpleIOC.Resolve<ISimpleIOC>();
            var simpleIOCB = simpleIOC.Resolve<ISimpleIOC>();
            
            Assert.AreEqual(simpleIOC,simpleIOCB);
            Assert.AreEqual(simpleIOCA,simpleIOCB);
        }

        [Test]
        public void SimpleIOCInject()
        {
            var simpleIOC = new SimpleIOC();
            
            simpleIOC.RegisterInstance(new DenpendencyA());
            
            simpleIOC.Register<DenpendencyB>();
            
            var somctrl = new SomeCtrl();
            
            simpleIOC.Inject(somctrl);
            
            Assert.IsNotNull(somctrl.A);
            Assert.IsNotNull(somctrl.B);
            
            Assert.AreEqual(somctrl.A.GetType(),typeof(DenpendencyA));
            Assert.AreEqual(somctrl.B.GetType(),typeof(DenpendencyB));
            
        }
        
        [Test]
        public void SimpleIOCClear()
        {
            var simpleIOC = new SimpleIOC();
            
            simpleIOC.RegisterInstance(new DenpendencyA());
            simpleIOC.RegisterInstance<ISimpleIOC>(simpleIOC);
            simpleIOC.Register<DenpendencyB>();


            simpleIOC.Clear();
            var dependencyA = simpleIOC.Resolve<DenpendencyA>();
            var dependencyB = simpleIOC.Resolve<DenpendencyB>();
            var ioc = simpleIOC.Resolve<ISimpleIOC>();
            
            Assert.IsNull(dependencyA);
            Assert.IsNull(dependencyB);
            Assert.IsNull(ioc);
        }

        [Test]
        public void TestGetGameObject()
        {
            var go = GameObject.Find("UnitTest");
            
            Assert.IsNotNull(go);
        }
    }

    class DenpendencyA { }
    
    class DenpendencyB { }

    class SomeCtrl
    {
        [SimpleIOCInject]
        public DenpendencyA A { get; set; }
     
        [SimpleIOCInject]
        public DenpendencyB B { get; set; }
    }
    
    
}