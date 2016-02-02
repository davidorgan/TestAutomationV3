using NUnit.Framework;
using System;
using System.Text;
using RelevantCodes.ExtentReports;

namespace TestFramework
{
    [TestFixture]
    public class FavouriteCar : DashboardPOM
    {
        public DashboardPOM FavouriteCarPage;

        public class ExtentManager
        {
            private static ExtentReports extent;

            public static ExtentReports Instance
            {
                get {
                    return extent ??
                           (extent =
                               new ExtentReports(@"..\..\TestFavouriteCarReport.html", true, DisplayOrder.NewestFirst));
                }
            }
        }
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

            FavouriteCarPage = new DashboardPOM(driver);

            baseURL = Settings.Default.BaseURL;
            verificationErrors = new StringBuilder();

            //// maximize window
            FavouriteCarPage.MaximizeWindow();

            //Set implicit wait
            FavouriteCarPage.setImplicitWait(20);


            extent.Config()
                   .DocumentTitle("Favourite Car Report")
                   .ReportName("Favourite Car")
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
        public void B_TheFavouriteCarTest()
        {
            try
            {
                test = extent.StartTest("Favourite Car Test", "Test the car is set as favourite car.")
               .AssignCategory(Settings.Default.Driver);
                //Got to base URL
                FavouriteCarPage.Goto(baseURL);

                //Login
                FavouriteCarPage.doLogin();

                //Test Steps go here
                FavouriteCarPage.waitForSpinnerDashboard();
                FavouriteCarPage.waitForPlansToLoadDashboard();

                bool favSet = FavouriteCarPage.assertFavouriteCarToggle(Settings.Default.CurrentVIN);
                if (!favSet)
                {
                    FavouriteCarPage.toggleFavouriteCar(Settings.Default.CurrentVIN);
                    FavouriteCarPage.waitForSpinnerDashboard();
                }

                FavouriteCarPage.hoverFavCar();
                //Any assertions if requried

                //Cannot currently assert favourite car pop-up due to dynamically created id!
                /*bool isPopup = FavouriteCarPage.assertFavCarPopUp();
                if (isPopup)
                {
                    IWebElement popup = driver.FindElement(By.XPath("//img[@src='/Content/themes/audi/assets/icons/favourite-car-active.png']/following-sibling::div[@class='extra-info-data']"));
                    Console.WriteLine("Found after toggle: "+popup.Text);
                }
                else
                {

                    throw new Exception("Pop up not found");
                }*/

                outputText = "Favourite Car set as expected with Favourite car image and pop-up text displayed.";

                //Take screenshot after Test
                System.Threading.Thread.Sleep(1000);
                FavouriteCarPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "FavouriteCarResults.png");
                test.Log(LogStatus.Pass, outputText + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/FavouriteCarResults.png"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                FavouriteCarPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "FavouriteCarError.png");
                test.Log(LogStatus.Fail, "Fail: " + e.Message + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/FavouriteCarError.png"));
                throw;
            }
        }

        [Test]
        public void A_TheUnFavouriteCarTest()
        {
            try
            {
                test = extent.StartTest("Un-Favourite Car Test", "Test the car is no longer set as favourite car.")
.AssignCategory(Settings.Default.Driver);
                //Got to base URL
                FavouriteCarPage.Goto(baseURL);

                //Login
                FavouriteCarPage.doLogin();

                //Test Steps go here
                FavouriteCarPage.waitForPlansToLoadDashboard();

                bool favSet = FavouriteCarPage.assertFavouriteCarToggle(Settings.Default.CurrentVIN);
                if (!favSet)
                {
                    bool imgDisplayed = FavouriteCarPage.assertFavCarImage();

                    //Any assertions if requried
                    if (!imgDisplayed)
                    {
                        Console.WriteLine("No Favourite car image as expected");
                        outputText = "No Favourite car image as expected";
                    }
                    else {
                        throw new Exception("The favourite car image is displayed unexpectedly; Favourite car toggle is not set");
                    }
                }
                else
                {
                    FavouriteCarPage.toggleFavouriteCar(Settings.Default.CurrentVIN);
                    FavouriteCarPage.waitForSpinnerDashboard();
                    bool imgDisplayed = FavouriteCarPage.assertFavCarImage();

                    //Any assertions if requried
                    if (!imgDisplayed)
                    {
                        Console.WriteLine("No Favourite car image as expected");
                        outputText = "No Favourite car image as expected";
                    }
                    else
                    {
                        throw new Exception("The favourite car image is displayed unexpectedly; Favourite car toggle is not set");
                    }
                }

                //Take screenshot after Test
                System.Threading.Thread.Sleep(1000);
                FavouriteCarPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "UnFavouriteCarResults.png");
                test.Log(LogStatus.Pass, outputText + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/UnFavouriteCarResults.png"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                FavouriteCarPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "UnFavouriteCarError.png");
                test.Log(LogStatus.Fail, "Fail: " + e.Message + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/UnFavouriteCarError.png"));
                throw;
            }
        }

    }
}