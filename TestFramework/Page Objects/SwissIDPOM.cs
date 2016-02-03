using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using System.Drawing.Imaging;
using OpenQA.Selenium.Support.UI;
using System.Data.SqlClient;
using OpenQA.Selenium.Interactions;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;
using OpenQA.Selenium.Remote;


namespace TestFramework
{

    /// <summary>
    /// POM for Swiss ID Process page.
    /// </summary>
    public class SwissIDPOM : DashboardPOM
    {
       
        string frontFilePathElementID;
        string backFilePathElementID;
        string frontImgPath = "C:/Users/DavidO/Documents/Accept - Test examples/Lukasz Id.jpg";
        string backImgPath = "C:/Users/DavidO/Documents/Accept - Test examples/Lukasz id 2.jpg";

        /// <summary>
        /// Initializes a new instance of the <see cref="SwissIDPOM"/> class.
        /// </summary>
        protected SwissIDPOM() { }


        /// <summary>
        /// Initializes a new instance of the <see cref="SwissIDPOM" /> class.
        /// </summary>
        /// <param name="driver">The driver.</param>
        public SwissIDPOM(IWebDriver driver)
        {
            this.driver = driver;
            this.currentAccount = AccountHelper.accountDetails.setAccountDetails();

            this.frontFilePathElementID = "file-front-page-path-" + this.currentAccount.vin;
            this.backFilePathElementID = "file-back-page-path-" + this.currentAccount.vin;
        }


        ///-- Web Page Elements --///
        ///-- ________________ --///

        //Upload Doc Elements
        private IWebElement chooseFileFront_Button { get { return driver.FindElement(By.XPath("//*[@id='upload-form-" + this.currentAccount.vin + "']/div[1]/div[1]/span")); } }
        private IWebElement chooseFileBack_Button { get { return driver.FindElement(By.XPath("//*[@id='upload-form-" + this.currentAccount.vin + "']/div[2]/div[1]/span")); } }
        private IWebElement submitImageUpload_Button { get { return driver.FindElement(By.XPath("//*[@id='upload-form-" + this.currentAccount.vin + "']/div[3]/input")); } }
    
    
        ///-- Web Page Actions --///
        ///-- ________________ --///
        

        public void chooseFrontImage()
        {
            Helpers.FileUploadSelector.FileSelector("//*[@id='upload-form-" + this.currentAccount.vin + "']/div[1]/div[1]/span", frontImgPath, this.driver);
        }

        public void chooseBackImage()
        {
            Helpers.FileUploadSelector.FileSelector("//*[@id='upload-form-" + this.currentAccount.vin + "']/div[2]/div[1]/span", backImgPath, this.driver);
        }

        public void submitImageUpload()
        {
            submitImageUpload_Button.Click();
        }

    }
}
