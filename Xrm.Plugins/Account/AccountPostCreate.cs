using Xrm.Domain.Commands;
using Xrm.Plugin.Base;
using Ctx = Xrm.Models.Crm;

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

            var updateAccountName = new UpdateAccountNameCommand
            {
                TargetAccount = targetAccount,
                Prefix = "Updated "
            };
            localContext.CommandBus.Handle(updateAccountName);
        }
    }
}
