﻿
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
    // This is a simple variable
    public class Variable : Declaration
    {
        public TSType Type { get; set; }
        public bool Optional { get; set; }
        public bool Public { get; set; }
        public bool Static { get; set; }
        public bool hasString { get; set; }

        public Variable(Container<Declaration> super)
            : base(super)
        {
            this.Optional = false;
            this.Public = false;
            this.Static = false;
            this.hasString = false;
        }

        public Variable(XElement elem, Container<Declaration> super, TypeScriptDefContext context)
            : this(super)
        {
            var Child = elem.Elements();

            foreach (XElement e in Child)
            {
                switch (e.Name.LocalName)
                {
                    case "ident":
                        this.Name = e.Attribute("text").Value;
                        break;
                    case "question":
                        this.Optional = true;
                        break;
                    case "type":
                        this.Type = Tool.NewType(e, context);
                        break;
                    case "k_export":
                        this.Public = true;
                        break;
                    case "k_static":
                        this.Static = true;
                        break;
                    case "string":
                        this.hasString = true;
                        break;
                }
            }
        }

        public override string ToString()
        {
            string type;
            string attr = "";
            string at = "";

            if (this.Static)
                attr += "static ";
            attr += "public ";

            if (this.Type == null || this.Type.ToString() == "")
                type = "object";
            else
                type = this.Type.ToString();

            return attr + type + " " + at + Tool.ClearKeyWord(this.Name);
        }
    }
}
