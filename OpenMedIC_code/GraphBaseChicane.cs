using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMedIC
{
    public class GraphBaseChicane: GraphBase
    {
        //this is a workaround to VS designer not liking the base class being abstract
        //but it's ok if your base class's base class is abstract
        //so this provides a non-abstract wrapper around GraphBase
        //see: https://stackoverflow.com/questions/1620847/how-can-i-get-visual-studio-2008-windows-forms-designer-to-render-a-form-that-im/2406058#2406058
    }
}
