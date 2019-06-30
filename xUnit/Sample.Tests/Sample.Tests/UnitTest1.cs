using System.Collections.Generic;
using Xunit;

namespace Sample.Tests
{
    public class UnitTest1
    {
        /// <summary>
        /// The test data must have this return type and should be static
        /// </summary>
        public static IEnumerable<object[]> TestData
        {
            get
            {
                //Load the sample data from some source like JSON or CSV here.
                var sampleDataList = new List<SampleData>
                {
                    new SampleData { A = 1, B = 2 },
                    new SampleData { A = 3, B = 2 },
                    new SampleData { A = 2, B = 2 },
                    new SampleData { A = 3, B = 23 },
                    new SampleData { A = 43, B = 2 },
                    new SampleData { A = 3, B = 22 },
                    new SampleData { A = 8, B = 2 },
                    new SampleData { A = 7, B = 25 },
                    new SampleData { A = 6, B = 27 },
                    new SampleData { A = 5, B = 2 }
             };

                var retVal = new List<object[]>();
                foreach(var sampleData in sampleDataList)
                {
                    //Add the strongly typed data to an array of objects with one element. This is what xUnit expects.
                    retVal.Add(new object[] { sampleData });
                }
                return retVal;
            }
        }

        /// <summary>
        /// Specify the test data property with an attribute. This method will get executed for each SampleData object in the list
        /// </summary>
        [Theory, MemberData(nameof(TestData))]       
        public void Test1(SampleData sampleData)
        {
            Assert.Equal(sampleData.A + sampleData.B, sampleData.C);
        }
    }
}
