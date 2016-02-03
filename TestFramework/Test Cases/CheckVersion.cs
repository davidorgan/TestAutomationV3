using NUnit.Framework;
using System;
using System.Text;
using RelevantCodes.ExtentReports;

namespace TestFramework
{
    [TestFixture]
    public class CheckVersion : DashboardPOM
    {
        public class ExtentManager
        {
            private static ExtentReports extent;

            public static ExtentReports Instance
            {
                get {
                    return extent ??
                           (extent = new ExtentReports(@"..\..\TestVersionReport.html", true, DisplayOrder.NewestFirst));
                }
            }
        }
        public DashboardPOM VersionPage;
        public ExtentReports extent = ExtentManager.Instance;
        public ExtentTest test;

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

            VersionPage = new DashboardPOM(driver);

            baseURL = Settings.Default.BaseURL;
            verificationErrors = new StringBuilder();

            //// maximize window
            VersionPage.MaximizeWindow();

            //Set implicit wait
            VersionPage.setImplicitWait(30);

            extent.Config()
                .DocumentTitle("Software Version Report")
                .ReportName("Software Version")
                .ReportHeadline("Report --- <a href='TestReport.html'>Back to Reports List</a>");

        }

        [TearDown]
        public void TeardownTest()
        {
            try
            {
                extent.EndTest(test);
                extent.Flush();
                driver.Quit();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
            Assert.AreEqual("", verificationErrors.ToString());
        }

        [Test]
        public void A_TheVersionTest()
        {
            try
            {
              
                test = extent.StartTest("Version Test", "Test to check the expected software version.")
                                .AssignCategory(Settings.Default.Driver); 
                //Got to base URL
                VersionPage.Goto(baseURL);

                //Login
                VersionPage.doLogin(VersionPage.currentAccount);

                //Test Steps go here
                VersionPage.checkVersion(Settings.Default.SoftwareVersion);
                //Any assertions if requried

                //Take screenshot after Test
                System.Threading.Thread.Sleep(1000);
                VersionPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "CheckVersionResults.png");
                test.Log(LogStatus.Pass, "Screenshot below: " + test.AddScreenCapture("Screenshots/CheckVersionResults.png"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                VersionPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "CheckVersionError.png");
                test.Log(LogStatus.Fail, "Fail: " + e.Message + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/CheckVersionError.png"));
                throw;
            }
        }

    }
}