using System;
using System.Collections.Generic;
using System.Linq;

namespace QueryParser.Web
{
    public class FilterCirteria
    {
        public string PropertyPath { get; set; }
        public Func<object, object, bool> Predicate { get; set; }
        public object Value { get; set; }
    }

    public struct FilterPredicate : Func<object,object,bool>
    {

    }
}
