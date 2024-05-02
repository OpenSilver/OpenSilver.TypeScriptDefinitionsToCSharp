
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

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using TinyPG;
using TypeScriptDefToCSharp;
using TypeScriptDefToCSharp.Model;

namespace OpenSilver.Compiler
{
    public class TypeScriptDefToCSharp : Task
    {
        private const string PARSER_VERSION = "0001"; // Increase this value when you want to force-regenerate the files in the end-users solutions. This is useful when we release a breaking change and we want the end-user to force-recompile the TypeScript Definition files.
        private const string NAME_OF_TYPESCRIPT_DEFINITIONS_CACHE_FILE = "TypeScriptDefinitionsCache.xml";

        public string InputFiles { get; set; }

        [Required]
        public string OutputDirectory { get; set; }

        [Output]
        public ITaskItem[] GeneratedFiles { get; set; }

        public bool NoRecompile { get; set; }

        public override bool Execute()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

            if (InputFiles == null)
            {
                return true;
            }

            try
            {
                // Execute the task if needed
                if (!NoRecompile)
                {
                    ProcessTypeScriptDefFile();
                }

                // Get the file list from the XML
                List<string> ListOfGeneratedFiles = new List<string>();
                if (File.Exists(OutputDirectory + NAME_OF_TYPESCRIPT_DEFINITIONS_CACHE_FILE))
                {
                    // Read the XML
                    string content = File.ReadAllText(OutputDirectory + NAME_OF_TYPESCRIPT_DEFINITIONS_CACHE_FILE);
                    // Deserialize
                    TypeScriptDefToCSharpOutput output = Tool.Deserialize<TypeScriptDefToCSharpOutput>(content);

                    // Store every generated file in our list
                    foreach (var file in output.TypeScriptDefinitionFiles)
                    {
                        ListOfGeneratedFiles.AddRange(file.CSharpGeneratedFiles);
                    }
                }
                // Convert them to get the output
                GeneratedFiles = ListOfGeneratedFiles.Select(s => new TaskItem() { ItemSpec = s }).ToArray();

                Log.LogMessage(MessageImportance.High, $"Output directory: {OutputDirectory}");
            }
            catch (Exception ex)
            {
                Log.LogErrorFromException(ex, true, true, null);
            }

            return !Log.HasLoggedErrors;
        }

        // Transform every TypeScript Definition file into XML and then into C# Wrapper
        private void ProcessTypeScriptDefFile()
        {
#if GENERATE_UNION_TYPES
            // Use that if you want to regenerate the UnionType.cs file
            // Recompile a project that use TypeScript Definition file
            // Go to the output and copy/paste the block
            // Then just use a small regex to replace like this: change '^>1  ' to '' (don't forget regex mode)
            Logger.WriteMessage(Tool.GenerateUnionType(6));
#endif

            //--------------------------------------------------------
            // PREPARE FOR PARSING
            //--------------------------------------------------------

            // Create the context
            TypeScriptDefContext context = new TypeScriptDefContext()
            {
                OutputDirectory = OutputDirectory,
            };

            // Reset AnonymousTypes static properties
            AnonymousType.Anons = new List<AnonymousType>();
            AnonymousType.AnonID = 1;

            // Create and load the Infos XML file
            TypeScriptDefToCSharpOutput DTSOld;
            TypeScriptDefToCSharpOutput DTSNew = new TypeScriptDefToCSharpOutput();

            if (File.Exists(OutputDirectory + NAME_OF_TYPESCRIPT_DEFINITIONS_CACHE_FILE))
            {
                string text = File.ReadAllText(OutputDirectory + NAME_OF_TYPESCRIPT_DEFINITIONS_CACHE_FILE);

                DTSOld = Tool.Deserialize<TypeScriptDefToCSharpOutput>(text);
            }
            else
            {
                DTSOld = new TypeScriptDefToCSharpOutput();
            }

            // Store an array with every .d.ts files
            string[] args = InputFiles.Split(';');

            //--------------------------------------------------------
            // PARSING OF TYPESCRIPT DEFINITION FILES (.d.ts)
            //--------------------------------------------------------

            // Convert every TypeScript Definition file into an XML file
            // We need to add a check with a hash to know if there is already the XML file
            for (int i = 0; i < args.Length; i++)
            {
                // Remove the ".d.ts" part of the name
                args[i] = args[i].Remove(args[i].Length - 5);

                // Read the file
                string TSDefinitionOriginal = File.ReadAllText(args[i] + ".d.ts");

                // Remove the comments
                string TSDefinition = ClearComments(TSDefinitionOriginal);

                // Calculate the file's hash
                string fileHash = Tool.GetHashString(TSDefinition) + PARSER_VERSION; // See note near the "PARSER_VERSION" declaration.
                // Try to remove the file from the Old Infos (null if not found)
                var file = DTSOld.ExtractFileByName(args[i] + ".d.ts");

                //------- PARSE THE FILE ONLY IF NEEDED -------

                // If there is no file with that name, or a different hash, or no corresponding XML
                if (file == null || file.FileContentHash != fileHash || !File.Exists(OutputDirectory + args[i] + ".xml"))
                {
                    string fileName = args[i] + ".d.ts";

                    Log.LogMessage($"Parsing file: {fileName}");

                    // Add this file to the New Infos
                    DTSNew.TypeScriptDefinitionFiles.Add(new TypeScriptDefinitionFile()
                    {
                        FileName = args[i] + ".d.ts",
                        FileContentHash = fileHash
                    });

                    //------- PARSING -------

                    // Use TinyPG generated classes to parse the file
                    Scanner Scanner = new Scanner(LogProgress);
                    Parser Parser = new Parser(Scanner);
                    ParseTree Tree = Parser.Parse(TSDefinition);

                    // HOW TO DEBUG THE PARSER: Place a breakpoint after this line and create a Watch with the following expression: Tree.PrintTree()

                    //------- ERROR HANDLING -------

                    if (Tree.Errors.Any())
                    {
                        // Used to disable error duplication
                        var CheckDuplicate = new List<int>();

                        // Loop on every parsing error
                        foreach (var err in Tree.Errors)
                        {
                            // Check if the error was already handled
                            if (!CheckDuplicate.Contains(err.Position))
                            {
                                // Calculate the line and col in the file INCLUDING THE COMMENTS
                                int line = CountLine(TSDefinitionOriginal, err.Position, out int col);
                                // Print a pretty message
                                Log.LogError(string.Empty, string.Empty, string.Empty, string.Empty,
                                    args[i] + ".d.ts", line, col, 0, 0,
                                    "Parsing error: invalid TypeScript Definition or unsupported feature");
                                // Add this error position to handle it once
                                CheckDuplicate.Add(err.Position);
                            }
                        }
                        // Leave the compilation
                        throw new Exception();
                    }

                    //------- XML EXPORT -------

                    // Create an XMLExporter
                    XMLExporter Exporter = new XMLExporter(OutputDirectory + args[i] + ".xml");
                    // Store and remove useless nodes from the tree
                    XMLExporter.ClearParseNode(Tree);
                    // Export the tree as a XML file
                    Exporter.PrintBasicTree(Tree);
                }
                else
                {
                    DTSNew.TypeScriptDefinitionFiles.Add(file);
                }
            }

            // Generate an intermediate version of the "NAME_OF_TYPESCRIPT_DEFINITIONS_CACHE_FILE" file so
            // that, if the export crashes, we don't re-parse again the TypeScript files
            // and, instead, we re-use the generated XML files:
            string newOutput = Tool.Serialize(DTSNew);
            File.WriteAllText(OutputDirectory + NAME_OF_TYPESCRIPT_DEFINITIONS_CACHE_FILE, newOutput);

            //--------------------------------------------------------
            // PARSING XML AND GENERATE C# CODE
            //--------------------------------------------------------

            //------- CHECK IF GENERATION IS NEEDED -------

            // If only one generated file is missing we need to generate all of them
            // If we parsed a new .d.ts, we have new file to generate
            bool needToExport = false;
            for (int i = 0; i < args.Length && needToExport == false; i++)
            {
                var file = DTSNew.TypeScriptDefinitionFiles[i];

                foreach (var f in file.CSharpGeneratedFiles)
                {
                    if (!File.Exists(f))
                    {
                        needToExport = true;
                    }
                }

                if (file.CSharpGeneratedFiles.Count == 0) // If there is no file, it's probably a new .d.ts
                {
                    needToExport = true;
                }
            }

            //------- IF NEEDED GENERATE THE FILES -------

            if (needToExport)
            {
                // Loop on every XML
                for (int i = 0; i < args.Length; i++)
                {
                    var file = DTSNew.TypeScriptDefinitionFiles[i];
                    context.CurrentGeneratedFiles = file.CSharpGeneratedFiles = new List<string>();

                    // Load the XML file
                    var Doc = XDocument.Load(OutputDirectory + args[i] + ".xml");
                    // Create the model
                    string fileName = Tool.RemoveDotIdentAdditionalChars(args[i].Replace('\\', '.')); // Note: we remove the unsupported characters because the TypeScript module name may contain some (such as forward slashes and dashes).
                    var Prog = new GlobalProgram(Doc, fileName, context);

                    // Add others files as "dependency"
                    foreach (string Others in args)
                    {
                        if (Others != args[i])
                        {
                            string n = Others.Replace('\\', '.');
                            n = Tool.RemoveDotIdentAdditionalChars(n); // Note: we remove the unsupported characters because the TypeScript Definition file name may contain some (such as dashes).
                            Prog.Imports.Add(new Import() { Alias = n, Name = n });
                        }
                    }
                    // Export the model as C#
                    Prog.Export(context);
                }

                // Write the Infos XML file before exporting AnonymousTypes, because these types
                // were added at the construction (allow associating them to a .d.ts file)
                newOutput = Tool.Serialize(DTSNew);
                File.WriteAllText(OutputDirectory + NAME_OF_TYPESCRIPT_DEFINITIONS_CACHE_FILE, newOutput);

                // Create the AnonymousTypes namespace
                Namespace Anon = new Namespace(null, context) { Name = "AnonymousTypes" };

                // Add every namespace as a dependency for the AnonymousTypes namespace
                foreach (string Others in args)
                {
                    var n = Tool.RemoveDotIdentAdditionalChars(Others); // Note: we remove the unsupported characters because the TypeScript Definition file name may contain some (such as dashes).
                    Anon.Imports.Add(new Import() { Alias = n, Name = n });
                }

                // Export every AnonymousType
                Directory.CreateDirectory(OutputDirectory + "AnonymousTypes");
                foreach (AnonymousType a in AnonymousType.Anons)
                {
                    a.Class.Super = Anon;
                    a.Class.Export(context);
                }
            }
            else
            {
                Log.LogMessage(@"TypeScript Definition files were skipped because no change was detected. To force the reprocessing of TypeScript Definition files, manually delete the file ""obj\Debug\" + NAME_OF_TYPESCRIPT_DEFINITIONS_CACHE_FILE + @""" and rebuild.");
            }
        }

        private void LogProgress(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                Log.LogMessage($"  Processing token '{token}'");
            }
        }

        // Return a string with comments removed (Line Comment "// ..." and Block Comment "/* ... */")
        private static string ClearComments(string input)
        {
            var result = new StringBuilder();

            for (int i = 0; i < input.Length; i++)
            {
                // If it's a line comment
                if (input[i] == '/' && input[i + 1] == '/')
                {
                    // Move forward until the end of line
                    while (input[i] != '\n')
                        i++;
                }
                // If it's a block comment
                else if (input[i] == '/' && input[i + 1] == '*')
                {
                    // Move forward until the end of the comment
                    while (input[i] != '*' || input[i + 1] != '/')
                        i++;
                    i++;
                }
                // Else simply append the charater to the resulting string
                else
                    result.Append(input[i]);
            }
            return result.ToString();
        }

        // pos is the position of the character where the error occured in the comment cleared file
        // we want to calculate the same character's position in the original file (with comments)
        private static int CountLine(string input, int pos, out int column)
        {
            // Init the line and column value
            int line = 1; // We increment line every time a '\n' is encountered (even in comments)
            column = 1; // We increment column at every character and reset it to 1 at every line incementation
            int p = 0; // Current cursor position, we increment it at every character not in a comment

            for (int i = 0; i < input.Length; i++)
            {
                // If it's a line comment
                if (input[i] == '/' && input[i + 1] == '/')
                {
                    // Move forward until the end of line
                    while (input[i] != '\n')
                        i++;
                    line++; // It's a line comment so we increment the line
                    column = 1; // And reset the column
                }
                // If it's a block comment
                else if (input[i] == '/' && input[i + 1] == '*')
                {
                    // Move forward until the end of the comment
                    while (input[i] != '*' || input[i + 1] != '/')
                    {
                        i++;
                        column++; // Increment the column at each character
                        if (input[i] == '\n')
                        {
                            line++; // If it's a newline, we increment the line and reset column
                            column = 1;
                        }
                    }
                    i++;
                }
                // If it's a simple newline (not in comment), update line and column, and increment the cursor
                else if (input[i] == '\n')
                {
                    line++;
                    p++;
                    column = 1;
                }
                // If it's a normal character, increment the column and the cursor
                else
                {
                    p++;
                    column++;
                }
                // If the cursor (not including comment) is at the error position, we exit the loop
                if (p >= pos)
                    break;
            }

            return line;
        }
    }
}
