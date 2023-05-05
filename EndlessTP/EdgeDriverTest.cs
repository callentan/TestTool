using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Interactions;
using System.Linq;
using System.Threading;

namespace EndlessTP
{
    [TestClass]
    public class EdgeDriverTest
    {
        private EdgeDriver _driver;

        [TestInitialize]
        public void EdgeDriverInitialize()
        {
            // Initialize edge driver 
            var options = new EdgeOptions
            {
                PageLoadStrategy = PageLoadStrategy.Normal
            };
            var edgeDriverDirectory = ".";
            _driver = new EdgeDriver(edgeDriverDirectory, options);

            // Login and open testcase manually
            // Like https://dev.azure.com/xxxxxxx/xxxxxxxxxx/_testPlans/execute?planId=xxxxx&suiteId=xxxxx
            _driver.Url = "";

            var windowHandles = _driver.WindowHandles;
            _driver.SwitchTo().Window(windowHandles[1]);
        }

        [TestMethod]
        public void AllTestPass()
        {
            ExecuteNextCase();
        }

        [TestCleanup]
        public void EdgeDriverCleanup()
        {
            _driver.Quit();
        }

        private void ExecuteNextCase()
        {
            var isNextCaseAvailable = GetExecuterToolbarNextButton().Enabled;

            if (isNextCaseAvailable)
            {
                var testStepsList = _driver.FindElement(By.ClassName("test-run-steps-list"));
                var testSteps = testStepsList.FindElements(By.CssSelector("ul.items > li"));
                foreach (IWebElement step in testSteps)
                {
                    var passButton = step.FindElement(By.ClassName("pass-test-step"));
                    passButton.Click();
                }

                Actions actions = new Actions(_driver);
                actions.KeyDown(Keys.Alt).SendKeys(Keys.ArrowRight).KeyUp(Keys.Alt).Perform();
                Thread.Sleep(1500);

                ExecuteNextCase();
            }
            else {
                var testToolBar = _driver.FindElement(By.ClassName("testRun-SaveClose-toolbar"));
                var testToolBarButtons = testToolBar.FindElements(By.CssSelector("ul.menu-bar > li"));
                var saveAndCloseButton = testToolBarButtons[1];
                saveAndCloseButton.Click();
            }

        }

        private IWebElement GetExecuterToolbarNextButton()
        {
            var testExecuterToolbar = _driver.FindElement(By.ClassName("menu-bar"));
            return testExecuterToolbar.FindElements(By.CssSelector("li")).LastOrDefault();
        }
    }
}
