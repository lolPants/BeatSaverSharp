using Microsoft.VisualStudio.TestTools.UnitTesting;
using BeatSaverSharp;
using BeatSaverSharp.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace BeatSaverSharpTests
{
    [TestClass]
    class ClientConstructor_Tests
    {
        [TestMethod]
        public void DefaultOptions()
        {
            HttpOptions httpOptions = new HttpOptions();
            BeatSaver client = new BeatSaver(httpOptions);
        }

        [TestMethod]
        public void ApplicationNameAndVersion()
        {
            HttpOptions httpOptions = new HttpOptions() { ApplicationName = "BeatSaverSharpUnitTests", Version = Assembly.GetExecutingAssembly().GetName().Version };
            BeatSaver client = new BeatSaver(httpOptions);
        }

        [TestMethod]
        public void ApplicationNameWithoutVersion()
        {
            HttpOptions httpOptions = new HttpOptions() { ApplicationName = "BeatSaverSharpUnitTests" };
            Assert.ThrowsException<ArgumentException>(() => new BeatSaver(httpOptions));
        }

        [TestMethod]
        public void AddedAgentWithStringVersion()
        {
            ApplicationAgent[] applicationAgents = new ApplicationAgent[] { new ApplicationAgent("BeatSaverSharpUnitTests", "1.0.0.0") };
            HttpOptions httpOptions = new HttpOptions() { Agents = applicationAgents };
            BeatSaver client = new BeatSaver(httpOptions);
        }

        [TestMethod]
        public void AddedAgentsWithVersion()
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            ApplicationAgent[] applicationAgents = new ApplicationAgent[] { new ApplicationAgent("BeatSaverSharpUnitTests", version) };
            HttpOptions httpOptions = new HttpOptions() { Agents = applicationAgents };
            BeatSaver client = new BeatSaver(httpOptions);
        }

        [TestMethod]
        public void AgentWithEmptyVersionString()
        {
            ApplicationAgent[] applicationAgents = new ApplicationAgent[] { new ApplicationAgent("BeatSaverSharpUnitTests", "") };
            HttpOptions httpOptions = new HttpOptions() { Agents = applicationAgents };
            Assert.ThrowsException<FormatException>(() => new BeatSaver(httpOptions));
        }

        [TestMethod]
        public void AgentWithEmptyApplicationString()
        {
            ApplicationAgent[] applicationAgents = new ApplicationAgent[] { new ApplicationAgent("", "1.0.0.0") };
            HttpOptions httpOptions = new HttpOptions() { Agents = applicationAgents };
            Assert.ThrowsException<FormatException>(() => new BeatSaver(httpOptions));
        }

        [TestMethod]
        public void AgentWithNullVersionString()
        {
            string nullString = null;
            ApplicationAgent[] applicationAgents = new ApplicationAgent[] { new ApplicationAgent("BeatSaverSharpUnitTests", nullString) };
            HttpOptions httpOptions = new HttpOptions() { Agents = applicationAgents };
            Assert.ThrowsException<ArgumentException>(() => new BeatSaver(httpOptions));
        }

        [TestMethod]
        public void AgentWithNullApplicationString()
        {
            string nullString = null;
            ApplicationAgent[] applicationAgents = new ApplicationAgent[] { new ApplicationAgent(nullString, "1.0.0.0") };
            HttpOptions httpOptions = new HttpOptions() { Agents = applicationAgents };
            Assert.ThrowsException<ArgumentException>(() => new BeatSaver(httpOptions));
        }



        [TestMethod]
        public void AgentWithNullVersion()
        {
            Version nullVersion = null;
            Assert.ThrowsException<NullReferenceException>(() =>  new ApplicationAgent[] { new ApplicationAgent("BeatSaverSharpUnitTests", nullVersion) });
        }
    }
}
