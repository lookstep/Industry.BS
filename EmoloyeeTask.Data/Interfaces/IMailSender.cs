namespace EmoloyeeTask.Data.Interfaces
{
    public interface IMailSender
    {
        public Task SendMailMessageAsync(string listenerMail, string code);
    }
}
