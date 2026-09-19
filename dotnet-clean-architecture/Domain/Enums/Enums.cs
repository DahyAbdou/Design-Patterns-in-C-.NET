using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Enums
{
    public class Enums
    {
        public enum UserStatusEnum
        {
            Active =1,
            Disabled =2
        }

        public enum OrderStatusEnum
        {
            Pending = 1,
            Paid = 2,
            Shipped = 3,
            Completed = 4,
            Failed = 5,
            Cancelled = 6
        }
    }
}
