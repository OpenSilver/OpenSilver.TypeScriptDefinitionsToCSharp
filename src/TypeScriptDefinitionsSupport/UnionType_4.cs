
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
    public class UnionType<T0, T1, T2, T3> : UnionType
    {
        private UnionType(object jsObj)
        {
            this.UnderlyingJSInstance = jsObj;
        }

        public static UnionType<T0, T1, T2, T3> FromJavaScriptInstance(object jsObj)
        {
            return new UnionType<T0, T1, T2, T3>(jsObj);
        }
        private T0 _t0 { get; set; }

        public UnionType(T0 t)
        {
            this._t0 = t;
            this._Type = typeof(T0);
            JSObject o = JSObject.CreateFrom(t);;
            this.UnderlyingJSInstance = o.UnderlyingJSInstance;
        }

        static public implicit operator UnionType<T0, T1, T2, T3>(T0 t)
        {
            return new UnionType<T0, T1, T2, T3>(t);
        }

        static public implicit operator T0(UnionType<T0, T1, T2, T3> value)
        {
            if (value._Type == null)
            {
                value._t0 = value.CreateInstance<T0>();
                value._Type = typeof(T0);
                return value._t0;
            }
            if (value._Type == typeof(T0))
                return value._t0;
            else
                throw new Exception("Unable to cast this UnionType to " + typeof(T0).Name + " because it contains a value that is of another type.");
        }
        private T1 _t1 { get; set; }

        public UnionType(T1 t)
        {
            this._t1 = t;
            this._Type = typeof(T1);
            JSObject o = JSObject.CreateFrom(t);;
            this.UnderlyingJSInstance = o.UnderlyingJSInstance;
        }

        static public implicit operator UnionType<T0, T1, T2, T3>(T1 t)
        {
            return new UnionType<T0, T1, T2, T3>(t);
        }

        static public implicit operator T1(UnionType<T0, T1, T2, T3> value)
        {
            if (value._Type == null)
            {
                value._t1 = value.CreateInstance<T1>();
                value._Type = typeof(T1);
                return value._t1;
            }
            if (value._Type == typeof(T1))
                return value._t1;
            else
                throw new Exception("Unable to cast this UnionType to " + typeof(T1).Name + " because it contains a value that is of another type.");
        }
        private T2 _t2 { get; set; }

        public UnionType(T2 t)
        {
            this._t2 = t;
            this._Type = typeof(T2);
            JSObject o = JSObject.CreateFrom(t);;
            this.UnderlyingJSInstance = o.UnderlyingJSInstance;
        }

        static public implicit operator UnionType<T0, T1, T2, T3>(T2 t)
        {
            return new UnionType<T0, T1, T2, T3>(t);
        }

        static public implicit operator T2(UnionType<T0, T1, T2, T3> value)
        {
            if (value._Type == null)
            {
                value._t2 = value.CreateInstance<T2>();
                value._Type = typeof(T2);
                return value._t2;
            }
            if (value._Type == typeof(T2))
                return value._t2;
            else
                throw new Exception("Unable to cast this UnionType to " + typeof(T2).Name + " because it contains a value that is of another type.");
        }
        private T3 _t3 { get; set; }

        public UnionType(T3 t)
        {
            this._t3 = t;
            this._Type = typeof(T3);
            JSObject o = JSObject.CreateFrom(t);;
            this.UnderlyingJSInstance = o.UnderlyingJSInstance;
        }

        static public implicit operator UnionType<T0, T1, T2, T3>(T3 t)
        {
            return new UnionType<T0, T1, T2, T3>(t);
        }

        static public implicit operator T3(UnionType<T0, T1, T2, T3> value)
        {
            if (value._Type == null)
            {
                value._t3 = value.CreateInstance<T3>();
                value._Type = typeof(T3);
                return value._t3;
            }
            if (value._Type == typeof(T3))
                return value._t3;
            else
                throw new Exception("Unable to cast this UnionType to " + typeof(T3).Name + " because it contains a value that is of another type.");
        }
    }
}