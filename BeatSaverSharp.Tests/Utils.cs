using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BeatSaverSharp.Tests
{
    internal class Utils
    {
        private static readonly HttpOptions Options = new("BeatSaverSharpTests", new Version(1, 0, 0));
        internal static readonly BeatSaver Client = new(Options);

        internal static void CheckTowerOfHeaven(Beatmap? map)
        {
            Assert.IsNotNull(map);

            if (map is not null)
            {
                Assert.AreEqual(map.ID, "5cff621048229f7d88fc724c");
                Assert.AreEqual(map.Key, "17f9");
                Assert.AreEqual(map.Hash, "108c239db3c0596f1ba7426353af1b4cc4fd8b08");

                Assert.AreEqual(map.Uploader.ID, "5cff0b7498cc5a672c850786");
                Assert.AreEqual(map.Uploader.Username, "zorowo");
            }
        }

        internal static void CheckOvercooked(Beatmap? map)
        {
            Assert.IsNotNull(map);

            if (map is not null)
            {
                Assert.AreEqual(map.ID, "5cff621748229f7d88fc9278");
                Assert.AreEqual(map.Key, "4c19");
                Assert.AreEqual(map.Hash, "64ec3d5b0f9239a56d1e709daf29dfa00a42cbef");

                Assert.AreEqual(map.Uploader.ID, "5cff0b7398cc5a672c84efe4");
                Assert.AreEqual(map.Uploader.Username, "lolpants");
            }
        }
    }
}
