using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetsGame.Common.Data
{
    public interface IEntityMetaData
    { 
        DateTime DateAdded { get; set; }
        DateTime LastModified { get; set; }
    }

    public interface IIdable<TId> : IEntityMetaData
    {
        TId Id { get; set; }
    }
}
