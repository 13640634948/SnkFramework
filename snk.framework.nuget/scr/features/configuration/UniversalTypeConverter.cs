using System;

namespace SnkFramework.NuGet.Features
{
    namespace Configuration
    {
        public class SnkUniversalTypeConverter : ISnkTypeConverter
        {
            public virtual bool Support(Type type)
            {
                var typeCode = Type.GetTypeCode(type);

                switch (typeCode)
                {
                    case TypeCode.Boolean:
                    case TypeCode.Char:
                    case TypeCode.SByte:
                    case TypeCode.Byte:
                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                    case TypeCode.Int32:
                    case TypeCode.UInt32:
                    case TypeCode.Int64:
                    case TypeCode.UInt64:
                    case TypeCode.Single:
                    case TypeCode.Double:
                    case TypeCode.DateTime:
                    case TypeCode.String:
                    case TypeCode.Decimal:
                        return true;
                    default:
                        {
                            return type == typeof(Version);
                        }
                }
            }

            public virtual object Convert(Type type, object value)
            {
                TypeCode typeCode = Type.GetTypeCode(type);
                switch (typeCode)
                {
                    case TypeCode.Boolean:
                        if (value is string)
                        {
                            string v = ((string)value).Trim().ToLower();
                            if (v.Equals("yes") || v.Equals("true"))
                                return true;
                            else if (v.Equals("no") || v.Equals("false"))
                                return false;
                            else
                                throw new FormatException();
                        }
                        else
                        {
                            return System.Convert.ChangeType(value, type);
                        }
                    case TypeCode.Char:
                    case TypeCode.SByte:
                    case TypeCode.Byte:
                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                    case TypeCode.Int32:
                    case TypeCode.UInt32:
                    case TypeCode.Int64:
                    case TypeCode.UInt64:
                    case TypeCode.Single:
                    case TypeCode.Double:
                    case TypeCode.DateTime:
                    case TypeCode.String:
                    case TypeCode.Decimal:
                        return System.Convert.ChangeType(value, type);
                    default:
                        {
                            if (type == typeof(Version))
                            {
                                if (value is Version)
                                    return (Version)value;

                                if (!(value is string))
                                    throw new FormatException(string.Format("This value \"{0}\" cannot be converted to the type \"{1}\"", value, type.Name));

                                try
                                {
                                    return new Version((string)value);
                                }
                                catch (Exception e)
                                {
                                    throw new FormatException(string.Format("This value \"{0}\" cannot be converted to the type \"{1}\"", value, type.Name), e);
                                }
                            }

                            throw new FormatException(string.Format(
                                "This value \"{0}\" cannot be converted to the type \"{1}\"", value, type.Name));
                        }
                }
            }
        }
    }
}