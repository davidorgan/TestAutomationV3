using NUnit.Framework;
using System;
using System.Text;

namespace TestFramework
{
    [TestFixture]
    [Ignore("This is a template Test case and should not be run")]
    public class TestTemplate:BasePageObject
    {
        public DashboardPOM TemplatePage;

        [SetUp]
        public void SetupTest()
        {
            //Set Browser driver
            if (Settings.Default.Driver.Equals("Chrome"))
            {
                driver = useChrome();
            }
            else if (Settings.Default.Driver.Equals("Firefox"))
            {
                driver = useFirefox();
            }
            else if (Settings.Default.Driver.Equals("IE"))
            {
                driver = useIE();
            }

            TemplatePage = new DashboardPOM(driver);

            baseURL = Settings.Default.BaseURL;
            verificationErrors = new StringBuilder();

            //// maximize window
            TemplatePage.MaximizeWindow();

            //Set implicit wait
            TemplatePage.setImplicitWait(30);

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
        public void TheTemplateTest()
        {
            try
            {
                //Got to base URL
                TemplatePage.Goto(baseURL);

                //Login
                TemplatePage.doLogin(Settings.Default.User, Settings.Default.Password);

                //Test Steps go here

               //Any assertions if requried

                //Take screenshot after Test
                System.Threading.Thread.Sleep(1000);
                TemplatePage.TakeScreenshot(@""+ Settings.Default.ScreenshotPath +"TemplateResults.png");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                TemplatePage.TakeScreenshot(@""+ Settings.Default.ScreenshotPath +"TemplateError.png"); 
                throw;
            }
        }

    }
}