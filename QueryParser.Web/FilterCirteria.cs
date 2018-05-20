﻿using System;

namespace QueryParser.Web
{
    public class FilterCirteria<T>
    {
        public Func<T, object, bool> Predicate { get; }
        public object Value { get; }

        public FilterCirteria(object value, Func<T, object, bool> predicate)
        {
            Value = value;
            Predicate = predicate;
        }
    }
}
