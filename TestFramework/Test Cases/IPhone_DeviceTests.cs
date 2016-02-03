using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Text;
using System.Net;
using OpenQA.Selenium.Remote;

namespace TestFramework
{
    [TestFixture, Explicit]
    public class IPhone_DeviceTests : DashboardPOM
    {
        public DashboardPOM IPhoneDeviceTestPage;

        [SetUp]
        public void SetupTest()
        {

            driver = useBrowserStackPhone("iPhone", "MAC", "iPhone 6 Plus");

            //Set Browser driver
            //driver = useChrome();
            baseURL = Settings.Default.BaseURL;
            IPhoneDeviceTestPage = new DashboardPOM(driver);

            
            verificationErrors = new StringBuilder();

            //// maximize window
            // Not working for Edge
            //Ie11DeviceTestPage.MaximizeWindow();

            //Set implicit wait
            IPhoneDeviceTestPage.setImplicitWait(10);

        }

        [TearDown]
        public void TeardownTest()
        {
            try
            {
                driver.Quit();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
            Assert.AreEqual("", verificationErrors.ToString());
        }

        [Test]
        public void TheIPhoneLoginTest()
        {
            try
            {
                //Got to base URL
                IPhoneDeviceTestPage.Goto(baseURL);

                //Login
                IPhoneDeviceTestPage.doLogin(IPhoneDeviceTestPage.currentAccount);

                //Test Steps go here
                IPhoneDeviceTestPage.waitForSpinnerDashboard();
                IPhoneDeviceTestPage.waitForPlansToLoadDashboard();
                //Any assertions if requried

                //Take screenshot after test
                System.Threading.Thread.Sleep(1000);
                IPhoneDeviceTestPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "IPhoneLoginResults.png");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                IPhoneDeviceTestPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "IPhoneLoginError.png"); ;
                throw;
            }
        }

    }
}