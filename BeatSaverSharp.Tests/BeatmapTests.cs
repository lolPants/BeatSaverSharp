using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BeatSaverSharp.Exceptions;
using static BeatSaverSharp.Tests.Utils;

namespace BeatSaverSharp.Tests
{
    [TestClass]
    public class BeatmapTests
    {
        #region Constructor Tests
        [TestMethod]
        public async Task CreatePartialBeatmapFromKey()
        {
            var map = new Beatmap(Client, key: "17f9");
            Assert.IsTrue(map.Partial);
            Assert.IsNull(map.Name);

            await map.Populate();
            Assert.IsFalse(map.Partial);
            Assert.IsNotNull(map.Name);
        }

        [TestMethod]
        public async Task CreatePartialBeatmapFromHash()
        {
            var map = new Beatmap(Client, hash: "108c239db3c0596f1ba7426353af1b4cc4fd8b08");
            Assert.IsTrue(map.Partial);
            Assert.IsNull(map.Name);

            await map.Populate();
            Assert.IsFalse(map.Partial);
            Assert.IsNotNull(map.Name);
        }

        [TestMethod]
        public async Task CreateInvalidPartialBeatmap()
        {
            Assert.ThrowsException<ArgumentException>(() =>
            {
                var map = new Beatmap(Client);
            });

            var task1 = Assert.ThrowsExceptionAsync<InvalidPartialKeyException>(async () =>
            {
                var map = new Beatmap(Client, key: "key");
                await map.Populate();
            });

            var task2 = Assert.ThrowsExceptionAsync<InvalidPartialHashException>(async () =>
            {
                var map = new Beatmap(Client, hash: "hash");
                await map.Populate();
            });

            await Task.WhenAll(task1, task2);
        }
        #endregion

        #region Method Tests
        [TestMethod]
        public async Task Refresh()
        {
            var map = await Client.Key("28a");
            Assert.IsNotNull(map);

            if (map is not null)
            {
                await map.Refresh();
            }
        }

        [TestMethod]
        public async Task RefreshStats()
        {
            var map = await Client.Key("28a");
            Assert.IsNotNull(map);

            if (map is not null)
            {
                await map.RefreshStats();
            }
        }

        [TestMethod]
        public async Task DownloadZip()
        {
            var map = await Client.Key("28a");
            Assert.IsNotNull(map);

            if (map is not null)
            {
                var bytes = await map.ZipBytes();
                Assert.AreNotEqual(bytes.Length, 0);
            }
        }

        [TestMethod]
        public async Task FetchCoverImage()
        {
            var map = await Client.Key("28a");
            Assert.IsNotNull(map);

            if (map is not null)
            {
                var bytes = await map.CoverImageBytes();
                Assert.AreNotEqual(bytes.Length, 0);
            }
        }
        #endregion

        #region Equality Tests
        [TestMethod]
        public async Task BeatmapEquality()
        {
            var keyTask = Client.Key("17f9");
            var hashTask = Client.Hash("108c239db3c0596f1ba7426353af1b4cc4fd8b08");
            var otherTask = Client.Key("28a");

            var x = await Task.WhenAll(keyTask, hashTask, otherTask);
            Beatmap? key = x[0];
            Beatmap? hash = x[1];
            Beatmap? other = x[2];

            Assert.IsNotNull(key);
            Assert.IsNotNull(hash);
            Assert.IsNotNull(other);

            Assert.AreEqual(key, hash);
            Assert.AreNotEqual(key, other);
            Assert.AreNotEqual(hash, other);
        }
        #endregion
    }
}
