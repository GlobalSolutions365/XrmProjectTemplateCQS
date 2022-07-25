using Xrm.Application.Commands;
using Xrm.Plugin.Base;
using Ctx = Xrm.Domain.Crm;

namespace Xrm.Plugin.Account
{
    public class AccountPostCreate : Base.Plugin
    {
        public AccountPostCreate() : base(typeof(AccountPostCreate))
        {
            RegisterPluginStep<Ctx.Account>(EventOperation.Create, ExecutionStage.PostOperation, Execute);
        }

        private void Execute(LocalPluginContext localContext)
        {
            Ctx.Account targetAccount = localContext.GetTarget<Ctx.Account>();

            var testTransactionalCommand = new TestTransactionalCommand
            {
                TargetAccount = targetAccount              
            };
            localContext.Handle(testTransactionalCommand);
        }
    }
}
