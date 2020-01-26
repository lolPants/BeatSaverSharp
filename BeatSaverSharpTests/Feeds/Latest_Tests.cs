using Microsoft.VisualStudio.TestTools.UnitTesting;
using BeatSaverSharp;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Linq;

namespace BeatSaverSharpTests.Feeds
{
    [TestClass]
    public class Latest_Tests
    {
        private Task<Page> GetPageAsync(uint page, IProgress<double> progress = null)
        {
            BeatSaver client = TestSetup.DefaultTestClient;
            return client.Latest(page, progress);
        }
        private Task<Page> GetPageAsync(uint page, CancellationToken cancellationToken, IProgress<double> progress = null)
        {
            BeatSaver client = TestSetup.DefaultTestClient;
            return client.Latest(page, cancellationToken, progress);
        }

        [TestMethod]
        public async Task GetFirstPage()
        {
            uint pageToGet = 0;
            Page page = await GetPageAsync(pageToGet);
            Assert.AreEqual((int)(pageToGet + 1), page.NextPage.Value);
            Assert.AreEqual(10, page.Docs.Count);
            Assert.IsFalse(string.IsNullOrEmpty(page.Docs.First().Hash));
        }

        [TestMethod]
        public async Task PageDoesntExist()
        {
            uint pageToGet = 0;
            Page page = await GetPageAsync(pageToGet);
            int lastPage = page.LastPage;
            Page emptyPage = await GetPageAsync((uint)(lastPage + 3));
            Assert.AreEqual(0, emptyPage.Docs.Count);
            Assert.IsNull(emptyPage.NextPage);
        }

        [TestMethod]
        public async Task WithCancellationRequested()
        {
            uint pageToGet = 0;

            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            cts.Cancel();
            Page page = null;
            try
            {
                page = await GetPageAsync(pageToGet, token);
            }
            catch (TaskCanceledException)
            {
                Assert.IsNull(page);
            }
            catch(Exception ex)
            {
                Assert.Fail($"An unexpected Exception was thrown: {ex.GetType().Name} - {ex.Message}");
            }
            Assert.IsNull(page);
        }

        [TestMethod]
        public async Task WithProgress()
        {
            uint pageToGet = 0;
            int progressUpdates = 0;
            Progress<double> progress = new Progress<double>(prog =>
            {
                progressUpdates++;
                Console.WriteLine($"Progress: {prog.ToString("P2")}");
            });

            Page page = null;
            try
            {
                page = await GetPageAsync(pageToGet, progress);
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
        public async Task CanceledWithProgress()
        {
            uint pageToGet = 0;
            int progressUpdates = 0;

            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            Progress<double> progress = new Progress<double>(prog =>
            {
                if (prog > 0.3f)
                    cts.Cancel();
                progressUpdates++;
                Console.WriteLine($"Progress: {prog.ToString("P2")}");
            });

            Page page = null;
            try
            {
                page = await GetPageAsync(pageToGet, token, progress);
                Assert.Fail("This should've thrown a TaskCanceledException.");
            }
            catch (TaskCanceledException)
            {
                Assert.IsNull(page);
            }
            catch (Exception ex)
            {
                Assert.Fail($"An unexpected Exception was thrown: {ex.GetType().Name} - {ex.Message}");
            }
        }
    }
}
