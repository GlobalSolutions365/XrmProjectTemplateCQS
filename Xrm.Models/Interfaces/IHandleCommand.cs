namespace Xrm.Models.Interfaces
{
    public interface IHandleCommand<TCommand> where TCommand : ICommand
    {
        void Handle(TCommand command);
    }
}
