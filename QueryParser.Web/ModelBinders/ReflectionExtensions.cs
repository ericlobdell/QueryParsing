using System;
using System.Collections.Generic;

namespace QueryParser.Web.ModelBinders
{
    public static class ReflectionExtensions
    {
        public static Object GetPropValue(this Object obj, String propName)
        {
            var nameParts = propName.Split('.');
            if ( nameParts.Length == 1 )
            {
                return obj.GetType().GetProperty(propName).GetValue(obj, null);
            }

            foreach ( String part in nameParts )
            {
                if ( obj == null ) { return null; }

                var type = obj.GetType();
                var info = type.GetProperty(part);
                if ( info == null ) { return null; }

                obj = info.GetValue(obj, null);
            }
            return obj;
        }

        public static List<string> GetPropertyPaths(this Type t)
        {
            List<string> GetPaths(Type type, string parent, List<string> paths)
            {
                var properties = type.GetProperties();

                foreach ( var property in properties )
                {
                    var path = string.IsNullOrWhiteSpace(parent)
                        ? property.Name
                        : $"{parent}.{property.Name}";

                    if ( property.PropertyType.Assembly == type.Assembly )
                        GetPaths(property.PropertyType, path, paths);

                    paths.Add(path);
                }

                return paths;
            }

            return GetPaths(t, "", new List<string>());
        }
    }
}
