using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xrm.Domain.Commands;
using Xrm.Domain.Events;
using Xrm.Models.Flow;

namespace Xrm.Domain.CommandHandlers
{
    public class TestTransactionalCommandHandler : CommandHandler<Commands.TestTransactionalCommand, Events.TestTransactionalEvent1>
    {
        public TestTransactionalCommandHandler(FlowArguments flowArgs) : base(flowArgs)
        {
        }

        public override TestTransactionalEvent1 Execute(TestTransactionalCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
