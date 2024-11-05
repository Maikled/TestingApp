using System.Reflection;

namespace TestingApp.Core.Processing.Compillers.CSharp
{
    public class CSharpReplacer
    {
        private IEnumerable<string> _usings = new List<string>()
        {
            "using System;",
            "using System.IO;"
        };

        private string _insertCode = "\n\n" + @"namespace ReplaceNamespace
{
    public static class CustomConsole
    {
        public static TextReader In { get; set; }
        public static TextWriter Out { get; set; }

        public static string ReadLine() => In?.ReadLine();
        public static void WriteLine(object value) => Out?.WriteLine(value);
        public static void Write(string value) => Out?.Write(value);
    }
}";

        public string ReplaceSourceCode(string sourceCode)
        {
            var replacedSourceCode = "";

            foreach(var usingItem in _usings)
            {
                if(!sourceCode.Contains(usingItem))
                {
                    replacedSourceCode += usingItem + "\n";
                }
            }

            replacedSourceCode += sourceCode.Replace("Console.", "ReplaceNamespace.CustomConsole.");
            replacedSourceCode += _insertCode;

            return replacedSourceCode;
        }

        public void ReplaceIO(Assembly assembly, StringReader stringReader, StringWriter stringWriter)
        {
            var customConsoleType = assembly.GetType("ReplaceNamespace.CustomConsole");
            if(customConsoleType != null)
            {
                customConsoleType.GetProperty("In")?.SetValue(null, stringReader);
                customConsoleType.GetProperty("Out")?.SetValue(null, stringWriter);
            }
        }
    }
}
