using System;
using System.Text.RegularExpressions;
using SnkFramework.NuGet.Features.Configuration;
using UnityEngine;

namespace SnkFramework.Runtime.configuration
{
    public class SnkUnityEngineTypeConverter : ISnkTypeConverter
    {
        public virtual bool Support(Type type)
        {
#if NETFX_CORE
            TypeCode typeCode = WinRTLegacy.TypeExtensions.GetTypeCode(type);
#else
            var typeCode = Type.GetTypeCode(type);
#endif
            if (type == typeof(Color))
                return true;
            if (type == typeof(Vector2))
                return true;
            if (type == typeof(Vector3))
                return true;
            if (type == typeof(Vector4))
                return true;
            return type == typeof(Rect);
        }

        public virtual object Convert(Type type, object value)
        {
#if NETFX_CORE
            TypeCode typeCode = WinRTLegacy.TypeExtensions.GetTypeCode(type);
#else
            var typeCode = Type.GetTypeCode(type);
#endif
            if (type == typeof(Color))
            {
                if (value is Color color1)
                    return color1;

                if (value is string == false)
                    throw new FormatException(
                        string.Format("This value \"{0}\" cannot be converted to the type \"{1}\"", value, type.Name));

                try
                {
                    if (ColorUtility.TryParseHtmlString((string)value, out var color))
                        return color;
                }
                catch (Exception e)
                {
                    throw new FormatException(
                        string.Format("This value \"{0}\" cannot be converted to the type \"{1}\"", value, type.Name),
                        e);
                }
            }
            else if (type == typeof(Vector2))
            {
                if (value is Vector2 vector2)
                    return vector2;

                if (value is string == false)
                    throw new FormatException(
                        string.Format("This value \"{0}\" cannot be converted to the type \"{1}\"", value, type.Name));

                try
                {
                    var val = Regex.Replace(((string)value).Trim(), @"(^\()|(\)$)", "");
                    var s = val.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (s.Length == 2)
                        return new Vector2(float.Parse(s[0]), float.Parse(s[1]));
                }
                catch (Exception e)
                {
                    throw new FormatException(
                        string.Format("This value \"{0}\" cannot be converted to the type \"{1}\"", value, type.Name),
                        e);
                }
            }
            else if (type == typeof(Vector3))
            {
                if (value is Vector3 vector3)
                    return vector3;
                
                if (value is string == false)
                    throw new FormatException(
                        string.Format("This value \"{0}\" cannot be converted to the type \"{1}\"", value, type.Name));

                try
                {
                    var val = Regex.Replace(((string)value).Trim(), @"(^\()|(\)$)", "");
                    var s = val.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (s.Length == 3)
                        return new Vector3(float.Parse(s[0]), float.Parse(s[1]), float.Parse(s[2]));
                }
                catch (Exception e)
                {
                    throw new FormatException(
                        string.Format("This value \"{0}\" cannot be converted to the type \"{1}\"", value, type.Name),
                        e);
                }
            }
            else if (type == typeof(Vector4))
            {
                if (value is Vector4)
                    return (Vector4)value;

                if (value is string == false)
                    throw new FormatException(
                        string.Format("This value \"{0}\" cannot be converted to the type \"{1}\"", value, type.Name));

                try
                {
                    var val = Regex.Replace(((string)value).Trim(), @"(^\()|(\)$)", "");
                    string[] s = val.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (s.Length == 4)
                        return new Vector4(float.Parse(s[0]), float.Parse(s[1]), float.Parse(s[2]), float.Parse(s[3]));
                }
                catch (Exception e)
                {
                    throw new FormatException(
                        string.Format("This value \"{0}\" cannot be converted to the type \"{1}\"", value, type.Name),
                        e);
                }
            }
            else if (type == typeof(Rect))
            {
                if (value is Rect rect)
                    return rect;

                if (value is string == false)
                    throw new FormatException(
                        string.Format("This value \"{0}\" cannot be converted to the type \"{1}\"", value, type.Name));

                try
                {
                    var val = Regex.Replace(((string)value).Trim(), @"(^\()|(\)$)", "");
                    var s = val.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (s.Length == 4)
                        return new Rect(float.Parse(s[0]), float.Parse(s[1]), float.Parse(s[2]), float.Parse(s[3]));
                }
                catch (Exception e)
                {
                    throw new FormatException(
                        string.Format("This value \"{0}\" cannot be converted to the type \"{1}\"", value, type.Name),
                        e);
                }
            }

            throw new FormatException(string.Format("This value \"{0}\" cannot be converted to the type \"{1}\"", value,
                type.Name));
        }
    }
}