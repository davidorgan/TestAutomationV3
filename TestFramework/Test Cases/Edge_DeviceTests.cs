using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Text;
using System.Net;
using OpenQA.Selenium.Remote;

namespace TestFramework
{
    [TestFixture, Explicit]
    public partial class Edge_DeviceTests : DashboardPOM
    {
        public DashboardPOM EdgeDeviceTestPage;

        [SetUp]
        public void SetupTest()
        {
            /*DesiredCapabilities capability = new DesiredCapabilities();
            capability.SetCapability("browserstack.user", "cubictelecom1");
            capability.SetCapability("browserstack.key", "c79ymuRgRUPWiEU9xr1H");
            capability.SetCapability("browser", "IE");
            capability.SetCapability("browser_version", "11.0");
            capability.SetCapability("os", "Windows");
            capability.SetCapability("os_version", "10");
            capability.SetCapability("resolution", "1920x1200");
            capability.SetCapability("browserstack.debug", "true");


            driver = new RemoteWebDriver(
              new Uri("http://hub.browserstack.com/wd/hub/"), capability
            );*/

            driver = useBrowserStack("Edge", "12.0", "Windows", "10", "1920x1200");

            //Set Browser driver
            //driver = useChrome();
            baseURL = Settings.Default.BaseURL;
            EdgeDeviceTestPage = new DashboardPOM(driver);

            
            verificationErrors = new StringBuilder();

            //// maximize window
            // Not working for Edge
            //Ie11DeviceTestPage.MaximizeWindow();

            //Set implicit wait
            EdgeDeviceTestPage.setImplicitWait(10);

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
        public void TheEdgeLoginTest()
        {
            try
            {
                //Got to base URL
                EdgeDeviceTestPage.Goto(baseURL);

                //Login
                EdgeDeviceTestPage.doLogin();

                //Test Steps go here
                EdgeDeviceTestPage.waitForSpinnerDashboard();
                EdgeDeviceTestPage.waitForPlansToLoadDashboard();
                //Any assertions if requried

                //Take screenshot after test
                System.Threading.Thread.Sleep(1000);
                EdgeDeviceTestPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "EdgeLoginResults.png");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                EdgeDeviceTestPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "EdgeLoginError.png"); ;
                throw;
            }
        }

    }
}