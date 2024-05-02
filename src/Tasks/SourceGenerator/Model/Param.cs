
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

using System.Xml.Linq;

namespace TypeScriptDefToCSharp.Model
{
    public class Param : Variable
    {
        public bool Spread { get; set; }

        public Param(Container<Declaration> super)
            : base(super)
        {
        }

        public Param(XElement elem, Container<Declaration> super, bool spread, TypeScriptDefContext context)
            : base(elem, super, context)
        {
            this.Spread = spread;
        }

        public override string ToString()
        {
            string type = this.Type.ToString();
            string res;

            if (type == "double" || type == "bool")
                type += "?";

            if (this.Spread)
                return "params " + type + "[] " + Tool.ClearKeyWord(this.Name);
            else
                res = type + " " + Tool.ClearKeyWord(this.Name);

            if (this.Optional)
                res += " = null";
            return res;
        }
    }
}
