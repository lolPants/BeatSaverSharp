using Microsoft.VisualStudio.TestTools.UnitTesting;
using BeatSaverSharp;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace BeatSaverSharpTests
{
    [TestClass]
    public class Beatmap_Tests
    {
        [TestMethod]
        public async Task GetByKey()
        {
            BeatSaver client = TestSetup.DefaultTestClient;
            string beatmapKey = "b";
            Beatmap beatmap = await client.Key(beatmapKey);
            CheckBeliever(beatmap);
        }

        [TestMethod]
        public async Task GetByHash()
        {
            BeatSaver client = TestSetup.DefaultTestClient;
            string beatmapHash = "19f2879d11a91b51a5c090d63471c3e8d9b7aee3";
            Beatmap beatmap = await client.Hash(beatmapHash);
            CheckBeliever(beatmap);
        }

        [TestMethod]
        public async Task Equality()
        {
            BeatSaver client = TestSetup.DefaultTestClient;
            string believerKey = "b";
            string believerHash = "19f2879d11a91b51a5c090d63471c3e8d9b7aee3";
            string beatItKey = "217";
            Beatmap believerByKey = await client.Key(believerKey);
            Beatmap believerByHash = await client.Hash(believerHash);
            Beatmap beatIt = await client.Key(beatItKey);
            Assert.AreEqual(believerByKey, believerByHash);
            Assert.AreNotEqual(believerByKey, beatIt);
            Assert.IsTrue(believerByKey == believerByHash);
            Assert.IsFalse(believerByKey != believerByHash);
            Assert.IsFalse(believerByKey == beatIt);
            Assert.IsTrue(believerByKey != beatIt);
            Assert.IsFalse(believerByKey == null);
            Assert.IsTrue(believerByKey != null);
            Assert.IsFalse(null == beatIt);
            Assert.IsTrue(null != beatIt);
            Assert.AreEqual(believerByKey.GetHashCode(), believerByHash.GetHashCode());
        }

        private void CheckBeliever(Beatmap beatmap)
        {
            string beatmapKey = "b";
            string beatmapHash = "19f2879d11a91b51a5c090d63471c3e8d9b7aee3";
            string expectedName = "Imagine Dragons - Believer";
            string expectedSongname = "Believer";
            string expectedSongSubname = "Imagine Dragons";
            string expectedSongAuthorName = "Rustic";
            string expectedLevelAuthorName = "rustic";
            string expectedDescription = "Currently expert only. Events included.";
            string expectedUploaderId = "5cff0b7298cc5a672c84e8c4";
            string expectedUploaderUsername = "rustic";
            Difficulties expectedDifficulties = new Difficulties() { Expert = true };
            int expectedBpm = 125;
            int expectedPlays = 70725;
            int expectedNumCharacteristics = 1;
            Assert.AreEqual(beatmapKey, beatmap.Key);
            Assert.AreEqual(beatmapHash, beatmap.Hash);
            Assert.AreEqual(expectedName, beatmap.Name);
            Assert.AreEqual(expectedSongname, beatmap.Metadata.SongName);
            Assert.AreEqual(expectedSongSubname, beatmap.Metadata.SongSubName);
            Assert.AreEqual(expectedSongAuthorName, beatmap.Metadata.SongAuthorName);
            Assert.AreEqual(expectedLevelAuthorName, beatmap.Metadata.LevelAuthorName);
            Assert.AreEqual(expectedDescription, beatmap.Description);
            Assert.AreEqual(expectedNumCharacteristics, beatmap.Metadata.Characteristics.Count);
            Assert.AreEqual(expectedUploaderId, beatmap.Uploader.ID);
            Assert.AreEqual(expectedUploaderUsername, beatmap.Uploader.Username);
            Assert.AreEqual(expectedPlays, beatmap.Stats.Plays);
            Assert.AreEqual(expectedDifficulties, beatmap.Metadata.Difficulties);
            Assert.AreEqual(expectedBpm, beatmap.Metadata.BPM);

            // TODO: Add more property equality checks.
        }

        [TestMethod]
        public async Task InvalidKey()
        {
            BeatSaver client = TestSetup.DefaultTestClient;
            string beatmapKey = "asdf";
            Beatmap beatmap = await client.Key(beatmapKey);
            Assert.IsNull(beatmap);
        }

        [TestMethod]
        public async Task EmptyKey()
        {
            BeatSaver client = TestSetup.DefaultTestClient;
            string beatmapKey = "";
            Beatmap beatmap = await client.Key(beatmapKey);
            Assert.IsNull(beatmap);
        }

        [TestMethod]
        public async Task NullKey()
        {
            BeatSaver client = TestSetup.DefaultTestClient;
            string beatmapKey = null;
            Beatmap beatmap = await client.Key(beatmapKey);
            Assert.IsNull(beatmap);
        }



        [TestMethod]
        public async Task KeyWithCancellationRequested()
        {
            BeatSaver client = TestSetup.DefaultTestClient;
            string beatmapKey = "b";
            
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            cts.Cancel();
            Beatmap beatmap = null;
            try
            {
                beatmap = await client.Key(beatmapKey, token);
            }
            catch (TaskCanceledException)
            {
                
            }
            catch(Exception ex)
            {
                Assert.Fail($"An unexpected Exception was thrown: {ex.GetType().Name} - {ex.Message}");
            }
            Assert.IsNull(beatmap);
        }

        [TestMethod]
        public async Task KeyWithProgress()
        {
            BeatSaver client = TestSetup.DefaultTestClient;
            string beatmapKey = "b";
            int progressUpdates = 0;
            Progress<double> progress = new Progress<double>(prog =>
            {
                progressUpdates++;
                Console.WriteLine($"Progress: {prog.ToString("P2")}");
            });

            Beatmap beatmap = null;
            try
            {
                beatmap = await client.Key(beatmapKey, progress);
                CheckBeliever(beatmap);
                Assert.IsTrue(progressUpdates > 0);
            }
            catch (TaskCanceledException)
            {

            }
            catch (Exception ex)
            {
                Assert.Fail($"An unexpected Exception was thrown: {ex.GetType().Name} - {ex.Message}");
            }
        }

        [TestMethod]
        public async Task HashWithCancellationRequested()
        {
            BeatSaver client = TestSetup.DefaultTestClient;
            string beatmapKey = "19f2879d11a91b51a5c090d63471c3e8d9b7aee3";

            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            cts.Cancel();
            Beatmap beatmap = null;
            try
            {
                beatmap = await client.Hash(beatmapKey, token);
            }
            catch (TaskCanceledException)
            {

            }
            catch (Exception ex)
            {
                Assert.Fail($"An unexpected Exception was thrown: {ex.GetType().Name} - {ex.Message}");
            }
            Assert.IsNull(beatmap);
        }

        [TestMethod]
        public async Task HashWithProgress()
        {
            BeatSaver client = TestSetup.DefaultTestClient;
            string beatmapKey = "19f2879d11a91b51a5c090d63471c3e8d9b7aee3";
            int progressUpdates = 0;
            Progress<double> progress = new Progress<double>(prog =>
            {
                progressUpdates++;
                Console.WriteLine($"Progress: {prog.ToString("P2")}");
            });

            Beatmap beatmap = null;
            try
            {
                beatmap = await client.Hash(beatmapKey, progress);
                CheckBeliever(beatmap);
                Assert.IsTrue(progressUpdates > 0);
            }
            catch (TaskCanceledException)
            {

            }
            catch (Exception ex)
            {
                Assert.Fail($"An unexpected Exception was thrown: {ex.GetType().Name} - {ex.Message}");
            }
        }
    }
}
