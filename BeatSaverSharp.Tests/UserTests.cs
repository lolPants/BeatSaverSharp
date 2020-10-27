using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static BeatSaverSharp.Tests.Utils;

namespace BeatSaverSharp.Tests
{
    [TestClass]
    public class UserTests
    {
        #region Method Tests
        [TestMethod]
        public async Task Beatmaps()
        {
            var user = await Client.User("5cff0b7398cc5a672c84efe4");
            Assert.IsNotNull(user);

            if (user is not null)
            {
                var maps = await user.Beatmaps();
                foreach (var map in maps.Docs)
                {
                    Assert.IsNotNull(map.Key);
                    Assert.AreEqual(map.Uploader.ID, "5cff0b7398cc5a672c84efe4");
                    Assert.AreEqual(map.Uploader.Username, "lolpants");
                }

                Assert.IsTrue(maps.Docs.Count > 0);
                Assert.IsTrue(maps.TotalDocs > 0);
                Assert.IsNull(maps.PreviousPage);
            }
        }

        [TestMethod]
        public async Task BeatmapsIterator()
        {
            var user = await Client.User("5cff0b7398cc5a672c84efe4");
            Assert.IsNotNull(user);

            // TODO: Reimplement
            Assert.Fail();

            //if (user is not null)
            //{
            //    int i = 0;
            //    await foreach (var map in user.BeatmapsIterator())
            //    {
            //        Assert.IsNotNull(map.Key);
            //        Assert.AreEqual(map.Uploader.ID, "5cff0b7398cc5a672c84efe4");
            //        Assert.AreEqual(map.Uploader.Username, "lolpants");

            //        if (i > 25) break;
            //        i++;
            //    }
            //}
        }
        #endregion

        #region Equality Tests
        [TestMethod]
        public async Task UserEquality()
        {
            var a = await Client.User("5cff0b7398cc5a672c84efe4");
            var b = await Client.User("5cff0b7398cc5a672c84efe4");
            var c = await Client.User("5cff0b7598cc5a672c8531c7");

            Assert.AreEqual(a, b);
            Assert.AreNotEqual(a, c);
            Assert.AreNotEqual(b, c);
        }
        #endregion
    }
}
