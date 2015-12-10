using NUnit.Framework;
using System;
using System.Text;
using RelevantCodes.ExtentReports;

namespace TestFramework
{
    [TestFixture]
    public class CheckUsage:UsageHistoryPOM
    {
        public UsageHistoryPOM UsageHistoryPage;

        public class ExtentManager
        {
            private static ExtentReports extent;

            public static ExtentReports Instance
            {
                get {
                    return extent ??
                           (extent =
                               new ExtentReports(@"..\..\TestUsageHistoryReport.html", true, DisplayOrder.NewestFirst));
                }
            }
        }
        public ExtentReports extent = ExtentManager.Instance;
        public ExtentTest test;

        [SetUp]
        public void SetupTest()
        {
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

            UsageHistoryPage = new UsageHistoryPOM(driver);

            baseURL = Settings.Default.BaseURL;
            verificationErrors = new StringBuilder();

            //// maximize window
            UsageHistoryPage.MaximizeWindow();

            //Set implicit wait
            UsageHistoryPage.setImplicitWait(30);

            //Dates strings for Check Usage/Purchase History
            today = DateTime.Now;
            futureDateTo = today.AddDays(60);
            futureDateFrom = today.AddDays(90);
            todayString = today.ToString("dd/MM/yyyy");
            futureDateToString = futureDateTo.ToString("dd/MM/yyyy");
            futureDateFromString = futureDateFrom.ToString("dd/MM/yyyy");

            // Set dates in settings    
            Settings.Default.CurrentDateString = todayString;
            Settings.Default.FutureDateFromString = futureDateFromString;
            Settings.Default.FutureDateToString = futureDateToString;

            // Save all settings
            Settings.Default.Save();
            Settings.Default.Reload();

            extent.Config()
                    .DocumentTitle("Usage History Report")
                    .ReportName("Usage History")
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
        public void B_TheRealUsageTest()
        {
            try
            {
                test = extent.StartTest("Check Usage History Test", "Test to check the usage history is displaying.")
                .AssignCategory(Settings.Default.Driver); 
                //Got to base URL
                UsageHistoryPage.Goto(baseURL);

                //Login
                UsageHistoryPage.doLogin(Settings.Default.User, Settings.Default.Password);

                //Go to Usage Page
                UsageHistoryPage.waitForSpinnerDashboard();
                UsageHistoryPage.waitForPlansToLoadDashboard();
                UsageHistoryPage.goToCheckUsage();

                //Enter dates
                UsageHistoryPage.enterCheckUsageDates("01/01/2015", todayString);

                //Submit
                UsageHistoryPage.submitCheckUsage();

                //Click to expand first usage history details ---- No Longer uses expand functionality from 0.2.1.20
                //UsageHistoryPage.doExpandCheckUsage();

                //Assert Usage history displayed.
                UsageHistoryPage.waitForSpinnerDashboard();
                UsageHistoryPage.assertUsageHistoryDisplayed("01/01/2015", todayString);
                outputText = "The expected text was displayed. \rSee screenshot created to confirm Usage History details are correct.";

                //Take screenshot after Test
                System.Threading.Thread.Sleep(1000);
                UsageHistoryPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "CheckUsageResults.png");
                test.Log(LogStatus.Pass, outputText + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/CheckUsageResults.png"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                UsageHistoryPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "CheckUsageError.png");
                test.Log(LogStatus.Fail, "Fail: " + e.Message + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/CheckUsageError.png"));
                throw;
            }
        }

        [Test]
        public void A_TheInvalidUsageTest()
        {
            try
            {
                test = extent.StartTest("Invalid Usage History Test", "Test to check the error validation messages are displayed for usage history.")
.AssignCategory(Settings.Default.Driver); 
                //Got to base URL
                UsageHistoryPage.Goto(baseURL);

                //Login
                UsageHistoryPage.doLogin(Settings.Default.User, Settings.Default.Password);

                //Go to Usage Page
                UsageHistoryPage.waitForSpinnerDashboard();
                UsageHistoryPage.waitForPlansToLoadDashboard();
                UsageHistoryPage.goToCheckUsage();

                //Enter dates
                UsageHistoryPage.enterCheckUsageDates(futureDateFromString, futureDateToString);

                //Submit
                UsageHistoryPage.submitCheckUsage();

                //Assert validation messages
                System.Threading.Thread.Sleep(2000);
                UsageHistoryPage.assertFromDateErrorUsageHistory();
                UsageHistoryPage.assertToDateErrorUsageHistory();
                outputText = "The expected error validation messages were displayed.";


                //Take screenshot after Test
                System.Threading.Thread.Sleep(1000);
                UsageHistoryPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "CheckUsageInvalidResults.png");
                test.Log(LogStatus.Pass, outputText + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/CheckUsageInvalidResults.png"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                UsageHistoryPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "CheckUsageInvalidError.png");
                test.Log(LogStatus.Fail, "Fail: " + e.Message + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/CheckUsageInvalidError.png"));
                throw;
            }
        }

    }
}