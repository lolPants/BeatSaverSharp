using BeatSaverSharp;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BeatSaverSharpTests
{
    internal static class TestSetup
    {
        public static BeatSaver DefaultTestClient;
        static TestSetup()
        {
            HttpOptions httpOptions = new HttpOptions() { ApplicationName = "BeatSaverSharpUnitTests", Version = Assembly.GetExecutingAssembly().GetName().Version };
            DefaultTestClient = new BeatSaver(httpOptions);
        }
    }
}
