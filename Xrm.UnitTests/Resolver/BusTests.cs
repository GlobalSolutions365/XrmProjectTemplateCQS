using FakeXrmEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xrm.Application.Commands;
using Xrm.Application.Events;
using Xrm.Infrastructure;
using Xrm.Domain.Crm;
using Xrm.Domain.Interfaces;

namespace Xrm.UnitTests.Resolver
{
    [TestClass]
    public class BusTests : BaseCrmTest
    {
        [TestMethod]
        public void CanResolveCommandHandler()
        {
            var context = new XrmFakedContext();

            ICommandBus cmdBus = new Bus();

            TestCommand cmd = new TestCommand { IsHandled = false };

            cmdBus.Handle(cmd, FlowArgs);

            Assert.IsTrue(cmd.IsHandled);
        }

        [TestMethod]
        public void CanResolveCommandHandlerWithUserContextRepo()
        {
            ICommandBus cmdBus = new Bus();

            SetAccountNrOfContactsCommand cmd = new SetAccountNrOfContactsCommand { FromContact = new Contact() };

            cmdBus.Handle(cmd, FlowArgs);
        }

        [TestMethod]
        public void CanResolveEventHandlers()
        {
            IEventBus cmdBus = new Bus();

            TestEvent @event = new TestEvent();

            cmdBus.NotifyListenersAbout(@event, FlowArgs);

            Assert.IsTrue(@event.IsHandled1);
            Assert.IsTrue(@event.IsHandled2);
        }

        [TestMethod]
        public void EventIsExecutedAfterCommand()
        {
            ICommandBus cmdBus = new Bus();

            TestCommand cmd = new TestCommand { IsHandled = false };

            cmdBus.Handle(cmd, FlowArgs);

            Assert.IsTrue(TestCommandExecutedEvent.IsHandled);
        }
    }
}

