using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoatAssociations.Model
{
    enum NavigabilityType { Unspecified, Navigable, NonNavigable }
    enum AggregationType: int { None = 0, Shared = 1, Composite = 2}

    class GoatAssociationEnd: NotifyPropertyClass
    {
        public NavigabilityType Navigability { get; set; } = NavigabilityType.Unspecified;
        public AggregationType Aggregation { get; set; } = AggregationType.None;
        public string Multiplicity { get; set; } = "";
        public bool Derived { get; set; } = false;
        public bool Union { get; set; } = false;
        public bool IsOwnedByClassifier { get; set; } = false;

        private string _role = "";
        public string Role
        {
            get
            {
                return _role;
            }
            set
            {
                if (_role != value)
                {
                    _role = value;
                    this.OnPropertyChanged(nameof(Role));
                }
            }
        }
    }
}
