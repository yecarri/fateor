using Fateor.Application.Organization.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Organization.Service
{
    public class MemberFeeBatchService : IMemberFeeBatchService
    {
        
        public MemberFeeBatch[] GetMemberFeeBatches(DateOnly dateFrom, DateOnly dateTo)
        {
            return new MemberFeeBatch[0];
        }
    }
}
