using EduvieIoC;
using NUnit.Framework;

namespace EduvieContainer.Tests
{
    public class ContainerTestBase
    {
        protected Container Container;

        [SetUp]
        public void BeforeEach()
        {
            Container = new Container();
        }

        [TearDown]
        public void AfterEach()
        {
            Container = null;
        }
    }

    [TestFixture]
    public class Container_GetInstance : ContainerTestBase
    {
        [Test]
        public void CreatesAnInstanceWithNoParams()
        {
            var subject = (A) Container.GetInstance(typeof(A));

            Assert.IsInstanceOf<A>(subject);
        }

        [Test]
        public void CreateAnInstanceWithParams()
        {
            var subject = (B)Container.GetInstance(typeof(B));
            Assert.IsInstanceOf<A>(subject.A);
        }

        [Test]
        public void ItAllowsParameterlessConstructor()
        {
            var subject = (C)Container.GetInstance(typeof(C));
            Assert.IsTrue(subject.Invoked);
        }

        [Test]
        public void ItAllowsGenericInitialization()
        {
            var subject = Container.GetInstance<A>();
            Assert.IsInstanceOf<A>(subject);
        }

    }

    [TestFixture]
    public class Container_Register : ContainerTestBase
    {
        [Test]
        public void RegisterATypeFromAnInterface()
        {
            Container.Register<IMaterial, Plastic>();
            var subject = Container.GetInstance<IMaterial>();

            Assert.IsInstanceOf<Plastic>(subject);
        }

        interface IMaterial
        {
            int Weight { get; }
        }

        class Plastic : IMaterial
        {
            public int Weight => 42;
        }

        class Metal : IMaterial
        {
            public int Weight => 84;
        }
    }

    [TestFixture]
    public class Container_RegisterSingleton : ContainerTestBase
    {
        [Test]
        public void ItReturnsSingleInstance()
        {
            var pet = new Pet();
            Container.RegisterSingleton(pet);
            var subject = Container.GetInstance<Pet>();

            Assert.IsTrue(pet.Equals(subject));
        }
    }

    internal class Pet
    {
        public Pet()
        {
        }
    }

    internal class C
    {
        public bool? Invoked { get; set; }
        public C()
        {
            Invoked = true;
        }
    }

    class A
    {

    }

    class B
    {
        public B()
        {

        }
        public A A { get; }
        public B(A a)
        {
            A = a;
        }
    }
}
