using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Text;
using System.Threading;
using RelevantCodes.ExtentReports;

namespace TestFramework
{
    [TestFixture]
    public class AddNewCar : DashboardPOM
    {
        private DashboardPOM DashboardPage;

        private class ExtentManager
        {
            private static ExtentReports extent;

            public static ExtentReports Instance
            {
                get {
                    return extent ??
                           (extent =
                               new ExtentReports(@"..\..\TestAddNewCarReport.html", true, DisplayOrder.NewestFirst));
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

            DashboardPage = new DashboardPOM(driver);

            baseURL = Settings.Default.BaseURL;
            verificationErrors = new StringBuilder();

            //// maximize window
            DashboardPage.MaximizeWindow();

            //Set implicit wait
            DashboardPage.setImplicitWait(30);

            // Save all settings
            Settings.Default.Save();

            extent.Config()
                    .DocumentTitle("Add Car Report")
                    .ReportName("Add Car")
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
        public void A_TheCancelAddCarTest()
        {
            try
            {
                test = extent.StartTest("Cancel Add Car Test", "Test to attempt adding a car and cancel before completion")
                                .AssignCategory(Settings.Default.Driver); 
                //Got to base URL
                DashboardPage.Goto(baseURL);

                //Login
                DashboardPage.doLogin(Settings.Default.User, Settings.Default.Password);

                //Test Steps go here
                DashboardPage.waitForSpinnerDashboard();
                DashboardPage.waitForPlansToLoadDashboard();
                DashboardPage.clickAddNewCar();
                DashboardPage.waitForSpinnerDashboard();
                DashboardPage.addVIN(Settings.Default.AddedVIN);

                /* Wait required so element can be clicked once pop-up animation is complete */
                Thread.Sleep(1000); 
                IWebElement modal = driver.FindElement(By.ClassName("reveal-modal-bg"));
                DashboardPage.waitUntilElementIsDisplayed(modal);

                DashboardPage.doYouWantToAddThisVehicle("N");

                /* Wait required so element can be clicked once pop-up animation is complete */
                Thread.Sleep(1000);
                DashboardPage.waitUntilElementNotDisplayed(modal);
                DashboardPage.goBackDashboard();

                //Any assertions if requried
                DashboardPage.assertPageTitle(driver.Title, Settings.Default.TitleDashboard);
                Console.WriteLine("Expected page displayed after cancelling adding the VIN");

                //Take screenshot after Test
                DashboardPage.waitUntilTitle(Settings.Default.TitleDashboard);
                DashboardPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "CancelAddCarResults.png");
                test.Log(LogStatus.Pass, "Screenshot below: " + test.AddScreenCapture("Screenshots/CancelAddCarResults.png"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                DashboardPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "CancelAddCarError.png");
                test.Log(LogStatus.Fail, "Fail: " + e.Message + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/CancelAddCarError.png"));
                throw;
            }
        }

        [Test,Explicit]
        public void B_TheAddCarTest()
        {
            try
            {
                test = extent.StartTest("Add Car Test", "Test to add new car.")
                                .AssignCategory(Settings.Default.Driver);
                //Got to base URL
                DashboardPage.Goto(baseURL);

                //Login
                DashboardPage.doLogin(Settings.Default.User, Settings.Default.Password);

                //Test Steps go here
                DashboardPage.waitForSpinnerDashboard();
                DashboardPage.waitForPlansToLoadDashboard();
                DashboardPage.clickAddNewCar();
                DashboardPage.waitForSpinnerDashboard();
                DashboardPage.addVIN(Settings.Default.AddedVIN);

                DashboardPage.doYouWantToAddThisVehicle("Y");

                //Any assertions if requried


                //Take screenshot after Test
                Thread.Sleep(1000);
                DashboardPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "AddCarResults.png");
                test.Log(LogStatus.Pass, "Screenshot below: " + test.AddScreenCapture("Screenshots/AddCarResults.png"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                DashboardPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "AddCarError.png");
                test.Log(LogStatus.Fail, "Fail: " + e.Message + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/AddCarError.png"));
                throw;
            }
        }

        [Test,Explicit]
        public void C_TheCancelRemoveCarTest()
        {
            try
            {
                test = extent.StartTest("Cancel Remove Car Test", "Test to cancel an attempt to remove a car.")
                .AssignCategory(Settings.Default.Driver);
                //Got to base URL
                DashboardPage.Goto(baseURL);

                //Login
                DashboardPage.doLogin(Settings.Default.User, Settings.Default.Password);

                //Test Steps go here
                DashboardPage.waitForSpinnerDashboard();
                DashboardPage.waitForPlansToLoadDashboard();
                DashboardPage.focusCar(Settings.Default.AddedVIN);
                Thread.Sleep(1000);
                DashboardPage.removeCar();
                Thread.Sleep(1000);
                DashboardPage.doYouWantToRemoveThisVehicle("No");


                //Any assertions if requried

                //Take screenshot after Test
                Thread.Sleep(1000);
                DashboardPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "CancelRemoveCarResults.png");
                test.Log(LogStatus.Pass, "Screenshot below: " + test.AddScreenCapture("Screenshots/CancelRemoveCarResults.png"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                DashboardPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "CancelRemoveCarError.png");
                test.Log(LogStatus.Fail, "Fail: " + e.Message + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/CancelRemoveCarError.png"));
                throw;
            }
        }

        [Test,Explicit]
        public void D_TheRemoveCarTest()
        {
            try
            {
                test = extent.StartTest("Remove Car Test", "Test to remove a car.")
                .AssignCategory(Settings.Default.Driver);
                //Got to base URL
                DashboardPage.Goto(baseURL);

                //Login
                DashboardPage.doLogin(Settings.Default.User, Settings.Default.Password);

                //Test Steps go here
                DashboardPage.waitForSpinnerDashboard();
                DashboardPage.waitForPlansToLoadDashboard();
                DashboardPage.focusCar(Settings.Default.AddedVIN);
                Thread.Sleep(1000);
                DashboardPage.removeCar();
                DashboardPage.doYouWantToRemoveThisVehicle("YES");

                //Any assertions if requried

                //Take screenshot after Test
                Thread.Sleep(1000);
                DashboardPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "RemoveCarResults.png");
                test.Log(LogStatus.Pass, "Screenshot below: " + test.AddScreenCapture("Screenshots/RemoveCarResults.png"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                DashboardPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "RemoveCarError.png");
                test.Log(LogStatus.Fail, "Fail: " + e.Message + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/RemoveCarError.png"));
                throw;
            }
        }

        [Test]
        public void E_TheAddAlreadyRegisteredCarTest()
        {
            try
            {
                test = extent.StartTest("Add  Already Registered Car Test", "Test to attempt adding a car that is already registered on account.")
                                .AssignCategory(Settings.Default.Driver);
                //Got to base URL
                DashboardPage.Goto(baseURL);

                //Login
                DashboardPage.doLogin(Settings.Default.User, Settings.Default.Password);

                //Test Steps go here
                DashboardPage.waitForSpinnerDashboard();
                DashboardPage.waitForPlansToLoadDashboard();
                DashboardPage.clickAddNewCar();
                DashboardPage.waitForSpinnerDashboard();
                DashboardPage.addVIN(Settings.Default.CurrentVIN);

                /* Wait required so element can be clicked once pop-up animation is complete */
                Thread.Sleep(1000);
                IWebElement modal = driver.FindElement(By.ClassName("reveal-modal-bg"));
                DashboardPage.waitUntilElementIsDisplayed(modal);

                //Any assertions if requried
                //Assert warning pop up contains correct text.
                DashboardPage.assertAlreadyRegisterdCar();

                Console.WriteLine("Expected Alert text displayed after adding the registered VIN");
                outputText = "Expected Alert text displayed after adding the registered VIN";

                //Take screenshot after Test
                DashboardPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "AddRegisteredCarResults.png");
                test.Log(LogStatus.Pass, outputText + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/AddRegisteredCarResults.png"));

                DashboardPage.closeAddVINAlert();
                outputText = "Alert Pop up closed.";

                //Take screenshot after Test
                Thread.Sleep(1000);
                DashboardPage.waitUntilTitle(Settings.Default.TitleDashboard);
                DashboardPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "AddRegisteredCarResults2.png");
                test.Log(LogStatus.Pass, outputText + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/AddRegisteredCarResults2.png"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                DashboardPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "AddRegisteredCarError.png");
                test.Log(LogStatus.Fail, "Fail: " + e.Message + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/AddRegisteredCarError.png"));
                throw;
            }
        }

        [Test]
        public void F_TheAddCarEmptyVINTest()
        {
            try
            {
                test = extent.StartTest("Add Car Empty VIN Test", "Test to attempt adding a car using empty VIN field.")
                                .AssignCategory(Settings.Default.Driver);
                //Got to base URL
                DashboardPage.Goto(baseURL);

                //Login
                DashboardPage.doLogin(Settings.Default.User, Settings.Default.Password);

                //Test Steps go here
                DashboardPage.waitForSpinnerDashboard();
                DashboardPage.waitForPlansToLoadDashboard();
                DashboardPage.clickAddNewCar();
                DashboardPage.waitForSpinnerDashboard();
                DashboardPage.addVIN("");

                //Any assertions if requried
                //Assert error validation message contains correct text.
                DashboardPage.assertAddCarEmptyVIN();

                Console.WriteLine("Expected error validation text displayed after adding empty VIN");
                outputText = "Expected error validation text displayed after adding empty VIN";

                //Take screenshot after Test
                Thread.Sleep(1000);
                DashboardPage.waitUntilTitle(Settings.Default.TitleDashboard);
                DashboardPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "AddCarEmptyVINResults.png");
                test.Log(LogStatus.Pass, outputText + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/AddCarEmptyVINResults.png"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                DashboardPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "AddCarEmptyVINError.png");
                test.Log(LogStatus.Fail, "Fail: " + e.Message + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/AddCarEmptyVINError.png"));
                throw;
            }
        }

        [Test]
        public void G_TheAddCarInvalidVINTest()
        {
            try
            {
                test = extent.StartTest("Add Car Invalid VIN Test", "Test to attempt adding a car using Invalid VIN field.")
                                .AssignCategory(Settings.Default.Driver);
                //Got to base URL
                DashboardPage.Goto(baseURL);

                //Login
                DashboardPage.doLogin(Settings.Default.User, Settings.Default.Password);

                //Test Steps go here
                DashboardPage.waitForSpinnerDashboard();
                DashboardPage.waitForPlansToLoadDashboard();
                DashboardPage.clickAddNewCar();
                DashboardPage.waitForSpinnerDashboard();
                DashboardPage.addVIN("12345678910111213");


                IWebElement errorPopup = driver.FindElement(By.XPath("//*[@id='server-error']"));
                DashboardPage.waitUntilElementIsDisplayed(errorPopup);

                //Any assertions if requried
                //Assert error validation message contains correct text.
                DashboardPage.assertAddCarInvalidVIN();

                Console.WriteLine("Expected Error text displayed after adding invalid VIN");
                outputText = "Expected Error text displayed after adding invalid VIN";

                //Take screenshot after Test
                Thread.Sleep(1000);
                DashboardPage.waitUntilTitle(Settings.Default.TitleDashboard);
                DashboardPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "AddCarInvalidVINResults.png");
                test.Log(LogStatus.Pass, outputText + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/AddCarInvalidVINResults.png"));

                //tmp: Click OK to error pop-up
                driver.FindElement(By.XPath("//*[@id='close-error-button']")).Click();

                DashboardPage.waitUntilElementNotDisplayed(errorPopup);
                outputText = "Error Pop up closed.";


                Thread.Sleep(1000);
                DashboardPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "AddCarInvalidVINResults2.png");
                test.Log(LogStatus.Pass, outputText + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/AddCarInvalidVINResults2.png"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                DashboardPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "AddCarInvalidVINError.png");
                test.Log(LogStatus.Fail, "Fail: " + e.Message + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/AddCarInvalidVINError.png"));
                throw;
            }
        }

        [Test, Explicit]
        public void H_TheCancelRemoveSIMOwnerTest()
        {
            try
            {
                test = extent.StartTest("Cancel Remove SIM Owner Test", "Test to cancel remove SIM owner.")
                .AssignCategory(Settings.Default.Driver);
                //Got to base URL
                DashboardPage.Goto(baseURL);

                //Login
                DashboardPage.doLogin(Settings.Default.User, Settings.Default.Password);

                //Test Steps go here
                DashboardPage.waitForSpinnerDashboard();
                DashboardPage.waitForPlansToLoadDashboard();

               // DashboardPage.focusCar(Settings.Default.CurrentVIN);
                //System.Threading.Thread.Sleep(1000);
                //DashboardPage.waitForPlansToLoadDashboard(Settings.Default.AddedVIN);

                DashboardPage.removeSimOwner();

                //DashboardPage.yesToRemoveSIMOwnerPopUp();

                DashboardPage.cancelToRemoveSIMOwnerPopUp();

                //Any assertions if requried

                //Take screenshot after Test
                Thread.Sleep(1000);
                DashboardPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "CancelRemoveSIMOwnerResults.png");
                test.Log(LogStatus.Pass, "Screenshot below: " + test.AddScreenCapture("Screenshots/CancelRemoveSIMOwnerResults.png"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                DashboardPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "CancelRemoveSIMOwnerError.png");
                test.Log(LogStatus.Fail, "Fail: " + e.Message + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/CancelRemoveSIMOwnerError.png"));
                throw;
            }
        }

    }
}
