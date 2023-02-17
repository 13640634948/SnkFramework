using System;

namespace SnkFramework.NuGet.Features
{
    namespace Configuration
    {
        public interface ISnkTypeConverter
        {
            bool Support(Type type);

            object Convert(Type type, object value);
        }
    }
}