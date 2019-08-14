using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xrm.Domain.CommandHandlers;
using Xrm.Domain.Commands;
using Xrm.Domain.EventHandler;
using Xrm.UnitTests.Fakes;

namespace Xrm.UnitTests.Domain.Branching
{
    [TestClass]
    public class BranchingThroughInheritanceTests : BaseCrmTest
    {
        [TestMethod]
        public void Branch1IsExecuted_WhenRequested()
        {
            var cmd = new BranchedCommand1
            {
                ExecuteBranch1 = true
            };

            CmdBus.Handle(cmd, FlowArgs);

            FakeTracingService trace = (FakeTracingService)FakeTracing;

            Assert.AreEqual(nameof(BrachedCommand1Handler), trace.TracedTexts[0]);
            Assert.AreEqual(nameof(BranchedEvent1Handler), trace.TracedTexts[1]);
        }

        [TestMethod]
        public void Branch2IsExecuted_WhenRequested()
        {
            var cmd = new BranchedCommand1
            {
                ExecuteBranch1 = false
            };

            CmdBus.Handle(cmd, FlowArgs);

            FakeTracingService trace = (FakeTracingService)FakeTracing;

            Assert.AreEqual(nameof(BrachedCommand1Handler), trace.TracedTexts[0]);
            Assert.AreEqual(nameof(BranchedEvent2Handler), trace.TracedTexts[1]);
        }
    }
}
