
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

namespace TypeScriptDefToCSharp.Model
{
    public class Array : TSType
    {
        public string Name
        {
            get
            {
                return this.Type.Name + "[]";
            }
            set { }
        }

        public TSType Type { get; private set; }

        public Array(TSType type)
        {
            this.Type = type;
        }

        public string New(string jsObj)
        {
            return "JSObject.FromJavaScriptInstance<" + this.ToString() + ">(" + jsObj + ")";
        }

        public override string ToString()
        {
            return "JSArray<" + this.Type.ToString() + ">";
        }
    }
}
