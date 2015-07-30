using System;
using NUnit.Framework;
using CSF;

namespace Test.CSF
{
  [TestFixture]
  public class TestDateTimeExtensions
  {
    #region tests

    [TestCase(1976, 09, 18, 2012, 10, 03, 36)]
    [TestCase(1976, 09, 18, 2012, 10, 20, 36)]
    [TestCase(1976, 09, 18, 2012, 09, 03, 35)]
    [TestCase(1976, 09, 18, 2012, 09, 20, 36)]
    [TestCase(1976, 09, 18, 2012, 08, 03, 35)]
    [TestCase(1976, 09, 18, 2012, 08, 20, 35)]
    public void TestAsAgeInYears(int birthYear, int birthMonth, int birthDay,
                                 int referenceYear, int referenceMonth, int referenceDay,
                                 int expectedAge)
    {
      // Arrange
      DateTime
        birth = new DateTime(birthYear, birthMonth, birthDay),
        reference = new DateTime(referenceYear, referenceMonth, referenceDay);

      // Act and assert together
      Assert.AreEqual(expectedAge,
                      birth.AsAgeInYears(reference),
                      "Birthday {0}: Age {1} on {2}",
                      birth.ToShortDateString(),
                      expectedAge,
                      reference.ToShortDateString());
    }

    [TestCase(2015,02,12, 2015,02,28)]
    [TestCase(2015,02,01, 2015,02,28)]
    [TestCase(2015,02,28, 2015,02,28)]
    [TestCase(2015,03,01, 2015,03,31)]
    [TestCase(2015,03,04, 2015,03,31)]
    [TestCase(2015,03,31, 2015,03,31)]
    public void TestGetLastDayOfMonth(int givenYear, int givenMonth, int givenDay,
                                      int expectedYear, int expectedMonth, int expectedDay)
    {
      // Arrange
      DateTime
        given = new DateTime(givenYear, givenMonth, givenDay),
        expected = new DateTime(expectedYear, expectedMonth, expectedDay);

      // Act and assert together
      Assert.AreEqual(expected,
                      given.GetLastDayOfMonth(),
                      "Date: {0}, expected: {1}",
                      given.ToShortDateString(),
                      expected.ToShortDateString());
    }

    #endregion
  }
}

