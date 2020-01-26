using Microsoft.VisualStudio.TestTools.UnitTesting;
using BeatSaverSharp;
using BeatSaverSharp.Exceptions;
using System.Threading.Tasks;
using System;

namespace BeatSaverSharpTests
{
    [TestClass]
    class RateLimit_Tests
    {
        [TestMethod]
        public async Task GetUser()
        {
            BeatSaver client = TestSetup.DefaultTestClient;
            string userId = "5cff0b7398cc5a672c84f1d8";
            Exception thrownException = null;
            for(int i = 0; i < 5; i++)
            {
                try
                {
                    User user = await client.User(userId);
                }
                catch(RateLimitExceededException ex)
                {
                    thrownException = ex;
                    Assert.AreEqual(0, ex.RateLimit.Remaining);
                    Console.WriteLine($"Reset time: {ex.RateLimit.Reset.ToLocalTime()}\nRemaining Calls: {ex.RateLimit.Remaining}\nTotal Calls Available: {ex.RateLimit.Total}");
                    break;
                }
                catch(Exception ex)
                {
                    Assert.Fail($"An unexpected Exception was thrown: {ex.GetType().Name} - {ex.Message}");
                }
            }
            Assert.IsNotNull(thrownException);
        }
    }
}
