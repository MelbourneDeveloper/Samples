using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MockDelegates
{
    [TestClass]
    public class Tests
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
    }
}
