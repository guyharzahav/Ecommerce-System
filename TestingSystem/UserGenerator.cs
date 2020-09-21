using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem
{
    class UserGenerator
    {
        public const  int FIXED_ROWS_SIZE = 3;
        public const int FIXED_COLUMNS_SIZE = 4;
        public const int FIXED_USERNAMES_SIZE = 4;
        public const int FIXED_PASSWORDS_SIZE = 3;
        public static int VALID_USERNAME = 0;
        public static int INCORRECT_USERNAME = 1;
        public static int EXTREMELYWRONG_USERNAME = 2;
        private static Random random = new Random();
        public static String[,] userNames = new String[FIXED_ROWS_SIZE, FIXED_COLUMNS_SIZE] 
        { {"Guy11","NAor","Chen12", "Sundy"},//Max-14,no spaces, a-Z,0-9,Min-3
          {"b","1","s","q"},
          {"       "," ! ! ! !)DROP TABLES","       !!@", "     112 ! 1@#@#$@@#$@$#@#$@#$@$#@$#$@#  23$#@$@#$#@@$#@##@$ "} };
        public static String[] passwords = new String[FIXED_USERNAMES_SIZE] {"bbb111","aasdasasd","asdasd", "sdasda"};

        public UserGenerator(){}
        //generate random string
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static String[] GetValidUsernames() 
        {
            String[] ret = new String[FIXED_COLUMNS_SIZE];
            for (int i = 0; i < FIXED_COLUMNS_SIZE; i++)
            {
                ret[i] = userNames[VALID_USERNAME, i];
            }
            return ret;
        }

        public static String[] GetIncorrectUsernames()
        {
            String[] ret = new String[FIXED_COLUMNS_SIZE];
            for (int i = 0; i < FIXED_COLUMNS_SIZE; i++)
            {
                ret[i] = userNames[INCORRECT_USERNAME, i];
            }
            return ret;
        }

        public static string[] GetExtremelyWrongUsernames() 
        {
            String[] ret = new String[FIXED_COLUMNS_SIZE];
            for (int i = 0; i < FIXED_COLUMNS_SIZE; i++)
            {
                ret[i] = userNames[EXTREMELYWRONG_USERNAME, i];
            }
            return ret;
        }

        public static string[] GetPasswords() 
        {
            return passwords;
        }

        public static void Main(string[] argv)
        {

            for (int i = 0; i < UserGenerator.FIXED_ROWS_SIZE; i++)
            {
                Console.WriteLine(UserGenerator.userNames[i, VALID_USERNAME]);
            }
       
            Console.ReadLine();
        }
    }
}

