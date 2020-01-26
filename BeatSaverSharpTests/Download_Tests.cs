using Microsoft.VisualStudio.TestTools.UnitTesting;
using BeatSaverSharp;
using BeatSaverSharp.Exceptions;
using System.Threading.Tasks;
using System;
using System.IO;
using System.IO.Compression;
using System.Threading;

namespace BeatSaverSharpTests
{
    [TestClass]
    class Download_Tests
    {
        [TestMethod]
        public async Task KeyWithProgress()
        {
            BeatSaver client = TestSetup.DefaultTestClient;
            string key = "b";
            int expectedFilesInZip = 4;
            int progressUpdates = 0;
            Progress<double> progress = new Progress<double>(prog =>
            {
                progressUpdates++;
                Console.WriteLine($"Progress: {prog.ToString("P2")}");
            });
            try
            {
                Beatmap beatmap = await client.Key(key);
                byte[] zip = await beatmap.DownloadZip(true, progress);
                Assert.IsTrue(progressUpdates > 100);
                using (var stream = new MemoryStream(zip))
                {
                    var zipArchive = new ZipArchive(stream);
                    Assert.AreEqual(expectedFilesInZip, zipArchive.Entries.Count);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail($"An unexpected Exception was thrown: {ex.GetType().Name} - {ex.Message}");
            }
        }

        [TestMethod]
        public async Task WithCancellationRequested()
        {
            BeatSaver client = TestSetup.DefaultTestClient;
            string key = "b";
            int progressUpdates = 0;
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;

            Progress<double> progress = new Progress<double>(prog =>
            {
                if (prog > 0.5d)
                    cts.Cancel();
                progressUpdates++;
                Console.WriteLine($"Progress: {prog.ToString("P2")}");
            });
            byte[] zip = null;
            try
            {
                Beatmap beatmap = await client.Key(key);
                zip = await beatmap.DownloadZip(true, token, progress);
                Assert.Fail("Beatmap.DownloadZip should've thrown a TaskCanceledException.");
            }
            catch(TaskCanceledException ex)
            {
                Assert.IsTrue(progressUpdates > 0);
                Assert.IsNull(zip);
            }
            catch (Exception ex)
            {
                Assert.Fail($"An unexpected Exception was thrown: {ex.GetType().Name} - {ex.Message}");
            }
        }
    }
}
