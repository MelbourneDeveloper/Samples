using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;

namespace MockDelegates
{
    [TestClass]
    public partial class Tests
    {
        [TestMethod]
        public void TestInterface()
        {
            //Mock the interface
            var adderMock = new Mock<IAdder>();

            //Specifiy behavior of Add method
            adderMock.Setup(a => a.Add(It.IsAny<int>(), It.IsAny<int>())).Returns(2);

            //Inject mock in to class
            var simpleInterfaceCalculator = new SimpleInterfaceCalculator(adderMock.Object);

            //Perform the operation
            simpleInterfaceCalculator.Add(1, 1);

            //Verify that the opreation occurred
            adderMock.Verify(a => a.Add(It.IsAny<int>(), It.IsAny<int>()));
        }

        [TestMethod]
        public void TestDelegate()
        {
            //Mock the delegate
            var adderMock = new Mock<Add>();

            //Specifiy behavior of delegate
            adderMock.Setup(a => a(It.IsAny<int>(), It.IsAny<int>())).Returns(2);

            //Inject mock in to class
            var simpleInterfaceCalculator = new SimpleDelegateCalculator(adderMock.Object);

            //Perform the operation
            simpleInterfaceCalculator.Add(1, 1);

            //Verify that the opreation occurred
            adderMock.Verify(a => a(It.IsAny<int>(), It.IsAny<int>()));
        }


        [TestMethod]
        public void TestStringConcatenation()
        {
            //Mock the delegate
            var adderMock = new Mock<Add<string>>();

            //Specifiy behavior of delegate
            adderMock.Setup(a => a(It.IsAny<string>(), It.IsAny<string>())).Returns("  ");

            //Inject mock in to class
            var simpleInterfaceCalculator = new StringConcatenator(adderMock.Object);

            //Perform the operation
            simpleInterfaceCalculator.ConcatenateString(" ", " ");

            //Verify that the opreation occurred
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

            //Get instances of the objects
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var simpleInterfaceCalculator = serviceProvider.GetService<SimpleInterfaceCalculator>();
            var simpleDelegateCalculator = serviceProvider.GetService<SimpleDelegateCalculator>();
            var stringConcatenator = serviceProvider.GetService<StringConcatenator>();

            //Verify

            var a = 1;
            var b = 1;

            var result = simpleInterfaceCalculator.Add(a, b);
            Assert.AreEqual(2, result);

            result = simpleDelegateCalculator.Add(a, b);
            Assert.AreEqual(2, result);

            var stringResult = stringConcatenator.ConcatenateString(" ", " ");
            Assert.AreEqual("  ", stringResult);
        }

        [TestMethod]
        public void TestIocContainerWithDependencies()
        {
            var serviceCollection = new ServiceCollection();

            var mockFileIO = new Mock<IFileIO>();

            //Wire up mock dependency
            serviceCollection.AddSingleton(mockFileIO.Object);

            //Register the class
            serviceCollection.AddSingleton<StringConcatenatorWithDependencies>();

            //Wire up the delegate implementation 
            serviceCollection.AddSingleton<Add<string>>(s => s.GetRequiredService<StringConcatenatorWithDependencies>().ConcatenateString);

            //Get instances of the service
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var add = serviceProvider.GetService<Add<string>>();

            //Verify

            var stringResult = add(" ", " ");
            Assert.AreEqual("  ", stringResult);

            //Verify that the file was written to
            mockFileIO.Verify(f => f.WriteData(It.Is<byte[]>(a => a.SequenceEqual(new byte[] { 32, 32 }))));
        }

    }
}
