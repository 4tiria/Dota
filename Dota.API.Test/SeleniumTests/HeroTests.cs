using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota.API.Test.SeleniumTests
{
    [TestFixture]
    public class HeroTests
    {
        private IWebDriver _webDriver;

        [SetUp]
        public void Setup()
        {
            _webDriver = new ChromeDriver();
            _webDriver.Navigate().GoToUrl("http://localhost:3000/heroes");
            _webDriver.Manage().Window.Maximize();
        }

        [Test]
        public void FirstTest()
        {
            Assert.Pass();
        }
    }
}
