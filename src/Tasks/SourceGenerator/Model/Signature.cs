
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
    public class Signature : Method
    {
        public Signature(Container<Declaration> super) : base(super)
        {
        }

        public Signature(XElement elem, Container<Declaration> super, TypeScriptDefContext context)
            : base(elem, super, context)
        {
        }

        public override string ToString()
        {
            string res = "";
            bool any = (this.ReturnType != null && this.ReturnType is BasicType &&
                ((BasicType)this.ReturnType).Name == "any");

            if (this.Static)
                res = "static ";
            if (this.ReturnType == null)
                res += "void ";
            res += this.Name + "(";

            for (int i = 0; i < this.Params.Count; ++i)
            {
                if (i > 0)
                    res += ", ";
                res += this.Params[i].ToString();
            }

            return res;
        }
    }
}
