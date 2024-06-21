using System;
using System.Linq;
using DotA.API.Helpers;
using NUnit.Framework;

namespace Dota.API.Test
{
    [TestFixture]
    public class ExtensionTests
    {
        private Random _random = new Random();

        [Test]
        [Repeat(100)]
        public void SplitNumberShouldHaveSumOfPartsEqualToNumber()
        {
            var number = _random.Next(0, 100);
            var parts = number.SplitNumber(_random.Next(10), _random.Next(0, 2));
            var sum = parts.Sum();
            Assert.AreEqual(number, sum, $"number: {number}, parts: {string.Join(", ", parts)}, sum: {sum}");
        }

    }
}