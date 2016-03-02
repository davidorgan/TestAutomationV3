using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Threading;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;

namespace TestFramework
{
    /// <summary>
    /// The page object model for the Dashboard extends Base page object model
    /// </summary>
    public class DashboardPOM : BasePageObject
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardPOM"/> class.
        /// </summary>
        protected DashboardPOM() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardPOM"/> class.
        /// </summary>
        /// <param name="driver">The driver.</param>
        public DashboardPOM(IWebDriver driver)
        {
            this.driver = driver;
            this.currentAccount = AccountHelper.accountDetails.setAccountDetails();
        }

        ///--Web Page Elements--///
        //Add Car Elements
        private IWebElement addCar_Link { get { return driver.FindElement(By.Id("add-vehicle-trigger")); } }
        private IWebElement addCarVIN_Input { get { return driver.FindElement(By.Id("inputVin")); } }
        private IWebElement addCarSubmit_Button { get { return driver.FindElement(By.XPath("//input[@value='Continue']")); } }
        private IWebElement addCarPopUp_Yes_Button { get { return driver.FindElement(By.XPath("//*[@id='form0']/div/div/ul/li[1]/input")); } }
        private IWebElement addCarPopUp_No_Button { get { return driver.FindElement(By.Id("wrongCar")); } }
        private IWebElement addCarAlreadyRegisteredWarning_Container { get { return driver.FindElement(By.XPath("//*[@id='myModal']/div[2]")); } }
        private IWebElement addCarWarningPopUpClose_Link { get { return driver.FindElement(By.XPath("//*[@id='myModal']/div[2]/a")); } }
        private IWebElement addCarValidationError_Span { get { return driver.FindElement(By.XPath("//*[@id='inputVin-error']")); } }
        private IWebElement addCarErrorPopUp_Span { get { return driver.FindElement(By.XPath("//*[@id='errors-summary']")); } }
        private IWebElement addCarErrorPopUpClose_Button { get { return driver.FindElement(By.Id("close-error-button")); } }

        //Remove Car Elements
        private IWebElement removeAddedCar_Link { get { return driver.FindElement(By.XPath("//*[@id='vehicle-dashboard-"+Settings.Default.AddedVIN+"']/div[2]/div/input")); } }
        private IWebElement removeCarPopUp_Yes_Button { get { return driver.FindElement(By.XPath("//*[@id='form7']/div/div/ul/li[1]/input")); } }
        private IWebElement removeCarPopUp_No_Button { get { return driver.FindElement(By.XPath("//*[@id='form7']/div/div/ul/li[2]/input")); } }
        private IWebElement removeSIMOwner_Link(string vin) { return driver.FindElement(By.XPath("//*[@id='vehicle-dashboard-" + vin + "']/div[2]/div/div[8]/input")); }
        private IWebElement removeSIMOwnerPopUp_Yes_Button { get { return driver.FindElement(By.XPath("//*[@id='form4']/div/div/ul/li[1]/input")); } }
        private IWebElement removeSIMOwnerPopUp_Cancel_Button { get { return driver.FindElement(By.XPath("//*[@id='form4']/div/div/ul/li[2]/input")); } }
        private IWebElement removeSimOwner_Modal(string vin) { return driver.FindElement(By.XPath("//*[@id='modal-remove-user-liable-"+vin+"']")); }

        //General Elements
        private IWebElement reloadPlan_Icon { get { return driver.FindElement(By.XPath("//div[2]/div/div/div/div/div[2]/div/div/img")); } }
        private IWebElement carTab(string vin) { return driver.FindElement(By.XPath("//div[@data-id='" + vin + "']")); }
        private IWebElement back_Button { get { return driver.FindElement(By.LinkText("Back")); } }
        
        //Purchase Plan Elememts
        private IWebElement topUp_Button { get { return driver.FindElement(By.LinkText("Top up")); } }
        private IWebElement nationalPlan_Dropdown { get { return driver.FindElement(By.Id("ui-id-1")); } }
        private IWebElement nationalPlanFirst_Link { get { return driver.FindElement(By.XPath("//form[@id='form0']/label/div/div[2]")); } }
        private IWebElement europePlan_Dropdown { get { return driver.FindElement(By.Id("ui-id-3")); } }
        private IWebElement europePlanFirst_Link { get { return driver.FindElement(By.XPath("//form[@id='form6']/label/div/div[2]")); } }
        private IWebElement purchasePlanSubmit_Button { get { return driver.FindElement(By.Id("completePurchase")); } }

        //Credit Card Elements
        private IWebElement selectPaymentCard_Dropdown { get { return driver.FindElement(By.Id("PaymentCardDetails_RegisteredPaymentCards_SelectedPaymentCardId")); } }
        private IWebElement selectDifferentCard_Link { get { return driver.FindElement(By.XPath("//*[@id='add-new-card-button_"+Settings.Default.CurrentVIN+"']")); } }
        private IWebElement useSavedCard_Link { get { return driver.FindElement(By.XPath("//*[@id='use-saved-card-button_" + Settings.Default.CurrentVIN + "']")); } }

        private IWebElement cardHolderName_InputField { get { return driver.FindElement(By.Id("PaymentCardDetails_AddPaymentCard_CardHolderName")); } }
        private IWebElement cardType_Dropdown { get { return driver.FindElement(By.Id("PaymentCardDetails_AddPaymentCard_CardType")); } }
        private IWebElement cardNumber_InputField { get { return driver.FindElement(By.Id("PaymentCardDetails_AddPaymentCard_CardNumber")); } }
        private IWebElement cardVerificationCode_InputField { get { return driver.FindElement(By.Id("PaymentCardDetails_AddPaymentCard_CardVerificationCode")); } }
        private IWebElement cardExpireMonth_Dropdown { get { return driver.FindElement(By.Id("PaymentCardDetails_AddPaymentCard_ExpireMonth")); } }
        private IWebElement cardExpireYear_Dropdown { get { return driver.FindElement(By.Id("PaymentCardDetails_AddPaymentCard_ExpireYear")); } }
        private IWebElement saveCard_CheckBox { get { return driver.FindElement(By.XPath("//*[@id='new-credit-card-form_" + Settings.Default.CurrentVIN + "']/div/div[3]/label")); } }

        private IWebElement cardHolderNameError_Span { get { return driver.FindElement(By.XPath("//*[@id='new-credit-card-form_"+Settings.Default.CurrentVIN+"']/div/div/div[1]/div[1]/span")); } }
        private IWebElement cardNummberError_Span { get { return driver.FindElement(By.XPath("//*[@id='new-credit-card-form_" + Settings.Default.CurrentVIN + "']/div/div/div[1]/div[3]/span")); } }
        private IWebElement cardCVVError_Span { get { return driver.FindElement(By.XPath("//*[@id='new-credit-card-form_" + Settings.Default.CurrentVIN + "']/div/div/div[1]/div[4]/span")); } }
        private IWebElement cardExpireMonthError_Span { get { return driver.FindElement(By.XPath("//*[@id='new-credit-card-form_" + Settings.Default.CurrentVIN + "']/div/div/div[2]/div/ul/li[1]/span")); } }
        private IWebElement cardTooManyAttempts_Button { get { return driver.FindElement(By.Id("close-error-button")); } }
        private IWebElement cardTooManyAttemptsWarning_Span { get { return driver.FindElement(By.XPath("//*[@id='errors-summary']/ul/li")); } }

        //Favourite Car Elements
        private IWebElement favouriteCar_Toggle(string vin){return driver.FindElement(By.XPath("//input[@id='set-favourite-"+ vin +"']"));} 
        private IWebElement favouriteCarToggle_Label(string vin) { return driver.FindElement(By.XPath("//*[@id='vehicle-dashboard-"+vin+"']/div[2]/div/div[5]/div[5]/div/label")); } 
        private IWebElement favouriteCar_Img { get { return driver.FindElement(By.XPath("//img[@src='/Content/themes/audi/assets/icons/favourite-car-active.png']")); } }
        private IWebElement favouriteCar_ToolTip { get { return driver.FindElement(By.XPath("/html/body/descendant::span[text()='My Favorite Car']")); } }

        //Pet Name Elements
        private IWebElement changePetName_Link { get { return driver.FindElement(By.XPath("//*[@id='form2']/label/span")); } }
        private IWebElement changePetName_InputField { get { return driver.FindElement(By.XPath("//*[@id='PersonalizedCarName']")); } }
        private IWebElement changePetNameSubmit_Button { get { return driver.FindElement(By.Id("submit-form")); } }
        private IWebElement currentPetName_Span { get { return driver.FindElement(By.XPath("//*[@id='device-triggers-list']/div/div/div/div/div[1]/label")); } }

        //Purchase History Elements
        private IWebElement purchaseHistory_Link { get { return driver.FindElement(By.XPath("//*[@id='form1']/label/span")); } }
        private IWebElement purchaseHistoryFromDate_InputField { get { return driver.FindElement(By.Id("PurchaseHistoryFrom")); } }
        private IWebElement purchaseHistoryToDate_InputField { get { return driver.FindElement(By.Id("PurchaseHistoryTo")); } }
        private IWebElement purchaseHistorySubmit_Button { get { return driver.FindElement(By.XPath("//*[@id='form0']/div[2]/div/input")); } }
        private IWebElement purchaseHistoryFromError_Span { get { return driver.FindElement(By.XPath("//*[@id='form0']/div[2]/div/div[1]/div/span")); } }
        private IWebElement purchaseHistoryToError_Span { get { return driver.FindElement(By.XPath("//*[@id='form0']/div[2]/div/div[2]/div/span")); } }
        private IWebElement purchaseHistoryFirstEntryInvoice_Link { get { return driver.FindElement(By.XPath("//*[@id='p0']/div[1]/div/a")); } }
        private IWebElement purchaseHistoryFirstEntryDate_Span { get { return driver.FindElement(By.XPath("//*[@id='p0']/div[1]/div/span[1]")); } }
        private IWebElement purchaseHistoryFirstEntryPlanName_Span { get { return driver.FindElement(By.XPath("//*[@id='p0']/div[1]/div/span[2]")); } }
        private IWebElement purchaseHistoryFirstEntryPurchaseBy_Span { get { return driver.FindElement(By.XPath("//*[@id='p0']/div[1]/div/span[3]")); } }
        private IWebElement purchaseHistoryFirstEntryPrice_Span { get { return driver.FindElement(By.XPath("//*[@id='p0']/div[1]/div/span[4]")); } }

        //Manage In Car Top Up Elements
        private IWebElement manageInCarTopUp_Link { get { return driver.FindElement(By.XPath("//*[@id='form0']/label/span")); } }
        private IWebElement manageInCarTopUpToggle_Input { get { return driver.FindElement(By.XPath("//*[@id='toggle-top-up-"+Settings.Default.CurrentVIN+"']")); } }
        private IWebElement manageInCarTopUpToggle_Label { get { return driver.FindElement(By.XPath("//*[@id='form0']/div/div/div[1]/div[2]/div/label")); } }
        private IWebElement manageInCarTopUpSelectCard_Dropdown { get { return driver.FindElement(By.Id("RegisteredPaymentCards_SelectedPaymentCardId")); } }
        private IWebElement manageInCarTopUpPhone_InputField { get { return driver.FindElement(By.XPath("//*[@id='mobile-number-"+Settings.Default.CurrentVIN+"']")); } }
        private IWebElement manageInCarTopUpLowBalance_Dropdown { get { return driver.FindElement(By.Id("SponsorUserDetails_SendLowBalanceNotification")); } }
        private IWebElement manageInCarTopUpLimit_Dropdown { get { return driver.FindElement(By.Id("SponsorUserDetails_TopUpLimit_SelectedAmount")); } }
        private IWebElement manageInCarTopUpEnable_Button { get { return driver.FindElement(By.Id("submit-in-car-top-up-"+Settings.Default.CurrentVIN)); } }
        private IWebElement manageInCarTopUpUpdate_Button { get { return driver.FindElement(By.Id("update-in-car-top-up-" + Settings.Default.CurrentVIN)); } }
        private IWebElement manageInCarTopUpDisable_Button { get { return driver.FindElement(By.Id("disable-in-car-top-up-" + Settings.Default.CurrentVIN)); } }
        private IWebElement manageInCarTopUp_Img(string vin) { return driver.FindElement(By.XPath("//*[@id='dashboard-side-vehicle-"+vin+"']/div/div[2]/div/div[2]/img"));}
        private IWebElement manageInCarTopUpError_Span(string vin) { return driver.FindElement(By.XPath("//*[@id='top-up-details-"+vin+"']/span[2]")); }


        /// <summary>
        /// Does the add new car.
        /// </summary>
        /// <param name="vin">The vin.</param>
        /// <param name="valid">The valid.</param>
        [Obsolete("Advise using individual functionality methods below over this method")]
        public void doAddNewCar(string vin, ConfirmAddVehicle AddV)
        {
            addCar_Link.Click();
            addCarVIN_Input.Clear();
            addCarVIN_Input.SendKeys(vin);
            addCarSubmit_Button.Click();
            Thread.Sleep(1000);
            if (AddV.Equals(ConfirmAddVehicle.Yes))
            {
                addCarPopUp_Yes_Button.Click();
            }
            else
            {
                addCarPopUp_No_Button.Click();
            }
            
        }

        /// <summary>
        /// Clicks the add new car.
        /// </summary>
        public void clickAddNewCar()
        {
            addCar_Link.Click();
        }

        /// <summary>
        /// Adds the vin.
        /// </summary>
        /// <param name="vin">The vin.</param>
        public void addVIN(string vin)
        {
            addCarVIN_Input.Clear();
            addCarVIN_Input.SendKeys(vin);
            addCarSubmit_Button.Click();
        }


        /// <summary>
        /// Do you want to add this vehicle.
        /// </summary>
        /// <param name="AddV">The add v.</param>
        /// <exception cref="System.Exception">
        /// Could not find link with JS click. + e.Message
        /// or
        /// Confirm/Cancel Add Vehicle step failed.
        /// </exception>
        public void doYouWantToAddThisVehicle(ConfirmAddVehicle AddV)
        {
            if (AddV.Equals(ConfirmAddVehicle.Yes))
            {
                IWebElement Link = addCarPopUp_Yes_Button;
                JavaScriptClick(Link);

            }
            else if (AddV.Equals(ConfirmAddVehicle.No))
            {
                try
                {
                    IWebElement Link = addCarPopUp_No_Button;
                    JavaScriptClick(Link);
                }
                catch (Exception e)
                {
                    throw new Exception("Could not find link with JS click. " + e.Message);
                }
            }
            else
            {
                throw new Exception("Confirm/Cancel Add Vehicle step failed.");
            }
        }

        /// <summary>
        /// Focuses the car. I.e. Clicks the car with expected vin on left hand side list of cars.
        /// </summary>
        /// <param name="vin">The vin.</param>
        public void focusCar(string vin)
        {
            Console.WriteLine("//div[@data-id='" + vin + "']");
            carTab(vin).Click();
        }

        /// <summary>
        /// Removes the car.
        /// </summary>
        public void removeCar()
        {
            removeAddedCar_Link.Click();          
        }

        /// <summary>
        /// Answers the 'Do you want to remove this vehicle' pop up.
        /// </summary>
        /// <param name="yesno">The yesno.</param>
        public void doYouWantToRemoveThisVehicle(string yesno)
        {
            yesno = yesno.ToLower();
            if (yesno.Equals("yes") || yesno.Equals("y"))
            {
                IWebElement Link = removeCarPopUp_Yes_Button;
                JavaScriptClick(Link);
            }
            else if (yesno.Equals("no") || yesno.Equals("n"))
            {
                IWebElement Link = removeCarPopUp_No_Button;
                JavaScriptClick(Link);
            }
        }

        /// <summary>
        /// Reloads the plan.
        /// </summary>
        public void ReloadPlan()
        {
            waitForPlansToLoadDashboard();
            reloadPlan_Icon.Click();
        }

        /// <summary>
        /// Goes back to dashboard.
        /// </summary>
        public void goBackDashboard()
        {
            back_Button.Click();
        }

        /// <summary>
        /// Toggles the favourite car.
        /// </summary>
        public void toggleFavouriteCar(string vin)
        {
            favouriteCarToggle_Label(vin).Click();
        }

        /// <summary>
        /// Asserts the favourite car toggle is set.
        /// </summary>
        /// <param name="vin">The vin.</param>
        /// <returns></returns>
        public bool assertFavouriteCarToggle(string vin)
        {
            IWebElement toggle = favouriteCar_Toggle(vin);
            if (toggle.Selected)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Asserts the fav car image.
        /// </summary>
        /// <returns></returns>
        public bool assertFavCarImage()
        {
            try
            {
                bool isDisplayed = favouriteCar_Img.Displayed;
                if (isDisplayed)
                {
                    return true;
                }
                return false;
            }
            catch (Exception e){          
                Console.WriteLine("Image not found as expected: "+ e.Message);
                return false;
            }
        }

        /// <summary>
        /// Hovers the fav car.
        /// </summary>
        public void hoverFavCar()
        {
            Actions action = new Actions(driver);
            action.MoveToElement(favouriteCar_Img).Perform();
        }

        /// <summary>
        /// Asserts the fav car pop up.
        /// </summary>
        /// <returns></returns>
        public bool assertFavCarPopUp()
        {
            IWebElement popup = favouriteCar_ToolTip;
            Console.WriteLine(popup.Text);
            bool isDisplayed = popup.Displayed;
            if (isDisplayed && popup.Text.Equals("My Favorite Car"))
            {
                return true;
            }
            throw new Exception("Favourite Car top-up text not displayed as expected.");
        }


        //Petname Actions
        /// <summary>
        /// Goes to Change Pet Name.
        /// </summary>
        public void goToPetName()
        {
            //ActionHelper.retryingFindClick(changePetName_Link);
            changePetName_Link.Click();
            //wait for scroll to top
            System.Threading.Thread.Sleep(1000);
        }

        /// <summary>
        /// Changes the name of the pet.
        /// </summary>
        /// <param name="name">The name.</param>
        public void changePetName(string name)
        {
            changePetName_InputField.Clear();
            changePetName_InputField.SendKeys(name);
        }

        /// <summary>
        /// Submits the name of the pet.
        /// </summary>
        public void submitPetName()
        {
            changePetNameSubmit_Button.Click();
        }

        /// <summary>
        /// Back from the name of the pet.
        /// </summary>
        public void backPetName()
        {
            back_Button.Click();
        }

        /// <summary>
        /// Checks the pet name change.
        /// </summary>
        /// <param name="expectedName">The expected name.</param>
        /// <returns></returns>
        public bool checkPetNameChange(string expectedName)
        {
            string currentPetName = currentPetName_Span.Text;
            if(currentPetName.Equals(expectedName))
            {
                return true;
            }
            return false;
        }

        //Purchase History
        /// <summary>
        /// Goes to purchase history dashboard.
        /// </summary>
        public void goToPurchaseHistoryDashboard()
        {
            purchaseHistory_Link.Click();
        }

        /// <summary>
        /// Enters the purchase history dates on dashboard.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        public void enterPurchaseHistoryDatesDashboard(string from, string to)
        {
            purchaseHistoryFromDate_InputField.Clear();
            purchaseHistoryFromDate_InputField.SendKeys(from);
            purchaseHistoryToDate_InputField.Clear();
            purchaseHistoryToDate_InputField.SendKeys(to);
        }

        /// <summary>
        /// Views the purchase history on dashboard.
        /// </summary>
        public void viewPurchaseHistoryDashboard()
        {
            purchaseHistorySubmit_Button.Click();
        }

        public bool assertFromDateErrorPurchaseHistoryDash()
        {
            IWebElement fromValidation = purchaseHistoryFromError_Span;
            string fromValidationstring = fromValidation.Text;
            if (fromValidationstring.Equals(Settings.Default.FromDateErrorPurchaseHistory))
            {
                return true;
            }
            throw new Exception("The from date validation message is not showing the expected text.");
        }

        public bool assertToDateErrorPurchaseHistoryDash()
        {
            IWebElement toValidation = purchaseHistoryToError_Span;
            string toValidationstring = toValidation.Text;
            if (toValidationstring.Equals(Settings.Default.ToDateErrorPurchaseHistory))
            {
                return true;
            }
            throw new Exception("The to date validation message is not showing the expected text.");
        }

        public bool assertPurchaseHistoryDetailsRow()
        {
            string date = purchaseHistoryFirstEntryDate_Span.Text;
            string planName = purchaseHistoryFirstEntryPlanName_Span.Text;
            string purchasedBy = purchaseHistoryFirstEntryPurchaseBy_Span.Text;
            string price = purchaseHistoryFirstEntryPrice_Span.Text;
            string invoiceText = purchaseHistoryFirstEntryInvoice_Link.Text;

            if (!isValidDate(date))
            {
                throw new Exception("Expected Date format not displayed: " + date);
            }

            if (!planName.Equals(Settings.Default.AccountLocale + " 1 Day 50MB") && !planName.Equals("EU Roaming 1 Day 50MB") && !planName.Equals("Europe Border 1 Day 50MB"))
            {
                throw new Exception("Expected Plan Name not displayed. " + planName + " displayed instead");
            }

            if (!purchasedBy.Contains(getCurrentFullName()))
            {
                throw new Exception("Expected Name not displayed.");
            }

            if (!IsValidCurrency(price))
            {
                throw new Exception("Price not displayed correctly.");
            }

            if (!invoiceText.Equals("Invoice"))
            {
                throw new Exception("Invoice Link Text not displayed correctly.");
            }
            return true;
        }

        /// <summary>
        /// Dropdowns the purchase history details on dashboard. &gt;&gt;&gt;No Longer used
        /// </summary>
        [Obsolete]
        public void dropdownPurchaseHistoryDetailsDashboard()
        {
            driver.FindElement(By.XPath("//*[@id='ui-id-1']")).Click();
        }

        /// <summary>q
        /// Cancels the purchase history on dashboard.
        /// </summary>
        public void cancelPurchaseHistoryDashboard()
        {
            back_Button.Click();
        }

        /// <summary>
        /// Back from the purchase history on dashboard.
        /// </summary>
        /// <param name="currentVIN">The current vin.</param>
        public void backPurchaseHistoryDashboard(string currentVIN)
        {
            back_Button.Click();
        }

        /// <summary>
        /// Method only allows actions for Liable/Sponsor account.
        /// </summary>
        /// <param name="doAction">The do action.</param>
        /// <exception cref="System.Exception">The Account type is not known for the user.</exception>
        public void accountLiableAction(Action doAction)
        {
            switch(this.currentAccount.acType)
            {
                case accountType.Sponsor:
                case accountType.Liable:
                {
                    doAction();
                    break;
                }
                case accountType.Standard:
                {
                    Assert.Inconclusive("Account is Standard and therefore should not have access to action");
                    break;
                }
                default:
                {
                    throw new Exception("The Account type is not known for the user.");
                }
            }
        }

        /// <summary>
        /// Method only allows actions for Sponsor account.
        /// </summary>
        /// <param name="doAction">The do action.</param>
        /// <exception cref="System.Exception">The Account type is not known for the user.</exception>
        public void accountSponsorAction(Action doAction)
        {
            switch (this.currentAccount.acType)
            {
                case accountType.Sponsor:
                {
                    doAction();
                    break;
                }
                case accountType.Liable:
                case accountType.Standard:
                {
                    Assert.Inconclusive("Account is Liable/Standard and therefore should not have access to action");
                    break;
                }
                default:
                {
                    throw new Exception("The Account type is not known for the user.");
                }
            }
        }

        //Manage in Car Top Up
        /// <summary>
        /// Goes to in car top up.
        /// </summary>
        public void goToInCarTopUp()
        {             
            manageInCarTopUp_Link.Click();
        }

        /// <summary>
        /// Enables the in car top up toggle.
        /// </summary>
        public void enableInCarTopUpToggle()
        {
            if (!manageInCarTopUpToggle_Input.Selected)
            {
                manageInCarTopUpToggle_Label.Click();
                Thread.Sleep(500);
            }
            else
            {
                Console.WriteLine("In Car Top Up already enabled.");
            }
        }

        public void disableInCarTopUpToggle()
        {
            if (manageInCarTopUpToggle_Input.Selected)
            {
                manageInCarTopUpToggle_Label.Click();
            }
            else
            {
                Console.WriteLine("In Car Top Up already disabled.");
            }
        }

        public void clickDisableInCarTopUp()
        {          
            try
            {
                turnOffImplicitWaits();
                manageInCarTopUpDisable_Button.Click();
            }
            catch (NoSuchElementException)
            {
                turnOnImplicitWaits();
                back_Button.Click();
            }
        }

        public void clickEnableInCarTopUp()
        {
            try
            {
                turnOffImplicitWaits();
                manageInCarTopUpEnable_Button.Click();
            }
            catch (NoSuchElementException)
            {
                turnOnImplicitWaits();               
                manageInCarTopUpUpdate_Button.Click();
            }
        }


        public void clickUpdateSettingsInCarTopUp()
        {
            manageInCarTopUpUpdate_Button.Click();
        }

        public void selectCreditCardInCarTopUp()
        {
            new SelectElement(manageInCarTopUpSelectCard_Dropdown).SelectByIndex(0);
        }

        public void enterNumberInCarTopUp()
        {
            manageInCarTopUpPhone_InputField.Clear();
            manageInCarTopUpPhone_InputField.SendKeys("+353872743926");
        }


        public void enterEmptyNumberInCarTopUp()
        {
            manageInCarTopUpPhone_InputField.Clear();
        }

        public void selectLowBalanceYes()
        {
            new SelectElement(manageInCarTopUpLowBalance_Dropdown).SelectByValue("true");
        }

        public void selectLowBalanceNo()
        {
            new SelectElement(manageInCarTopUpLowBalance_Dropdown).SelectByValue("false");
        }

        public void selectTopUpLimit(string value)
        {
            new SelectElement(manageInCarTopUpLimit_Dropdown).SelectByValue(value);
        }

        public void assertInCarTopUpEnabled()
        {
            string span = manageInCarTopUp_Link.Text;
            if (span.Contains("Manage In-Car Top up: Enabled"))
            {
                if(manageInCarTopUp_Img(Settings.Default.CurrentVIN).Displayed)
                {
                    outputText = "In Car Top Up text and image displays as expected for Enabled.";
                    return;
                }
            }
            throw new Exception("Expected text/image is not displayed for In Car top Up Enabled");
        }

        public void assertInCarTopUpDisabled()
        {
            string span = manageInCarTopUp_Link.Text;
            if (span.Contains("Manage In-Car Top up: Disabled"))
            {
                return;
            }
            throw new Exception("Expected text is not displayed for In Car top Up Disabled");
        }

        public void assertInCarTopUpErrorPhone()
        {
            string span = manageInCarTopUpError_Span(Settings.Default.CurrentVIN).Text;
            if (span.Contains("The mobile number field is required when the low balance notification is active"))
            {
                return;
            }
            throw new Exception("Error validation span not containing expected text.");
        }

        /// <summary>
        /// Goes to top up.
        /// </summary>
        public void goToTopUp()
        {
            topUp_Button.Click();
        }

        /// <summary>
        /// Selects the local plan drop down.
        /// </summary>
        public void selectLocalPlanDropDown()
        {
            Thread.Sleep(1000);
            nationalPlan_Dropdown.Click();
        }

        /// <summary>
        /// Selects 1 day plan.
        /// </summary>
        public void select1DayPlan()
        {
            Thread.Sleep(1000);
            nationalPlanFirst_Link.Click();
        }

        /// <summary>
        /// Submits the purchase plan.
        /// </summary>
        public void submitPurchasePlan()
        {
            purchasePlanSubmit_Button.Click();
        }

        public void selectEuropePlanDropdown()
        {
            Thread.Sleep(1000);
            europePlan_Dropdown.Click();
        }

        public void selectEU1DayPlan()
        {
            Thread.Sleep(1000);
            europePlanFirst_Link.Click();
        }

        /// <summary>
        /// Asserts the local plan purchase success.
        /// </summary>
        /// <returns></returns>
        public bool assertLocalPlanPurchaseSuccess()
        {
            IWebElement bodyTag = driver.FindElement(By.TagName("body"));
            string expected = "Thank you for purchasing the " + Settings.Default.LocalPlanCountry + " 1 Day 50MB plan.";
            if (bodyTag.Text.Contains(expected))
            {
                return true;
            }
            throw new Exception("Expected Success message was not displayed. Expected: "+expected);
        }

        public bool assertEuropePlanPurchaseSuccess()
        {
            IWebElement bodyTag = driver.FindElement(By.TagName("body"));
            if (bodyTag.Text.Contains(Settings.Default.PlanPurchaseSuccessEuropeBorder) || bodyTag.Text.Contains(Settings.Default.PlanPurchaseSuccessEuropeRoaming))
            {
                return true;
            }
            throw new Exception("Expected Success message was not displayed for europe plan.");
        }

        /// <summary>
        /// Asserts the already registerd car.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.Exception">Expected message was not displayed when VIN used is already registered.</exception>
        public bool assertAlreadyRegisterdCar()
        {
            IWebElement Alert = addCarAlreadyRegisteredWarning_Container;
            Console.WriteLine("Actual: "+ Alert.Text);
            Console.WriteLine("Expected: " + Settings.Default.RegisterdCarAlert);

            if (Alert.Text.Contains(Settings.Default.RegisterdCarAlert))
            {
                return true;
            }
            throw new Exception("Expected message was not displayed when VIN used is already registered.");
        }

        public void closeAddVINAlert()
        {
            addCarWarningPopUpClose_Link.Click();
        }

        /// <summary>
        /// Asserts the add car empty vin.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.Exception">Expected message was not displayed when VIN field was empty.</exception>
        public bool assertAddCarEmptyVIN()
        {
            IWebElement Alert = addCarValidationError_Span;
            Console.WriteLine("Actual: " + Alert.Text);
            Console.WriteLine("Expected: " + Settings.Default.AddEmptyVINMessage);

            if (Alert.Text.Contains(Settings.Default.AddEmptyVINMessage))
            {
                return true;
            }
            throw new Exception("Expected message was not displayed when VIN field was empty.");
        }

        /// <summary>
        /// Asserts the add car invalid vin.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.Exception">Expected message was not displayed in pop up when VIN field was Invalid.</exception>
        public bool assertAddCarInvalidVIN()
        {
            IWebElement Alert = addCarErrorPopUp_Span;

            Console.WriteLine("Actual: " + Alert.Text);
            Console.WriteLine("Expected: " + Settings.Default.InvalidVINErrorMessage);

            if (Alert.Text.Contains(Settings.Default.InvalidVINErrorMessage))
            {
                return true;
            }
            throw new Exception("Expected message was not displayed in pop up when VIN field was Invalid.");
        }

        public void useDifferentCard()
        {
            selectDifferentCard_Link.Click();
        }

        public void enterInvalidCreditCardDetails()
        {
            cardHolderName_InputField.Clear();
            cardHolderName_InputField.SendKeys("OneName");

            cardNumber_InputField.Clear();
            cardNumber_InputField.SendKeys("123DF");

            cardVerificationCode_InputField.Clear();
            cardVerificationCode_InputField.SendKeys("1");

            new SelectElement(cardExpireMonth_Dropdown).SelectByText("01 Jan");
        }

        public bool assertInvalidCreditCardDetailsErrors()
        {
            bool nameError = assertInvalidCardNameError();
            bool numberError = assertInvalidCardNumberError();
            bool cvvError = assertInvalidCVVError();
            //Not possible to use invalid expire month in january
            //bool monthError = assertInvalidExpireMonthError();

            if (nameError && numberError && cvvError /*&& monthError*/)
            {
                return true;
            }
            throw new Exception("The expected error messages were not displayed for Invalid field entries.");
        }

        public bool assertInvalidCardNameError()
        {
            if (cardHolderNameError_Span.Text.Contains(Settings.Default.CreditCardInvalidNameError))
            {
                return true;
            }
            Console.WriteLine("Expected error message not displayed for Invalid Holder Name.");
            return false;
        }

        public bool assertInvalidCVVError()
        {
            if (cardCVVError_Span.Text.Contains(Settings.Default.CreditCardInvalidCVVError))
            {
                return true;
            }
            Console.WriteLine("Expected error message not displayed for Invalid CVV value.");
            return false;
        }

        public bool assertInvalidExpireMonthError()
        {
            if (cardExpireMonthError_Span.Text.Contains(Settings.Default.CreditCardExpireMonthError))
            {
                return true;
            }
            Console.WriteLine("Expected error message not displayed for Invalid Expire Month.");
            return false;
        }

        public bool assertInvalidCardNumberError()
        {
            if (cardNummberError_Span.Text.Contains(Settings.Default.CreditCardInvalidError))
            {
                return true;
            }
            Console.WriteLine("Expected error message not displayed for Invalid Card Number.");
            return false;
        }

        public bool assertEmptyCreditCardDetailsErrors()
        {
            bool nameError = assertEmptyCardNameError();
            bool numberError = assertEmptyCardNumberError();
            bool cvvError = assertEmptyCVVError();

            if (nameError && numberError && cvvError)
            {
                return true;
            }
            throw new Exception("The expected error messages were not displayed for Empty field entries.");
        }

        private bool assertEmptyCVVError()
        {
            if (cardCVVError_Span.Text.Contains(Settings.Default.CreditCardEmptyCVVError))
            {
                return true;
            }
            Console.WriteLine("Expected error message not displayed for Empty CVV value.");
            return false;
        }

        private bool assertEmptyCardNumberError()
        {
            if (cardNummberError_Span.Text.Contains(Settings.Default.CreditCardEmptyNumberError))
            {
                return true;
            }
            Console.WriteLine("Expected error message not displayed for Empty Card Number.");
            return false;
        }

        private bool assertEmptyCardNameError()
        {
            if (cardHolderNameError_Span.Text.Contains(Settings.Default.CreditCardEmptyNameError))
            {
                return true;
            }
            Console.WriteLine("Expected error message not displayed for Empty Holder Name.");
            return false;
        }

        public void enterValidCreditCardDetails()
        {
            cardHolderName_InputField.Clear();
            cardHolderName_InputField.SendKeys(AccountHelper.paymentDetails.cardHolderName);

            new SelectElement(cardType_Dropdown).SelectByText(AccountHelper.paymentDetails.cardType);

            cardNumber_InputField.Clear();
            cardNumber_InputField.SendKeys(AccountHelper.paymentDetails.cardNumber);

            cardVerificationCode_InputField.Clear();
            cardVerificationCode_InputField.SendKeys(AccountHelper.paymentDetails.cardCVV);

            new SelectElement(cardExpireMonth_Dropdown).SelectByText(AccountHelper.paymentDetails.cardExpireMonth);
            new SelectElement(cardExpireYear_Dropdown).SelectByText(AccountHelper.paymentDetails.cardExpireYear);

        }

        public void saveCardPayment()
        {
            saveCard_CheckBox.Click();
        }

        public bool assertSavedCardPresent()
        {
            if (selectPaymentCard_Dropdown.Displayed)
            {
                return true;
            }
            return false;
        }

        protected void closeTooManyAttemptPopUp()
        {
            cardTooManyAttempts_Button.Click();
        }

        public void submitPaymentUntilPopUp()
        {
            try
            {
                waitForSpinnerDashboard();
                submitPurchasePlan();
                submitPaymentUntilPopUp();
            }
            catch (Exception)
            {
                if (cardTooManyAttemptsWarning_Span.Displayed)
                {
                    return;
                }
                throw;
            }
        }

        public void assertTooManyAttemptsText()
        {
            string span = cardTooManyAttemptsWarning_Span.Text;
            if (span.Contains("Due to too many invalid credit card retries, you cannot complete your purchase at this moment. Please try again in"))
            {
                return;
            }
            throw new Exception("Expected text not displayed in too many attempts pop up.");
        }

        public void removeSimOwner()
        {
            removeSIMOwner_Link(Settings.Default.CurrentVIN).Click();
        }

        public void yesToRemoveSIMOwnerPopUp()
        {
            IWebElement modal = removeSimOwner_Modal(Settings.Default.CurrentVIN);
            waitUntilElementIsDisplayed(modal);
            removeSIMOwnerPopUp_Yes_Button.Click();
            waitUntilElementNotDisplayed(modal);
        }

        public void cancelToRemoveSIMOwnerPopUp()
        {
            IWebElement modal = removeSimOwner_Modal(Settings.Default.CurrentVIN);
            waitUntilElementIsDisplayed(modal);
            removeSIMOwnerPopUp_Cancel_Button.Click();
            waitUntilElementNotDisplayed(modal);
        }
    }
}
