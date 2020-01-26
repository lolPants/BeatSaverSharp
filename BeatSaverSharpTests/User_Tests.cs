using Microsoft.VisualStudio.TestTools.UnitTesting;
using BeatSaverSharp;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace BeatSaverSharpTests
{
    [TestClass]
    public class User_Tests
    {
        [TestMethod]
        public async Task GetUser()
        {
            BeatSaver client = TestSetup.DefaultTestClient;
            string userId = "5cff0b7398cc5a672c84f1d8"; // ruckus
            string expectedUsername = "ruckus";
            User user = await client.User(userId);
            Assert.AreEqual(expectedUsername, user.Username);
        }

        [TestMethod]
        public async Task GetUserBeatmaps()
        {
            BeatSaver client = TestSetup.DefaultTestClient;
            string userId = "5cff0b7398cc5a672c84f1d8"; // ruckus
            User user = await client.User(userId);
            Page beatmaps = await user.Beatmaps(0);
            Assert.AreEqual(10, beatmaps.Docs.Count);
            Assert.AreEqual(beatmaps.Docs[0].Uploader, user);
        }



        [TestMethod]
        public async Task UserEquality()
        {
            BeatSaver client = TestSetup.DefaultTestClient;
            string userId = "5cff0b7398cc5a672c84f1d8"; // ruckus
            string otherUserId = "5cff0b7298cc5a672c84e8c4"; // rustic
            User user = await client.User(userId);
            await Task.Delay(2000);
            User otherUser = await client.User(otherUserId);
            Page beatmaps = await user.Beatmaps(0);
            Assert.AreEqual(beatmaps.Docs[0].Uploader, user);
            Assert.AreNotEqual(user, otherUser);
            Assert.IsTrue(beatmaps.Docs[0].Uploader == user);
            Assert.IsFalse(beatmaps.Docs[0].Uploader != user);
            Assert.IsFalse(user == otherUser);
            Assert.IsTrue(user != otherUser);
            Assert.IsFalse(user == null);
            Assert.IsTrue(user != null);
            Assert.IsFalse(null == otherUser);
            Assert.IsTrue(null != otherUser);
        }

        [TestMethod]
        public async Task InvalidUserId()
        {
            BeatSaver client = TestSetup.DefaultTestClient;
            string userId = "asdf0b7398cc5a672c84f1d8";
            User user = await client.User(userId);
            Assert.IsNull(user);
        }

        [TestMethod]
        public async Task EmptyUserId()
        {
            BeatSaver client = TestSetup.DefaultTestClient;
            string userId = "";
            User user = await client.User(userId);
            Assert.IsNull(user);
        }

        [TestMethod]
        public async Task NullUserId()
        {
            BeatSaver client = TestSetup.DefaultTestClient;
            string userId = null;
            User user = await client.User(userId);
            Assert.IsNull(user);
        }



        [TestMethod]
        public async Task WithCancellationRequested()
        {
            BeatSaver client = TestSetup.DefaultTestClient;
            string userId = "5cff0b7398cc5a672c84f1d8"; // ruckus
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            cts.Cancel();
            User user = null;
            try
            {
                user = await client.User(userId, token);
            }
            catch (TaskCanceledException)
            {
                
            }
            catch(Exception ex)
            {
                Assert.Fail($"An unexpected Exception was thrown: {ex.GetType().Name} - {ex.Message}");
            }
            Assert.IsNull(user);
        }

        [TestMethod]
        public async Task WithProgress()
        {
            BeatSaver client = TestSetup.DefaultTestClient;
            string userId = "5cff0b7398cc5a672c84f1d8"; // ruckus
            string expectedUsername = "ruckus";
            int progressUpdates = 0;
            Progress<double> progress = new Progress<double>(prog =>
            {
                progressUpdates++;
                Console.WriteLine($"Progress: {prog.ToString("P2")}");
            });
            User user = null;
            try
            {
                user = await client.User(userId, progress);
                Assert.AreEqual(expectedUsername, user.Username);
                Assert.IsTrue(progressUpdates > 0);
            }
            catch (Exception ex)
            {
                Assert.Fail($"An unexpected Exception was thrown: {ex.GetType().Name} - {ex.Message}");
            }
        }
    }
}
