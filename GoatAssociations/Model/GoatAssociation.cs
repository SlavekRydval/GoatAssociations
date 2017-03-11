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


        public GoatAssociation()
        {
            Left.PropertyChanged += LeftRight_PropertyChanged;
            Right.PropertyChanged += LeftRight_PropertyChanged;
        }

        //MVVM question: Is this really Model stuff or should it be moved to ViewModel?
        private void LeftRight_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(GoatAssociationEnd.Aggregation))
            {
                if (sender == Left)
                {
                    if (Left.Aggregation != AggregationType.None)
                        Right.Aggregation = AggregationType.None;
                }
                else
                if (sender == Right)
                {
                    if (Right.Aggregation != AggregationType.None)
                        Left.Aggregation = AggregationType.None;
                }

            }
        }
    }
}
