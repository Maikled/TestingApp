using TestingApp.Core.Models.Tests.Interfaces;

namespace TestingApp.Core.Models.Tests.DataProviders
{
    public class TextDataProvider : ITestDataProvider
    {
        public TestData GetTestData()
        {
            return new TestData(GetSourceCode(), GetInputData(), GetOutputData());
        }

        private string GetInputData()
        {
            return "2\r\nbanana\r\n4\r\nbanana\r\n2";
        }

        private string GetOutputData()
        {
            return "2\r\naabn\r\n-1\r\n";
        }

        private string GetSourceCode()
        {
            return "using System;\r\nusing System.Collections.Generic;\r\nusing System.Linq;\r\n\r\n\r\nnamespace ConsoleApp1\r\n{\r\n    class Program\r\n    {\r\n        static void Main()\r\n        {\r\n            var sets = int.Parse(Console.ReadLine());\r\n            for (int i = 0; i < sets; i++)\r\n            {\r\n                var s = Console.ReadLine();\r\n                var n = int.Parse(Console.ReadLine());\r\n\r\n                var groups = s.GroupBy(p => p).OrderByDescending(p => p.Key);\r\n\r\n                if(n < groups.Count())\r\n                {\r\n                    Console.WriteLine(\"-1\");\r\n                    continue;\r\n                }\r\n\r\n                Dictionary<char, int> counter = new Dictionary<char, int>();\r\n                foreach(var item in groups)\r\n                {\r\n                    counter.Add(item.Key, s.Count(p => p == item.Key));\r\n                }\r\n\r\n                for (int j = 1; j <= s.Length; j++)\r\n                {\r\n                    var count = 0;\r\n                    foreach (var item in counter)\r\n                    {\r\n                        count += item.Value / j;\r\n                        if(item.Value % j != 0)\r\n                        {\r\n                            count++;\r\n                        }\r\n                    }\r\n\r\n                    if(count <= n)\r\n                    {\r\n                        int c = 0;\r\n                        string result = \"\";\r\n                        foreach(var item in counter.Reverse())\r\n                        {\r\n                            c = item.Value / j;\r\n                            if(item.Value % j != 0)\r\n                            {\r\n                                c++;\r\n                            }\r\n\r\n                            while (c-- != 0)\r\n                            {\r\n                                result += item.Key;\r\n                            }\r\n                        }\r\n\r\n                        c = n - result.Length;\r\n                        while(c-- != 0)\r\n                        {\r\n                            result += 'a';\r\n                        }\r\n\r\n                        Console.WriteLine(j);\r\n                        Console.WriteLine(string.Join(\"\", result.OrderBy(p => p)));\r\n\r\n                        break;\r\n                    }\r\n                }\r\n            }\r\n        }\r\n    }\r\n}";
        }
    }
}
