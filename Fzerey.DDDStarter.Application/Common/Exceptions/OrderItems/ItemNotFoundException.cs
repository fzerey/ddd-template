using System;
using Fzerey.DDDStarter.Application.Common.Exceptions.Base;
using Fzerey.DDDStarter.Application.Common.Exceptions.Constants;

namespace Fzerey.DDDStarter.Application.Common.Exceptions.OrderItems
{
    public class ItemNotFoundException : NotFoundException
    {

        public ItemNotFoundException() : base(ExceptionMessages.ITEM_NOT_FOUND, ExceptionCodes.ITEM_NOT_FOUND)
        {
        }
    }
}
