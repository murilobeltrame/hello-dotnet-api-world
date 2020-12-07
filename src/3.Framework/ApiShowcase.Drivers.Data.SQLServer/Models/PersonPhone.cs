﻿using System;
using System.Collections.Generic;

#nullable disable

namespace ApiShowcase.Drivers.Data.SQLServer.Models
{
    public partial class PersonPhone
    {
        public int BusinessEntityId { get; set; }
        public string PhoneNumber { get; set; }
        public int PhoneNumberTypeId { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual Person BusinessEntity { get; set; }
        public virtual PhoneNumberType PhoneNumberType { get; set; }
    }
}
