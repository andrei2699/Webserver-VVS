// Generated by Selenium IDE

using System;
using System.Collections.Generic;
using System.Drawing;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using Xunit;

namespace WebserverTests.Selenium.Tests
{
    public class NavigateExternalURLTests : IDisposable
    {
        public IWebDriver driver { get; private set; }
        public IDictionary<String, Object> vars { get; private set; }
        public IJavaScriptExecutor js { get; private set; }

        public NavigateExternalURLTests()
        {
            driver = new FirefoxDriver();
            js = (IJavaScriptExecutor)driver;
            vars = new Dictionary<String, Object>();
        }

        public void Dispose()
        {
            driver.Quit();
        }

        // [Fact(Skip = "Test")]
        public void NavigateExternalURL()
        {
            driver.Navigate().GoToUrl("http://localhost:8089/a.html");
            driver.Manage().Window.Size = new Size(550, 691);
            driver.FindElement(By.LinkText("do external links work?")).Click();
        }
    }
}