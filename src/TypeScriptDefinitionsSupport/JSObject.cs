
/*===================================================================================
* 
*   Copyright (c) Userware/OpenSilver.net
*      
*   This file is part of the OpenSilver Runtime (https://opensilver.net), which is
*   licensed under the MIT license: https://opensource.org/licenses/MIT
*   
*   As stated in the MIT license, "the above copyright notice and this permission
*   notice shall be included in all copies or substantial portions of the Software."
*  
\*====================================================================================*/

using System;

namespace TypeScriptDefinitionsSupport
{
    public interface IJSObject
    {
        object UnderlyingJSInstance { get; set; }
    }

    public class JSObject : IJSObject
    {
        public static readonly JSObject Undefined = new JSObject(OpenSilver.Interop.ExecuteJavaScript("undefined"));
        public static readonly JSObject Null = new JSObject(OpenSilver.Interop.ExecuteJavaScript("null"));

        public object UnderlyingJSInstance { get; set; }

        public JSObject()
        {
            // Use new Object() instead of {} syntax, because the latter
            // is interpreted as undefined in the Simulator
            this.UnderlyingJSInstance = OpenSilver.Interop.ExecuteJavaScript("new Object()");
        }

        public JSObject(object jsObj)
        {
            this.UnderlyingJSInstance = jsObj;
        }

        static public T FromJavaScriptInstance<T>(object jsInstance) where T : IJSObject, new()
        {
            var t = new T();
            t.UnderlyingJSInstance = jsInstance;
            return t;
        }

        // ----------------------
        // Cast operator from standard C# types
        // ----------------------

        static public implicit operator JSObject(bool b)
        {
            return new JSObject(b);
        }

        static public implicit operator JSObject(byte b)
        {
            return new JSObject(b);
        }

        static public implicit operator JSObject(sbyte s)
        {
            return new JSObject(s);
        }

        static public implicit operator JSObject(char c)
        {
            return new JSObject(c);
        }

        static public implicit operator JSObject(decimal d)
        {
            return new JSObject(d);
        }

        static public implicit operator JSObject(double d)
        {
            return new JSObject(d);
        }

        static public implicit operator JSObject(float f)
        {
            return new JSObject(f);
        }

        static public implicit operator JSObject(int i)
        {
            return new JSObject(i);
        }

        static public implicit operator JSObject(uint u)
        {
            return new JSObject(u);
        }

        static public implicit operator JSObject(long l)
        {
            return new JSObject(l);
        }

        static public implicit operator JSObject(ulong u)
        {
            return new JSObject(u);
        }

        static public implicit operator JSObject(short s)
        {
            return new JSObject(s);
        }

        static public implicit operator JSObject(ushort u)
        {
            return new JSObject(u);
        }

        static public implicit operator JSObject(string s)
        {
            return new JSObject(s);
        }

        static public JSObject CreateFrom(object o)
        {
            if (o is JSObject)
                return (JSObject)o;
            else if (o is IJSObject)
                return new JSObject(((IJSObject)o).UnderlyingJSInstance);
            else
                return new JSObject(o);
        }

        public override bool Equals(object obj)
        {
            if (obj is IJSObject)
                return ((IJSObject)obj).UnderlyingJSInstance == this.UnderlyingJSInstance;
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool Helper_IsBuiltInType<T>()
        {
            return typeof(T) == typeof(int) ||
                   typeof(T) == typeof(float) ||
                   typeof(T) == typeof(double) ||
                   typeof(T) == typeof(string) ||
                   typeof(T) == typeof(bool) ||
                   typeof(T) == typeof(uint) ||
                   typeof(T) == typeof(short) ||
                   typeof(T) == typeof(ushort) ||
                   typeof(T) == typeof(long) ||
                   typeof(T) == typeof(ulong) ||
                   typeof(T) == typeof(decimal) ||
                   typeof(T) == typeof(byte) ||
                   typeof(T) == typeof(sbyte);
        }

        public static T Helper_ConvertTo<T>(object jsObj)
        {
            if (typeof(T) == typeof(int))
                return (T)(object)Convert.ToInt32(jsObj);
            else if (typeof(T) == typeof(float))
                return (T)(object)Convert.ToSingle(jsObj);
            else if (typeof(T) == typeof(double))
                return (T)(object)Convert.ToDouble(jsObj);
            else if (typeof(T) == typeof(string))
                return (T)(object)Convert.ToString(jsObj);
            else if (typeof(T) == typeof(bool))
                return (T)(object)Convert.ToBoolean(jsObj);
            else if (typeof(T) == typeof(uint))
                return (T)(object)Convert.ToUInt32(jsObj);
            else if (typeof(T) == typeof(short))
                return (T)(object)Convert.ToInt16(jsObj);
            else if (typeof(T) == typeof(ushort))
                return (T)(object)Convert.ToUInt16(jsObj);
            else if (typeof(T) == typeof(long))
                return (T)(object)Convert.ToInt64(jsObj);
            else if (typeof(T) == typeof(ulong))
                return (T)(object)Convert.ToUInt64(jsObj);
            else if (typeof(T) == typeof(decimal))
                return (T)(object)Convert.ToDecimal(jsObj);
            else if (typeof(T) == typeof(byte))
                return (T)(object)Convert.ToByte(jsObj);
            else if (typeof(T) == typeof(sbyte))
                return (T)(object)Convert.ToSByte(jsObj);
            else
                throw new Exception("The type '" + typeof(T).Name + "' is not a built-in type");
        }
    }
}
