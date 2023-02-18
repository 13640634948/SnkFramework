using System;

namespace SnkFramework.NuGet.Basic
{
    namespace TypeConverter
    {
        public interface ISnkTypeConverter
        {
            bool Support(Type type);

            object Convert(Type type, object value);
        }
    }
}