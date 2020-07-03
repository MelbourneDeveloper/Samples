using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MockDelegates
{
    [TestClass]
    public partial class DelegateTests
    {
        [TestMethod]
        public void TestInterface()
        {
            //Mock the interface
            var adderMock = new Mock<IAdder>();

            //Specify behavior of Add method
            adderMock.Setup(a => a.Add(It.IsAny<int>(), It.IsAny<int>())).Returns(2);

            //Inject mock in to class
            var simpleInterfaceCalculator = new SimpleInterfaceCalculator(adderMock.Object);

            //Perform the operation
            simpleInterfaceCalculator.Add(1, 1);

            //Verify that the operation occurred
            adderMock.Verify(a => a.Add(It.IsAny<int>(), It.IsAny<int>()));
        }

        [TestMethod]
        public void TestInterfaceWithFunc()
        {
            //Mock the interface
            var adderMock = new Mock<IAdder>();

            //Specify behavior of Add method
            adderMock.Setup(a => a.Add(It.IsAny<int>(), It.IsAny<int>())).Returns<int, int>((a, b) => a + b);

            //Inject mock in to class
            var simpleInterfaceCalculator = new SimpleInterfaceCalculator(adderMock.Object);

            //Perform the operation
            simpleInterfaceCalculator.Add(1, 1);

            //Verify that the operation occurred
            adderMock.Verify(a => a.Add(It.IsAny<int>(), It.IsAny<int>()));
        }

        [TestMethod]
        public void TestDelegate()
        {
            //Mock the delegate
            var adderMock = new Mock<Add>();

            //Specify behavior of delegate
            adderMock.Setup(a => a(It.IsAny<int>(), It.IsAny<int>())).Returns(2);

            //Inject mock in to class
            var simpleInterfaceCalculator = new SimpleDelegateCalculator(adderMock.Object);

            //Perform the operation
            simpleInterfaceCalculator.Add(1, 1);

            //Verify that the operation occurred
            adderMock.Verify(a => a(It.IsAny<int>(), It.IsAny<int>()));
        }

        [TestMethod]
        public void TestDelegateWithFunc()
        {
            //Mock the delegate
            var adderMock = new Mock<Add>();

            //Specify behavior of delegate
            adderMock.Setup(a => a(It.IsAny<int>(), It.IsAny<int>())).Returns(new Add((a, b) => a + b));

            //Inject mock in to class
            var simpleInterfaceCalculator = new SimpleDelegateCalculator(adderMock.Object);

            //Perform the operation
            simpleInterfaceCalculator.Add(1, 1);

            //Verify that the operation occurred
            adderMock.Verify(a => a(It.IsAny<int>(), It.IsAny<int>()));
        }

        [TestMethod]
        public void TestStringConcatenation()
        {
            //Mock the delegate
            var adderMock = new Mock<Add<string>>();

            //Specify behavior of delegate
            adderMock.Setup(a => a(It.IsAny<string>(), It.IsAny<string>())).Returns("  ");

            //Inject mock in to class
            var simpleInterfaceCalculator = new StringConcatenator(adderMock.Object);

            //Perform the operation
            simpleInterfaceCalculator.ConcatenateString(" ", " ");

            //Verify that the operation occurred
            adderMock.Verify(a => a(It.IsAny<string>(), It.IsAny<string>()));
        }



        [TestMethod]
        public void TestIocContainer()
        {
            var serviceCollection = new ServiceCollection();

            //Wire up the interface implementation
            serviceCollection.AddSingleton<IAdder, Adder>();

            //Wire up the delegate implementation
            serviceCollection.AddSingleton<Add>((a, b) => a + b);

            //Wire up the generic delegate implementation
            serviceCollection.AddSingleton<Add<string>>((a, b) => a + b);

            //Register the three classes
            serviceCollection.AddSingleton<SimpleDelegateCalculator>();
            serviceCollection.AddSingleton<SimpleInterfaceCalculator>();
            serviceCollection.AddSingleton<StringConcatenator>();
            serviceCollection.AddSingleton(typeof(Workflow)

            //Get instances of the objects
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var simpleInterfaceCalculator = serviceProvider.GetService<SimpleInterfaceCalculator>();
            var simpleDelegateCalculator = serviceProvider.GetService<SimpleDelegateCalculator>();
            var stringConcatenator = serviceProvider.GetService<StringConcatenator>();

            //Verify

            const int firstNumber = 1;
            const int secondNumber = 1;

            var result = simpleInterfaceCalculator.Add(firstNumber, secondNumber);
            Assert.AreEqual(2, result);

            result = simpleDelegateCalculator.Add(firstNumber, secondNumber);
            Assert.AreEqual(2, result);

            var stringResult = stringConcatenator.ConcatenateString(" ", " ");
            Assert.AreEqual("  ", stringResult);
        }

        [TestMethod]
        public void TestIocContainerWithDependencies()
        {
            var serviceCollection = new ServiceCollection();

            var mockFileIo = new Mock<IFileIo>();

            //Wire up mock dependency
            serviceCollection.AddSingleton(mockFileIo.Object);

            //Register the class
            serviceCollection.AddSingleton<StringConcatenatorWithDependencies>();

            //Wire up the delegate implementation 
            serviceCollection.AddSingleton<Add<string>>(s => s.GetRequiredService<StringConcatenatorWithDependencies>().ConcatenateString);

            //Get instance of the service
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var add = serviceProvider.GetService<Add<string>>();

            //Call the implementation
            var stringResult = add(" ", " ");

            //Verify
            Assert.AreEqual("  ", stringResult);

            //Verify that the file was written to
            mockFileIo.Verify(f => f.WriteData(It.Is<byte[]>(a => a.SequenceEqual(new byte[] { 32, 32 }))));
        }

        [TestMethod]
        public void TestStructureMapContainerWithDependencies()
        {
            var mockFileIo = new Mock<IFileIo>();

            var container = new Container(_ =>
            {

                //Wire up mock dependency
                _.For<IFileIo>().Use(mockFileIo.Object);

                //Register the class
                _.For<StringConcatenatorWithDependencies>();

                //Wire up the delegate implementation 
                _.For<Add<string>>().Use<Add<string>>(c => c.GetInstance<StringConcatenatorWithDependencies>().ConcatenateString);
            });

            //Get instance of the service
            var add = container.GetInstance<Add<string>>();

            //Call the implementation
            var stringResult = add(" ", " ");

            //Verify
            Assert.AreEqual("  ", stringResult);

            //Verify that the file was written to
            mockFileIo.Verify(f => f.WriteData(It.Is<byte[]>(a => a.SequenceEqual(new byte[] { 32, 32 }))));
        }

        [TestMethod]
        public void TestIocContainerWithFactoryInjection()
        {
            var adders = new Dictionary<string, IAdder> { { "simple", new Adder() } };
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<CreateInstance<IAdder>>(provider => name => adders[name]);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var adderFactory = serviceProvider.GetService<CreateInstance<IAdder>>();
            var adder = adderFactory("simple");
            var result = adder.Add(1, 1);
            Assert.AreEqual(2, result);
        }


        [TestMethod]
        public void TestIocContainerWithFactoryInjection2()
        {
            var adders = new Dictionary<string, IAdder> { { "simple", new Adder() } };
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<CreateInstance<IAdder>>(provider => name => adders[name]);
            serviceCollection.AddSingleton<SimpleFactoryInterfaceCalculator>();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var simpleFactoryInterfaceCalculator = serviceProvider.GetService<SimpleFactoryInterfaceCalculator>();
            var result = simpleFactoryInterfaceCalculator.Add(1, 1);
            Assert.AreEqual(2, result);
        }
    }
}
