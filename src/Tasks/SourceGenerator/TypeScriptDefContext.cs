
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

namespace TypeScriptDefToCSharp
{
    public class TypeScriptDefContext
    {
        // Directory where the files are outputed (obj directory)
        public string OutputDirectory { get; set; }

        // List of generated files to add them for compilation
        public List<string> CurrentGeneratedFiles { get; set; }

        public TypeScriptDefContext()
        {
            CurrentGeneratedFiles = new List<string>();
        }
    }
}
