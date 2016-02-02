using NUnit.Framework;
using System;
using System.Text;
using System.Threading;
using RelevantCodes.ExtentReports;

namespace TestFramework
{
    [TestFixture]
    public class ManageInCarTopUp : DashboardPOM
    {
        private DashboardPOM InCarTopUpPage;

        private class ExtentManager
        {
            private static ExtentReports extent;

            public static ExtentReports Instance
            {
                get
                {
                    return extent ??
                           (extent =
                               new ExtentReports(@"..\..\TestInCarTopUpReport.html", true, DisplayOrder.NewestFirst));
                }
            }
        }

        private ExtentReports extent = ExtentManager.Instance;
        private ExtentTest test;

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

            InCarTopUpPage = new DashboardPOM(driver);

            baseURL = Settings.Default.BaseURL;
            verificationErrors = new StringBuilder();

            //// maximize window
            InCarTopUpPage.MaximizeWindow();

            //Set implicit wait
            InCarTopUpPage.setImplicitWait(30);

            extent.Config()
        .DocumentTitle("Manage In Car Top Up Report")
        .ReportName("In Car Top Up")
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
        public void B_TheEnableInCarTopUpTest()
        {
            try
            {
                test = extent.StartTest("Enable In Car Top Up Test", "Test to enable In Car Top Up")
                .AssignCategory(Settings.Default.Driver); 

                //Got to base URL
                InCarTopUpPage.Goto(baseURL);

                //Login
                InCarTopUpPage.doLogin();
                InCarTopUpPage.waitForSpinnerDashboard();
                InCarTopUpPage.waitForPlansToLoadDashboard();

                //Test Steps go here
                InCarTopUpPage.goToInCarTopUp();
                InCarTopUpPage.waitForSpinnerDashboard();
                //wait for scroll to top
                Thread.Sleep(1000);

                InCarTopUpPage.enableInCarTopUpToggle();
                InCarTopUpPage.clickEnableInCarTopUp();
                InCarTopUpPage.waitForSpinnerDashboard();
                InCarTopUpPage.waitForPlansToLoadDashboard();

                //Any assertions if requried
                InCarTopUpPage.assertInCarTopUpEnabled();

                //Take screenshot after test
                Thread.Sleep(1000);
                InCarTopUpPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "EnableInCarTopUpResults.png");
                test.Log(LogStatus.Pass, "Screenshot below: " + test.AddScreenCapture("Screenshots/EnableInCarTopUpResults.png"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                InCarTopUpPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "EnableInCarTopUpError.png");
                test.Log(LogStatus.Fail, "Fail: " + e.Message + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/EnableInCarTopUpError.png"));
                throw;
            }
        }

        [Test]
        public void A_TheDisableInCarTopUpTest()
        {
            try
            {
                test = extent.StartTest("Disable In Car Top Up Test", "Test to disable In Car Top Up")
                .AssignCategory(Settings.Default.Driver);

                //Got to base URL
                InCarTopUpPage.Goto(baseURL);

                //Login
                InCarTopUpPage.doLogin();
                InCarTopUpPage.waitForSpinnerDashboard();
                InCarTopUpPage.waitForPlansToLoadDashboard();

                //Test Steps go here
                InCarTopUpPage.goToInCarTopUp();
                InCarTopUpPage.waitForSpinnerDashboard();
                //wait for scroll to top
                Thread.Sleep(1000);

                InCarTopUpPage.disableInCarTopUpToggle();
                InCarTopUpPage.clickDisableInCarTopUp();
                InCarTopUpPage.waitForSpinnerDashboard();
                InCarTopUpPage.waitForPlansToLoadDashboard();

                //Any assertions if requried
                InCarTopUpPage.assertInCarTopUpDisabled();

                //Take screenshot after test
                Thread.Sleep(1000);
                InCarTopUpPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "DisableInCarTopUpResults.png");
                test.Log(LogStatus.Pass, "Screenshot below: " + test.AddScreenCapture("Screenshots/DisableInCarTopUpResults.png"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                InCarTopUpPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "DisableInCarTopUpError.png");
                test.Log(LogStatus.Fail, "Fail: " + e.Message + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/DisableInCarTopUpError.png"));
                throw;
            }
        }

        [Test]
        public void C_TheUpdateInCarTopUpTest()
        {
            try
            {
                test = extent.StartTest("Update In Car Top Up Test", "Test to Update In Car Top Up")
                .AssignCategory(Settings.Default.Driver);

                //Got to base URL
                InCarTopUpPage.Goto(baseURL);

                //Login
                InCarTopUpPage.doLogin();
                InCarTopUpPage.waitForSpinnerDashboard();
                InCarTopUpPage.waitForPlansToLoadDashboard();

                //Test Steps go here
                InCarTopUpPage.goToInCarTopUp();
                InCarTopUpPage.waitForSpinnerDashboard();
                //wait for scroll to top
                Thread.Sleep(1000);

                InCarTopUpPage.enableInCarTopUpToggle();
                InCarTopUpPage.selectCreditCardInCarTopUp();
                InCarTopUpPage.enterNumberInCarTopUp();
                InCarTopUpPage.selectLowBalanceYes();
                InCarTopUpPage.selectTopUpLimit("10.00");

                InCarTopUpPage.clickEnableInCarTopUp();
                InCarTopUpPage.waitForSpinnerDashboard();
                InCarTopUpPage.waitForPlansToLoadDashboard();

                //Any assertions if requried
                InCarTopUpPage.assertInCarTopUpEnabled();

                //Take screenshot after test
                Thread.Sleep(1000);
                InCarTopUpPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "UpdateInCarTopUpResults.png");
                test.Log(LogStatus.Pass, "Screenshot below: " + test.AddScreenCapture("Screenshots/UpdateInCarTopUpResults.png"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                InCarTopUpPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "UpdateInCarTopUpError.png");
                test.Log(LogStatus.Fail, "Fail: " + e.Message + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/UpdateInCarTopUpError.png"));
                throw;
            }
        }

        [Test]
        public void D_TheErrorInCarTopUpTest()
        {
            try
            {
                test = extent.StartTest("Error In Car Top Up Test", "Test to get error message for In Car Top Up")
                .AssignCategory(Settings.Default.Driver);

                //Got to base URL
                InCarTopUpPage.Goto(baseURL);

                //Login
                InCarTopUpPage.doLogin();
                InCarTopUpPage.waitForSpinnerDashboard();
                InCarTopUpPage.waitForPlansToLoadDashboard();

                //Test Steps go here
                InCarTopUpPage.goToInCarTopUp();
                InCarTopUpPage.waitForSpinnerDashboard();
                //wait for scroll to top
                Thread.Sleep(1000);

                InCarTopUpPage.enableInCarTopUpToggle();
                InCarTopUpPage.selectCreditCardInCarTopUp();
                InCarTopUpPage.enterEmptyNumberInCarTopUp();
                InCarTopUpPage.selectLowBalanceYes();
                InCarTopUpPage.selectTopUpLimit("5.00");

                InCarTopUpPage.clickEnableInCarTopUp();
                InCarTopUpPage.waitForSpinnerDashboard();

                //Any assertions if requried
                InCarTopUpPage.assertInCarTopUpErrorPhone();
                
                //Take screenshot after test
                Thread.Sleep(1000);
                InCarTopUpPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "ErrorInCarTopUpResults.png");
                test.Log(LogStatus.Pass, "Screenshot below: " + test.AddScreenCapture("Screenshots/ErrorInCarTopUpResults.png"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                InCarTopUpPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "ErrorInCarTopUpError.png");
                test.Log(LogStatus.Fail, "Fail: " + e.Message + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/ErrorInCarTopUpError.png"));
                throw;
            }
        }

    }
}