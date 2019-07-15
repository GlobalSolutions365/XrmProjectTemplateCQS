using FakeXrmEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xrm.Base;
using Xrm.Domain.Commands;
using Xrm.Domain.Events;
using Xrm.Models.Interfaces;

namespace Xrm.UnitTests.Resolver
{
    [TestClass]
    public class BusTests
    {
        [TestMethod]
        public void CanResolveCommandHandler()
        {
            var context = new XrmFakedContext();

            ICommandBus cmdBus = new Bus(context.GetOrganizationService());

            TestCommand cmd = new TestCommand { IsHandled = false };

            cmdBus.Handle(cmd);

            Assert.IsTrue(cmd.IsHandled);
        }

        [TestMethod]
        public void CanResolveEventHandlers()
        {
            var context = new XrmFakedContext();

            IEventBus cmdBus = new Bus(context.GetOrganizationService());

            TestEvent @event = new TestEvent();

            cmdBus.NotifyListenersAbout(@event);

            Assert.IsTrue(@event.IsHandled1);
            Assert.IsTrue(@event.IsHandled2);
        }
    }
}
