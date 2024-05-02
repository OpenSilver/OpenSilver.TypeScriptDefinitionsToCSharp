
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
    public class TypeScriptDefToCSharpOutput
    {
        public TypeScriptDefToCSharpOutput()
        {
            TypeScriptDefinitionFiles = new List<TypeScriptDefinitionFile>();
        }

        public List<TypeScriptDefinitionFile> TypeScriptDefinitionFiles { get; set; }

        public TypeScriptDefinitionFile ExtractFileByName(string fileName)
        {
            foreach (var file in TypeScriptDefinitionFiles)
            {
                if (fileName == file.FileName)
                {
                    TypeScriptDefinitionFiles.Remove(file);
                    return file;
                }
            }
            return null;
        }
    }

    public class TypeScriptDefinitionFile
    {
        public TypeScriptDefinitionFile()
        {
            CSharpGeneratedFiles = new List<string>();
        }

        public string FileName { get; set; }
        public string FileContentHash { get; set; }
        public List<string> CSharpGeneratedFiles { get; set; }
    }


}
