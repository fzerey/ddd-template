using System;
using Fzerey.DDDStarter.Application.Common.Exceptions.Base;
using Fzerey.DDDStarter.Application.Common.Exceptions.Constants;

namespace Fzerey.DDDStarter.Application.Common.Exceptions.Orders
{
    public class OrderNotFoundException : NotFoundException
    {
        public OrderNotFoundException() : base(ExceptionMessages.ORDER_NOT_FOUND, ExceptionCodes.ORDER_NOT_FOUND)
        {
        }
    }
}
