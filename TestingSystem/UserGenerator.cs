using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem
{
    class UserGenerator
    {
        private const int FIXED_ARRAY_SIZE = 4;
        private static Random random = new Random();
        private String[] userNames;
        private String[] passwords;
        
        public UserGenerator()
        {
            userNames = new String[FIXED_ARRAY_SIZE];
            passwords = new String[FIXED_ARRAY_SIZE];
            init();
        }
        //generate random string
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        //init the arrays in random strings
        public void init()
        {
            for (int i = 0; i < FIXED_ARRAY_SIZE; i++)
            {
                userNames[i] = RandomString(i + 2);
            }
            for (int i = 0; i < FIXED_ARRAY_SIZE; i++)
            {
                passwords[i] = RandomString(i + 2);
            }
        }
        //generate random username
        public String GenerateUsername()
        {
            return userNames[random.Next(0, FIXED_ARRAY_SIZE)];
        }

        public String GeneratePasswords()
        {
            return passwords[random.Next(0, FIXED_ARRAY_SIZE)];
        }

        public static void main() {
            UserGenerator gen = new UserGenerator();
            Console.WriteLine(gen.GenerateUsername());
        }
    }
}

