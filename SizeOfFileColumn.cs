using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

using FastFloatTestBench.Mappings;
using FastFloatTestBench.Converters;


using BenchmarkDotNet.Reports;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

using System.Runtime.CompilerServices;


namespace FastFloatTestBench
{

    public class SizeOfFileColumn : IColumn
    {
        public string Id => nameof(SizeOfFileColumn);

        public string ColumnName => "SizeOfFile(MB)";

        public string Legend => "Size of file being parsed";

        public UnitType UnitType => UnitType.Size;

        public bool AlwaysShow => true;

        public ColumnCategory Category => ColumnCategory.Metric;

        public int PriorityInCategory => 0;

        public bool IsNumeric => true;

        public bool IsAvailable(Summary summary) => true;

        public bool IsDefault(Summary summary, BenchmarkCase benchmarkCase) => false;

        public string GetValue(Summary summary, BenchmarkCase benchmarkCase) => GetValue(summary, benchmarkCase, SummaryStyle.Default);

        public string GetValue(Summary summary, BenchmarkCase benchmarkCase, SummaryStyle style)
        {
            var benchmarkName = benchmarkCase.Descriptor.WorkloadMethod.Name.ToLower();
            var myFileName =  benchmarkCase.Parameters.Items.FirstOrDefault(x => x.Name == "fileName").ToString();
            if (myFileName == null)
            {
                return "no parameter";
            }
			Console.WriteLine($"here !! name : {myFileName}");

            // var N = Convert.ToInt32(parameter.Value);
            // var filename = $"disk-size.{benchmarkName}.{N}.txt";
            return File.Exists(myFileName) ? (File.ReadAllText(myFileName).Length/1024).ToString() : "no file";
        }

        public override string ToString() => ColumnName;
    }

}