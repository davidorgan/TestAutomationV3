using NUnit.Framework;
using System;
using System.Text;
using RelevantCodes.ExtentReports;

namespace TestFramework
{
    [TestFixture]
    public class PurchaseHistoryDashboard : DashboardPOM
    {
        private DashboardPOM DashboardPurchaseHistoryPage;

        private class ExtentManager
        {
            private static ExtentReports extent;

            public static ExtentReports Instance
            {
                get {
                    return extent ??
                           (extent =
                               new ExtentReports(@"..\..\TestPurchaseHistoryDashboardReport.html", true,
                                   DisplayOrder.NewestFirst));
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

            DashboardPurchaseHistoryPage = new DashboardPOM(driver);

            baseURL = Settings.Default.BaseURL;
            verificationErrors = new StringBuilder();

            //// maximize window
            DashboardPurchaseHistoryPage.MaximizeWindow();

            //Set implicit wait
            DashboardPurchaseHistoryPage.setImplicitWait(30);
            DashboardPurchaseHistoryPage.setPageLoadTimeout(10);
            DashboardPurchaseHistoryPage.setScriptLoadTimeout(10);


            extent.Config()
                .DocumentTitle("Purchase History Dashboard Report")
                .ReportName("Purchase History Dashboard")
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
        public void B_ThePurchaseHistoryDashboardValidTest()
        {
            try
            {
                test = extent.StartTest("Valid Purchase History Dashboard Test", "Test the Purchase History is displayed with valid dates from Dashboard.")
                            .AssignCategory(Settings.Default.Driver);
                //Got to base URL
                DashboardPurchaseHistoryPage.Goto(baseURL);

                //Login
                DashboardPurchaseHistoryPage.doLogin(Settings.Default.User, Settings.Default.Password);

                //Test Steps go here

                //Wait until spinner is first displayed and then no longer displayed
                DashboardPurchaseHistoryPage.waitForPlansToLoadDashboard();

                DashboardPurchaseHistoryPage.goToPurchaseHistoryDashboard();
                DashboardPurchaseHistoryPage.waitForSpinnerDashboard();
                DashboardPurchaseHistoryPage.enterPurchaseHistoryDatesDashboard("01/01/2015",Settings.Default.CurrentDateString);
                DashboardPurchaseHistoryPage.viewPurchaseHistoryDashboard();

                DashboardPurchaseHistoryPage.waitForSpinnerDashboard();
                //Dropdown no longer used in updated layout - 0.2.1.19
                //DashboardPurchaseHistoryPage.dropdownPurchaseHistoryDetailsDashboard();
                System.Threading.Thread.Sleep(1000);
                outputText = "Purchase History details displayed from dashboard.";

                //Any assertions if requried
                DashboardPurchaseHistoryPage.assertPurchaseHistoryDetailsRow();
                outputText += "<br />Purchase history shows expected details.";

                DashboardPurchaseHistoryPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "PurchaseHistoryDashboardValidResults1.png");
                test.Log(LogStatus.Pass, outputText + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/PurchaseHistoryDashboardValidResults1.png"));
                DashboardPurchaseHistoryPage.backPurchaseHistoryDashboard(Settings.Default.CurrentVIN);

                DashboardPurchaseHistoryPage.waitForSpinnerDashboard();

                //Take screenshot after Test
                System.Threading.Thread.Sleep(1000);
                DashboardPurchaseHistoryPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "PurchaseHistoryDashboardValidResults2.png");
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                DashboardPurchaseHistoryPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "PurchaseHistoryDashboardValidError.png");
                test.Log(LogStatus.Fail, "Fail: " + e.Message + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/PurchaseHistoryDashboardValidError.png"));
                throw;
            }
        }

        [Test]
        public void A_ThePurchaseHistoryDashboardInValidTest()
        {
            try
            {
                test = extent.StartTest("Invalid Purchase History Dashboard Test", "Test the Purchase History error validation is displayed correctly from dashboard.")
            .AssignCategory(Settings.Default.Driver);
                //Got to base URL
                DashboardPurchaseHistoryPage.Goto(baseURL);

                //Login
                DashboardPurchaseHistoryPage.doLogin(Settings.Default.User, Settings.Default.Password);

                //Test Steps go here

                //Wait until spinner is first displayed and then no longer displayed
                DashboardPurchaseHistoryPage.waitForPlansToLoadDashboard();

                DashboardPurchaseHistoryPage.goToPurchaseHistoryDashboard();
                DashboardPurchaseHistoryPage.waitForSpinnerDashboard();
                DashboardPurchaseHistoryPage.enterPurchaseHistoryDatesDashboard(Settings.Default.FutureDateFromString, Settings.Default.FutureDateToString);
                DashboardPurchaseHistoryPage.viewPurchaseHistoryDashboard();


                DashboardPurchaseHistoryPage.assertFromDateErrorPurchaseHistoryDash();
                DashboardPurchaseHistoryPage.assertToDateErrorPurchaseHistoryDash();
                outputText = "Purchase history error validation message displayed correctly.";

                //Take screenshot after Test
                System.Threading.Thread.Sleep(1000);
                DashboardPurchaseHistoryPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "PurchaseHistoryDashboardInValidResults1.png");
                test.Log(LogStatus.Pass, outputText + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/PurchaseHistoryDashboardInValidResults1.png"));

                DashboardPurchaseHistoryPage.cancelPurchaseHistoryDashboard();

                DashboardPurchaseHistoryPage.waitForSpinnerDashboard();

                //Any assertions if requried

                //Take screenshot after Test
                System.Threading.Thread.Sleep(1000);
                DashboardPurchaseHistoryPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "PurchaseHistoryDashboardInValidResults2.png");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                DashboardPurchaseHistoryPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "PurchaseHistoryDashboardValidError.png");
                test.Log(LogStatus.Fail, "Fail: " + e.Message + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/PurchaseHistoryDashboardValidError.png"));
                throw;
            }
        }

    }
}