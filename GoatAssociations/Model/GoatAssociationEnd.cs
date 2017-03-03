using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoatAssociations.Model
{
    enum NavigabilityType { Unspecified, Navigable, NonNavigable }
    enum AggregationType: int { None = 0, Shared = 1, Composite = 2}

    class GoatAssociationEnd
    {
        public NavigabilityType Navigability { get; set; }
        public AggregationType Aggregation { get; set; }
        public string Multiplicity { get; set; }
        public bool Derived { get; set; }
        public bool Union { get; set; }
        public bool IsOwnedByClassifier { get; set; }
        public string Role;
    }
}
