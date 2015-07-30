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
      DateTime
        birth = new DateTime(birthYear, birthMonth, birthDay),
        reference = new DateTime(referenceYear, referenceMonth, referenceDay);

      Assert.AreEqual(expectedAge,
                      birth.AsAgeInYears(reference),
                      "Birthday {0}: Age {1} on {2}",
                      birth.ToShortDateString(),
                      expectedAge,
                      reference.ToShortDateString());
    }

    #endregion
  }
}

