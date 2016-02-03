using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFramework
{

    public enum accountType { Liable, Sponsor, Standard }
    public enum ConfirmAddVehicle { Yes, No }

    public static class AccountHelper
    {
        public class paymentDetails
        {
            
            public static String cardHolderName { get { return "Denise Minnock"; } set { cardHolderName = value; } }
            public static String cardType { get { return "VISA"; } set { cardType = value; } }
            public static String cardNumber { get { return "4539258504070281"; } set { cardNumber = value; } }
            public static String cardCVV { get { return "160"; } set { cardCVV = value; } }
            public static String cardExpireMonth { get { return "01 Jan"; } set { cardExpireMonth = value; } }
            public static String cardExpireYear { get { return "19"; } set { cardExpireYear = value; } }

        }

        public class accountDetails 
        {
            public string username;
            public string password;
            public string vin;
            public accountType acType;
            public string firstName;
            public string lastName;


            public accountDetails()
            {
                this.username = Settings.Default.User;
                this.password = Settings.Default.Password;
                this.vin = Settings.Default.CurrentVIN;
                this.acType = accountType.Sponsor;
            }


            public accountDetails(string user, string pw, string vin, accountType acT, string fname, string lname)
            {
                this.username = user;
                this.password = pw;
                this.vin = vin;
                this.acType = acT;
                this.firstName = fname;
                this.lastName = lname;
            }

            public static accountDetails setAccountDetails()
            {
                if (Settings.Default.UserType.Equals(accountType.Standard))
                {
                    accountDetails standardAccount = new accountDetails(
                        Settings.Default.StandardUser,
                        Settings.Default.Password,
                        Settings.Default.VIN2QA,
                        accountType.Standard,
                        "Standard",
                        "Test"
                        );
                    return standardAccount;         
                }
                return new accountDetails();
            }

        }
        
    }

}
