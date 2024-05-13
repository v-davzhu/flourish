using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFlourish.MockData
{
    internal class OperatorSchema
    {
        public static readonly string Data = @"
            {
                "":"": {
                    ""description"": ""contains prefix"",
                    ""isDefault"": true,
                },
                ""="": {
                    ""description"": ""equals"",
                },
                ""<>"": {
                    ""description"": ""not equals"",
                },
                "">"": {
                    ""description"": ""greater than"",
                },
                "">="": {
                    ""description"": ""greater than equals"",
                },
                ""<"": {
                    ""description"": ""less than"",
                },
                ""<="": {
                    ""description"": ""less than equals"",
                },
            }";
    }
}
