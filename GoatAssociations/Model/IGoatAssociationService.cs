using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoatAssociations.Model
{
    /// <summary>
    /// Interface providing methods for reading and saving association
    /// </summary>
    public interface IGoatAssociationService
    {
        GoatAssociationModel Read();
        void Save(GoatAssociationModel Association);
    }
}
