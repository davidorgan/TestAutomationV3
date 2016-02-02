using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Text;
using RelevantCodes.ExtentReports;

namespace TestFramework
{
    [TestFixture]
    public class PurchasePlan : DashboardPOM
    {
        private DashboardPOM PurchasePlanPage;

        private class ExtentManager
        {
            private static ExtentReports _extent;

            public static ExtentReports Instance
            {
                get {
                    return _extent ??
                           (_extent =
                               new ExtentReports(@"..\..\TestPurchasePlanReport.html", true, DisplayOrder.NewestFirst));
                }
            }
        }

        private ExtentReports Extent = ExtentManager.Instance;
        private ExtentTest Test;

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

            PurchasePlanPage = new DashboardPOM(driver);

            baseURL = Settings.Default.BaseURL;
            verificationErrors = new StringBuilder();

            //// maximize window
            PurchasePlanPage.MaximizeWindow();

            //Set implicit wait
            PurchasePlanPage.setImplicitWait(30);
            PurchasePlanPage.setPageLoadTimeout(10);
            PurchasePlanPage.setScriptLoadTimeout(10);


            Extent.Config()
                .DocumentTitle("Purchase Plan Report")
                .ReportName("Purchase Plan")
                .ReportHeadline("Report --- <a href='TestReport.html'>Back to Reports List</a>");

        }

        [TearDown]
        public void TeardownTest()
        {
            try
            {
                Extent.EndTest(Test);
                Extent.Flush();
                driver.Quit();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
            Assert.AreEqual("", verificationErrors.ToString());
        }

        [Test,Explicit]
        public void A_ThePurchaseLocalPlanTest()
        {
            try
            {
                Test = Extent.StartTest("Purchase Plan Test", "Test the Purchase Plan functionality for local plan.")
                                .AssignCategory(Settings.Default.Driver);
                //Got to base URL
                PurchasePlanPage.Goto(baseURL);

                //Login
                PurchasePlanPage.doLogin();

                //Test Steps go here

                //Wait until spinner is first displayed and then no longer displayed
                PurchasePlanPage.waitForSpinnerDashboard();
                PurchasePlanPage.waitForPlansToLoadDashboard();

                PurchasePlanPage.goToTopUp();
                PurchasePlanPage.selectLocalPlanDropDown();

                PurchasePlanPage.waitForSpinnerDashboard();
                System.Threading.Thread.Sleep(1000);

                PurchasePlanPage.select1DayPlan();

                if (!PurchasePlanPage.assertSavedCardPresent())
                {
                    PurchasePlanPage.enterValidCreditCardDetails();
                    PurchasePlanPage.saveCardPayment();
                }

                PurchasePlanPage.submitPurchasePlan();

                //Any assertions if requried
                PurchasePlanPage.waitForSpinnerDashboard();

                PurchasePlanPage.assertLocalPlanPurchaseSuccess();
                outputText = "Plan purchased and success message displayed as expected.";

                //Take screenshot after Test
                System.Threading.Thread.Sleep(1000);
                PurchasePlanPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "PurchaseLocalPlanResults1.png");
                Test.Log(LogStatus.Pass, outputText + "<br />Screenshot below: " + Test.AddScreenCapture("Screenshots/PurchaseLocalPlanResults1.png"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                PurchasePlanPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "PurchaseLocalPlanError.png");
                Test.Log(LogStatus.Fail, "Fail: " + e.Message + "<br />Screenshot below: " + Test.AddScreenCapture("Screenshots/PurchaseLocalPlanError.png"));
                throw;
            }
        }

        [Test, Explicit]
        public void B_ThePurchaseEuropePlanTest()
        {
            try
            {
                Test = Extent.StartTest("Purchase Europe Plan Test", "Test the Purchase Plan functionality for europe plan.")
                                .AssignCategory(Settings.Default.Driver);
                //Got to base URL
                PurchasePlanPage.Goto(baseURL);

                //Login
                PurchasePlanPage.doLogin();

                //Test Steps go here

                //Wait until spinner is first displayed and then no longer displayed
                PurchasePlanPage.waitForSpinnerDashboard();
                PurchasePlanPage.waitForPlansToLoadDashboard();

                PurchasePlanPage.goToTopUp();
                PurchasePlanPage.selectEuropePlanDropdown();

                PurchasePlanPage.waitForSpinnerDashboard();
                System.Threading.Thread.Sleep(1000);

                PurchasePlanPage.selectEU1DayPlan();

                if (!PurchasePlanPage.assertSavedCardPresent())
                {
                    PurchasePlanPage.enterValidCreditCardDetails();
                    PurchasePlanPage.saveCardPayment();
                }

                PurchasePlanPage.submitPurchasePlan();

                //Any assertions if requried
                PurchasePlanPage.waitForSpinnerDashboard();

                PurchasePlanPage.assertEuropePlanPurchaseSuccess();
                outputText = "Plan purchased and success message displayed as expected.";

                //Take screenshot after Test
                System.Threading.Thread.Sleep(1000);
                PurchasePlanPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "PurchaseEUPlanResults.png");
                Test.Log(LogStatus.Pass, outputText + "<br />Screenshot below: " + Test.AddScreenCapture("Screenshots/PurchaseEUPlanResults.png"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                PurchasePlanPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "PurchaseEUPlanError.png");
                Test.Log(LogStatus.Fail, "Fail: " + e.Message + "<br />Screenshot below: " + Test.AddScreenCapture("Screenshots/PurchaseEUPlanError.png"));
                throw;
            }
        }

        [Test]
        public void C_ThePurchasePlanInvalidCardDetails()
        {
            try
            {
                Test = Extent.StartTest("Invalid Credit Card Details Test", "Test the error validation messages for Invalid Credit Card Details.")
                                .AssignCategory(Settings.Default.Driver);
                //Got to base URL
                PurchasePlanPage.Goto(baseURL);

                //Login
                PurchasePlanPage.doLogin();

                //Test Steps go here

                //Wait until spinner is first displayed and then no longer displayed
                PurchasePlanPage.waitForSpinnerDashboard();
                PurchasePlanPage.waitForPlansToLoadDashboard();

                PurchasePlanPage.goToTopUp();
                PurchasePlanPage.selectLocalPlanDropDown();

                PurchasePlanPage.waitForSpinnerDashboard();
                System.Threading.Thread.Sleep(1000);

                PurchasePlanPage.select1DayPlan();

                System.Threading.Thread.Sleep(1000);
                IWebElement ErrorPop = driver.FindElement(By.Id("server-error"));
                if (ErrorPop.Displayed)
                {
                    closeTooManyAttemptPopUp();
                    System.Threading.Thread.Sleep(1000);
                }

                PurchasePlanPage.useDifferentCard();
                System.Threading.Thread.Sleep(2000);

                PurchasePlanPage.enterInvalidCreditCardDetails();
                PurchasePlanPage.submitPurchasePlan();
                PurchasePlanPage.waitForSpinnerDashboard();

                if (ErrorPop.Displayed)
                {
                    closeTooManyAttemptPopUp();
                    System.Threading.Thread.Sleep(1000);
                }

                //Any assertions if requried
                PurchasePlanPage.assertInvalidCreditCardDetailsErrors();
                outputText = "Expected Error Validation messages displayed.";

                //Take screenshot after Test
                System.Threading.Thread.Sleep(1000);
                PurchasePlanPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "InvalidCreditCardResults1.png");
                Test.Log(LogStatus.Pass, outputText + "<br />Screenshot below: " + Test.AddScreenCapture("Screenshots/InvalidCreditCardResults1.png"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                PurchasePlanPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "InvalidCreditCardError.png");
                Test.Log(LogStatus.Fail, "Fail: " + e.Message + "<br />Screenshot below: " + Test.AddScreenCapture("Screenshots/InvalidCreditCardError.png"));
                throw;
            }
        }

        [Test]
        public void D_ThePurchasePlanEmptyCardDetails()
        {
            try
            {
                Test = Extent.StartTest("Empty Credit Card Details Test", "Test the error validation messages for Empty Credit Card Details.")
                                .AssignCategory(Settings.Default.Driver);
                //Got to base URL
                PurchasePlanPage.Goto(baseURL);

                //Login
                PurchasePlanPage.doLogin();

                //Test Steps go here

                //Wait until spinner is first displayed and then no longer displayed
                PurchasePlanPage.waitForSpinnerDashboard();
                PurchasePlanPage.waitForPlansToLoadDashboard();

                PurchasePlanPage.goToTopUp();
                PurchasePlanPage.selectLocalPlanDropDown();

                PurchasePlanPage.waitForSpinnerDashboard();
                System.Threading.Thread.Sleep(1000);

                PurchasePlanPage.select1DayPlan();

                System.Threading.Thread.Sleep(1000);
                IWebElement ErrorPop = driver.FindElement(By.Id("server-error"));
                if (ErrorPop.Displayed)
                {
                    closeTooManyAttemptPopUp();
                    System.Threading.Thread.Sleep(1000);
                }
                PurchasePlanPage.useDifferentCard();
                System.Threading.Thread.Sleep(2000);

                PurchasePlanPage.submitPurchasePlan();
                PurchasePlanPage.waitForSpinnerDashboard();

                if (ErrorPop.Displayed)
                {
                    closeTooManyAttemptPopUp();
                    System.Threading.Thread.Sleep(1000);
                }

                //Any assertions if requried
                PurchasePlanPage.assertEmptyCreditCardDetailsErrors();
                outputText = "Expected Error Validation messages displayed.";

                //Take screenshot after Test
                System.Threading.Thread.Sleep(1000);
                PurchasePlanPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "EmptyCreditCardResults1.png");
                Test.Log(LogStatus.Pass, outputText + "<br />Screenshot below: " + Test.AddScreenCapture("Screenshots/EmptyCreditCardResults1.png"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                PurchasePlanPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "EmptyCreditCardError.png");
                Test.Log(LogStatus.Fail, "Fail: " + e.Message + "<br />Screenshot below: " + Test.AddScreenCapture("Screenshots/EmptyCreditCardError.png"));
                throw;
            }
        }

        [Test]
        public void E_ThePurchasePlanTooManyAttempts()
        {
            try
            {
                Test = Extent.StartTest("To Many Invalid Attempts Test", "Test the too many attempts pop up is displayed when invalid details entered multiple times.")
                                .AssignCategory(Settings.Default.Driver);
                //Got to base URL
                PurchasePlanPage.Goto(baseURL);

                //Login
                PurchasePlanPage.doLogin();

                //Test Steps go here

                //Wait until spinner is first displayed and then no longer displayed
                PurchasePlanPage.waitForSpinnerDashboard();
                PurchasePlanPage.waitForPlansToLoadDashboard();

                PurchasePlanPage.goToTopUp();
                PurchasePlanPage.selectLocalPlanDropDown();

                PurchasePlanPage.waitForSpinnerDashboard();
                System.Threading.Thread.Sleep(1000);

                PurchasePlanPage.select1DayPlan();

                System.Threading.Thread.Sleep(1000);
                IWebElement ErrorPop = driver.FindElement(By.Id("server-error"));
                if (ErrorPop.Displayed)
                {
                    PurchasePlanPage.assertTooManyAttemptsText();
                    closeTooManyAttemptPopUp();
                    System.Threading.Thread.Sleep(1000);
                }
                PurchasePlanPage.useDifferentCard();
                System.Threading.Thread.Sleep(2000);

                PurchasePlanPage.submitPaymentUntilPopUp();

                //Any assertions if requried
                PurchasePlanPage.assertTooManyAttemptsText();
                outputText = "Expected Message Displayed for too many invalid attempts.";

                //Take screenshot after Test
                System.Threading.Thread.Sleep(1000);
                PurchasePlanPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "TooManyAttemptsResults.png");
                Test.Log(LogStatus.Pass, outputText + "<br />Screenshot below: " + Test.AddScreenCapture("Screenshots/TooManyAttemptsResults.png"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                PurchasePlanPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "TooManyAttemptsError.png");
                Test.Log(LogStatus.Fail, "Fail: " + e.Message + "<br />Screenshot below: " + Test.AddScreenCapture("Screenshots/TooManyAttemptsError.png"));
                throw;
            }
        }

    }
}