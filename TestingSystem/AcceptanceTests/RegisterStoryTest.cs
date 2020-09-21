using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.AcceptanceTests
{
    /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-registration-22 </req>
    [TestClass]
    public class RegisterStoryTest : SystemTrackTest
    {
        string[] validUsernames = UserGenerator.GetValidUsernames();
        string[] incorrectUsernames = UserGenerator.GetIncorrectUsernames(); 
        string[] extremelyIncorrectUsernames = UserGenerator.GetExtremelyWrongUsernames();
        string[] passwords = UserGenerator.GetPasswords();

        [TestInitialize]
        public void SetUp()
        {
        }

        [TestCleanup]
        public void TearDown()
        {
            ClearAllUsers();
        }

        [TestMethod]
        //happy
        public void LegalUserDetailsTest() 
        {
            for (int i = 0; i < validUsernames.Length; i++) 
            {
                string username = validUsernames[i];
                string password = passwords[i];
                Assert.IsTrue(Register(username, password).Item1, Register(username, password).Item2);
            }
        }

        [TestMethod]
        //sad
        public void InvalidUserNameTest()
        {
            for (int i = 0; i < validUsernames.Length; i++)
            {
                string username = incorrectUsernames[i];
                string password = passwords[i];
                Assert.IsFalse(Register(username, password).Item1, Register(username, password).Item2);
            }
        }

        [TestMethod]
        //bad
        public void BlankUsernameAndPasswordRegisterTest()
        {
            for (int i = 0; i < validUsernames.Length; i++)
            {
                string username = extremelyIncorrectUsernames[i];
                string password = passwords[i];
                Assert.IsFalse(Register(username, password).Item1, Register(username, password).Item2);
            }
        }
    }
}
