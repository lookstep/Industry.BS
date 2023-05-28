using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmoloyeeTask.Data.Interfaces
{
    public interface IMailSender
    {
        public Task SendMailMessageAsync(string listenerMail, string code);
    }
}
