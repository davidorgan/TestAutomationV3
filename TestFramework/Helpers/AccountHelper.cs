using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFramework
{

    public enum accountType { Liable, Sponsor, Standard }
    public enum ConfirmAddVehicle { Yes, No }

    public class AccountHelper
    {
        public class paymentDetails
        {
            
            public String cardHolderName { get { return "Denise Minnock"; } set { cardHolderName = value; } }
            public String cardType { get { return "VISA"; } set { cardType = value; } }
            public String cardNumber { get { return "4539258504070281"; } set { cardNumber = value; } }
            public String cardCVV { get { return "160"; } set { cardCVV = value; } }
            public String cardExpireMonth { get { return "01 Jan"; } set { cardExpireMonth = value; } }
            public String cardExpireYear { get { return "19"; } set { cardExpireYear = value; } }
            public paymentDetails() { }
            public paymentDetails(string holderName, string cardType, string cardNumber, string cardCVV, string expireMonth, string expireYear)
            {
                this.cardHolderName = holderName;
                this.cardType = cardType;
                this.cardNumber = cardNumber;
                this.cardCVV = cardCVV;
                this.cardExpireMonth = expireMonth;
                this.cardExpireYear = expireYear;
            }
        }

        public static class accountDetails 
        {
            public static string username = Settings.Default.User;
            public static string password = Settings.Default.Password;
            public static string vin = Settings.Default.CurrentVIN;
            public static accountType acType = accountType.Sponsor;
            public static string firstName;
            public static string lastName;

            public static void setStandardAccountdetails()
            {
                if (Settings.Default.UserType.Equals(accountType.Standard))
                {
                    AccountHelper.accountDetails.username = Settings.Default.StandardUser;
                    AccountHelper.accountDetails.password = Settings.Default.Password;
                    AccountHelper.accountDetails.vin = Settings.Default.VIN2QA;                  
                    AccountHelper.accountDetails.acType = accountType.Standard;                                  
                }

            }

            public static void setNewAccountDetails(string mailNum)
            {
                AccountHelper.accountDetails.username = "autoMail" + mailNum + Settings.Default.DisposableMail;
                AccountHelper.accountDetails.password = Settings.Default.Password;    
                AccountHelper.accountDetails.vin =  Settings.Default.CurrentVIN;                    
                AccountHelper.accountDetails.acType = accountType.Sponsor;                     
                AccountHelper.accountDetails.firstName = "FNameAutoReallast";
                AccountHelper.accountDetails.lastName = "LNameAutoReallast";                    
            }
        }
        
    }

}
