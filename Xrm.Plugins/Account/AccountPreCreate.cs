using System;
using Xrm.Domain.Commands;
using Ctx = Xrm.Models.Crm;
using Xrm.Plugin.Base;

namespace Xrm.Plugin.Account
{
    public class AccountPreCreate : BasePlugin
    {
        public AccountPreCreate() : base(typeof(AccountPreCreate))
        {
            RegisterPluginStep<Ctx.Account>(EventOperation.Create, ExecutionStage.PreOperation, Execute);
        }

        private void Execute(LocalPluginContext localContext)
        {
            Ctx.Account targetAccount = localContext.GetTarget<Ctx.Account>();

            var updateAccountName = new UpdateAccountNameCommand
            {
                TargetAccount = targetAccount,
                Prefix = "Updated "
            };
            CommandBus.Handle(updateAccountName);
        }
    }
}
