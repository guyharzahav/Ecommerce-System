using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.AcceptanceTests
{
    /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-login-23 </req>
    [TestClass]
    public class LoginStoryTest : SystemTrackTest
    {
        string[] validUsernames = UserGenerator.GetValidUsernames();
        string[] passwords = UserGenerator.GetPasswords();

        [TestInitialize]
        public void SetUp()
        {
            for (int i = 0; i < UserGenerator.FIXED_USERNAMES_SIZE; i++) //fixed from version 1
            {
                Register(validUsernames[i], passwords[i]);
            }
        }

        [TestCleanup]
        public void TearDown()
        {
            ClearAllUsers();
        }

        [TestMethod]
        //happy
        public void ExistingUsernameAndPasswordTest()
        {
            for (int i = 0; i < UserGenerator.FIXED_USERNAMES_SIZE; i++)
            {
                Assert.IsTrue(Login(validUsernames[i], passwords[i]).Item1, Login(validUsernames[i], passwords[i]).Item2);
            }
        }

        [TestMethod]
        //sad
        public void UnmatchUsernameAndPasswordTest()
        {
            for (int i = 0; i < UserGenerator.FIXED_USERNAMES_SIZE - 1; i++)
            {
                Assert.IsFalse(Login(validUsernames[i], passwords[i + 1]).Item1, Login(validUsernames[i], passwords[i + 1]).Item2);
            }
        }

        [TestMethod]
        //sad
        public void LoginTwiceTest() //fixed from version 1
        {
            for (int i = 0; i < UserGenerator.FIXED_USERNAMES_SIZE; i++)
            {
                Assert.IsTrue(Login(validUsernames[i], passwords[i]).Item1, Login(validUsernames[i], passwords[i]).Item2);
                Assert.IsFalse(Login(validUsernames[i], passwords[i]).Item1, Login(validUsernames[i], passwords[i]).Item2);
            }
        }

        [TestMethod]
        //bad
        public void InvalidPasswordAndUsernameTest()
        {
            Assert.IsFalse(Login("@!#!#!@$%^$%^", "  ").Item1, Login("@!#!#!@$%^$%^", "  ").Item2);
        }
    }
}
