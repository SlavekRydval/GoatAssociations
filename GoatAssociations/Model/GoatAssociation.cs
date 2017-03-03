using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoatAssociations.Model
{
    class GoatAssociation
    {
        public GoatAssociationEnd Left { get; set; } = new GoatAssociationEnd();
        public GoatAssociationEnd Right { get; set; } = new GoatAssociationEnd();
    }
}
