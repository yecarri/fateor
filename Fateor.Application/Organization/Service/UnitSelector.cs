using Fateor.Application.Organization.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Organization.Service
{
    public sealed class UnitSelector : IUnitSelector
    {
        private readonly int _defaultUnitId;

        public UnitSelector(int defaultUnitId)
        {
            _defaultUnitId=defaultUnitId;
        }

        public int SelectUnit(Member member)
        {
            return _defaultUnitId;
        }
    }
}
