
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

using System.Collections.Generic;
using System.Linq;

namespace TypeScriptDefToCSharp.Model
{
    public class UnionTypeModel : TSType
    {
        public string Name { get { return this.ToString(); } set {} }
        public List<TSType> Types { get; set; }
        public int Count
        {
            get
            {
                return this.Types.Count();
            }
        }

        public UnionTypeModel()
        {
            this.Types = new List<TSType>();
        }

        public override string ToString()
        {
            string typeList = "";

            for (int i = 0; i < this.Types.Count; ++i)
            {
                if (i > 0)
                    typeList += ", ";
                typeList += this.Types[i].ToString();
            }

            return "UnionType<" + typeList + ">";
        }

        public string New(string jsObj)
        {
            return this.ToString() + ".FromJavaScriptInstance(" + jsObj + ")";
        }
    }
}
