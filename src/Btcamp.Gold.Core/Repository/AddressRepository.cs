﻿using Btcamp.Gold.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Btcamp.Gold.Core.Repository
{
    public class AddressRepository : Infrastructure.RepositoryBase<Address>, Infrastructure.IRepository<Address>
    {
        public AddressRepository(Infrastructure.IDbContextFactory dbfactory)
            : base(dbfactory)
        {

        }
    }
}
