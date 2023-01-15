using Fateor.Application.Bulletin.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fateor.Models
{
    public class BulletinListViewModel : ViewModelBase
    {
        public List<Bulletin>? Bulletins { get; set; }
    }
}