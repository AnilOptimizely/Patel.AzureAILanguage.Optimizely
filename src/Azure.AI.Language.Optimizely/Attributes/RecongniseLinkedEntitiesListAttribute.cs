﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.AI.Language.Optimizely.Attributes
{
    public class RecongniseLinkedEntitiesListAttribute : TextAnalyticsBaseContentAttribute
    {
        public override bool AnalyzeCMSContent => true;
    }
}
