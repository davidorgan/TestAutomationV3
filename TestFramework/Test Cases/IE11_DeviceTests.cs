using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Text;
using System.Net;
using OpenQA.Selenium.Remote;

namespace TestFramework
{
    [TestFixture, Explicit]
    public class IE11_DeviceTests : DashboardPOM
    {
        public DashboardPOM Ie11DeviceTestPage;

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

            driver = useBrowserStack("IE", "11.0", "Windows", "10", "1920x1200");

            //Set Browser driver
            //driver = useChrome();
            baseURL = Settings.Default.BaseURL;
            Ie11DeviceTestPage = new DashboardPOM(driver);

            
            verificationErrors = new StringBuilder();

            //// maximize window
            Ie11DeviceTestPage.MaximizeWindow();

            //Set implicit wait
            Ie11DeviceTestPage.setImplicitWait(10);

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
        public void TheIE11LoginTest()
        {
            try
            {
                //Got to base URL
                Ie11DeviceTestPage.Goto(baseURL);

                //Login
                Ie11DeviceTestPage.doLogin(Settings.Default.User, Settings.Default.Password);

                //Test Steps go here

                //Any assertions if requried

                //Take screenshot after test
                System.Threading.Thread.Sleep(1000);
                Ie11DeviceTestPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "IE11LoginResults.png");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Ie11DeviceTestPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "TemplateError.png"); ;
                throw;
            }
        }

    }
}