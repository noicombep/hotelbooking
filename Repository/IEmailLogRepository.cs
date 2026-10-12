using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace Repository
{
    public interface IEmailLogRepository
    {
        Task Add(EmailLog log);
    }
}
