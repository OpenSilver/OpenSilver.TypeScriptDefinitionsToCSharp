
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

using System.Linq;
using System.Xml.Linq;

namespace TypeScriptDefToCSharp.Model
{
    // The Global Program is the namespace that will contain the content of one file (.d.ts, or xml now)
    public class GlobalProgram : Namespace
    {
        public GlobalProgram(TypeScriptDefContext context)
            : base(null, context)
        {
        }

        // This is simply boring XML parsing, should've used deserialization but at this time I was a noob sorry
        public GlobalProgram(XDocument doc, string name, TypeScriptDefContext context)
            : this(context)
        {
            this.Name = name;
            var Child = doc.Root.Elements();
            string[] path = name.Split('.');
            this.StaticClass.Name = path.Last() + "Class";

            foreach (XElement elem in Child)
            {
                switch (elem.Name.LocalName)
                {
                    case "import":
                        this.Imports.Add(new Import(elem));
                        break;
                    case "namespace":
                        this.AddChildNamespace(elem, context); // We don't add it directly because maybe the namespace already exists
                        break;
                    // Variable directly in the namespace
                    case "variable":
                        this.StaticClass.Properties.Add(new Property(elem, this.StaticClass, context) { Static = true });
                        break;
                    // Function directly in the namespace
                    case "function":
                        this.StaticClass.Methods.Add(new Method(elem, this.StaticClass, context) { Static = true });
                        break;
                    case "interface":
                        this.Declarations.Add(TypeDeclaration.New(elem, this, context));
                        break;
                    case "class":
                        this.Declarations.Add(TypeDeclaration.New(elem, this, context));
                        break;
                }
            }
        }
    }
}
