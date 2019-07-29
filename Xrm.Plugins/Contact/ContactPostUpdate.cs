using Xrm.Domain.Commands;
using Xrm.Plugin.Base;
using Ctx = Xrm.Models.Crm;

namespace Xrm.Plugin.Contact
{
    public class ContactPostUpdate : Base.Plugin
    {
        public ContactPostUpdate() : base(typeof(ContactPostUpdate))
        {
            RegisterPluginStep<Ctx.Contact>(EventOperation.Update, ExecutionStage.PostOperation, Execute);
        }

        private void Execute(LocalPluginContext localContext)
        {
            Ctx.Contact targetContact = localContext.GetTarget<Ctx.Contact>();

            var setAccountNrOfContactsCommand = new SetAccountNrOfContactsCommand
            {
                FromContact = targetContact
            };
            CommandBus.Handle(setAccountNrOfContactsCommand);
        }
    }
}
