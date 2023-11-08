using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.ViewModels.Account
{
    public class FinalyOrderViewModel
    {
        public long OrderId { get; set; }
        public long UserId { get; set; }
        public bool IsWallet { get; set; }
    }
    public enum FinalyOrderResult
    {
        HasNotUser,
        NotFound,
        Error,
        Success
    }
}
