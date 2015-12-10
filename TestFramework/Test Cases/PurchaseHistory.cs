using NUnit.Framework;
using System;
using System.Text;
using RelevantCodes.ExtentReports;

namespace TestFramework
{
    [TestFixture]
    public class PurchaseHistory : PurchaseHistoryPOM
    {
        private PurchaseHistoryPOM PurchaseHistoryPage;

        private class ExtentManager
        {
            private static ExtentReports extent;

            public static ExtentReports Instance
            {
                get {
                    return extent ??
                           (extent =
                               new ExtentReports(@"..\..\TestPurchaseHistoryReport.html", true, DisplayOrder.NewestFirst));
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

            PurchaseHistoryPage = new PurchaseHistoryPOM(driver);

            baseURL = Settings.Default.BaseURL;
            verificationErrors = new StringBuilder();

            //// maximize window
            PurchaseHistoryPage.MaximizeWindow();

            //Set implicit wait
            PurchaseHistoryPage.setImplicitWait(30);

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
        .DocumentTitle("Purchase History Report")
        .ReportName("Purchase History")
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
        public void B_TheValidPurchaseHistoryTest()
        {
            try
            {
                test = extent.StartTest("Valid Purchase History Test", "Test the Purchase History is displayed with valid dates.")
.AssignCategory(Settings.Default.Driver);

                //Got to base URL
                PurchaseHistoryPage.Goto(baseURL);

                //Login
                PurchaseHistoryPage.doLogin(Settings.Default.User, Settings.Default.Password);

                //Go to Usage Page
                PurchaseHistoryPage.goToPurchaseHistory();

                //Enter dates
                PurchaseHistoryPage.enterPurchaseHistory("01/01/2015", todayString);

                //Submit
                PurchaseHistoryPage.submitPurchaseHistory();

                //Click to expand first usage history details -- No longer uses drop down
                //PurchaseHistoryPage.doExpandPurchaseHistory();

                //Assertions
                PurchaseHistoryPage.assertPurchaseHistoryDetailsRow();
                outputText = "Purchase History details displayed.";

                //Take screenshot after Test
                System.Threading.Thread.Sleep(1000);
                PurchaseHistoryPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "PurchaseHistoryResults.png");
                test.Log(LogStatus.Pass, outputText + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/PurchaseHistoryResults.png"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                PurchaseHistoryPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "PurchaseHistoryError.png");
                test.Log(LogStatus.Fail, "Fail: " + e.Message + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/PurchaseHistoryError.png"));
                throw;
            }
        }

        [Test]
        public void A_TheInvalidPurchaseHistoryTest()
        {
            try
            {
                test = extent.StartTest("Invalid Purchase History Test", "Test the Purchase History error validation messages display with invalid dates.")
.AssignCategory(Settings.Default.Driver);
                //Got to base URL
                PurchaseHistoryPage.Goto(baseURL);

                //Login
                PurchaseHistoryPage.doLogin(Settings.Default.User, Settings.Default.Password);

                //Go to Usage Page
                PurchaseHistoryPage.goToPurchaseHistory();

                //Enter dates
                PurchaseHistoryPage.enterPurchaseHistory(futureDateFromString, futureDateToString);

                //Submit
                PurchaseHistoryPage.submitPurchaseHistory();

                //Assert/Wait
                //Wait error message
                System.Threading.Thread.Sleep(2000);

                PurchaseHistoryPage.assertFromDateErrorPurchaseHistory();
                outputText = "Validation message displayed correctly for From date.";
                PurchaseHistoryPage.assertToDateErrorPurchaseHistory();
                outputText += "<br />Validation message displayed correctly for To date.";


                //Take screenshot after Test
                PurchaseHistoryPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "PurchaseHistoryInvalidResults.png");
                test.Log(LogStatus.Pass, outputText + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/PurchaseHistoryInvalidResults.png"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                PurchaseHistoryPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "PurchaseHistoryInvalidError.png");
                test.Log(LogStatus.Fail, "Fail: " + e.Message + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/PurchaseHistoryInvalidError.png"));
                throw;
            }
        }

    }
}
