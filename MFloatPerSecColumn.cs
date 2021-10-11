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

    public class MFloatPerSecColumn : IColumn
    {
        public string Id => nameof(MFloatPerSecColumn);

        public string ColumnName => "MFloat/s";

        public string Legend => "Number of MFloat per second";

        public UnitType UnitType => UnitType.Size;

        public bool AlwaysShow => true;

        public ColumnCategory Category => ColumnCategory.Metric;

        public int PriorityInCategory => 1;

        public bool IsNumeric => true;

        public bool IsAvailable(Summary summary) => true;

        public bool IsDefault(Summary summary, BenchmarkCase benchmarkCase) => false;

        public string GetValue(Summary summary, BenchmarkCase benchmarkCase) => GetValue(summary, benchmarkCase, SummaryStyle.Default);

    public string GetValue(Summary summary, BenchmarkCase benchmarkCase, SummaryStyle style)
    {
      var disp_info = benchmarkCase.DisplayInfo;
      var nbFloat = (int)benchmarkCase.Parameters.Items[2].Value;

      var qry = summary.Reports.Where(x => x.BenchmarkCase.DisplayInfo == disp_info);
      if (qry.Any())
      {
        var s = qry.FirstOrDefault();
        if (s.ResultStatistics != null)
        { 
          double fps = nbFloat * 1000 / s.ResultStatistics.Min;
          return string.Format("{0,8:f2}",fps);
        }
      }
      return "n/a";


        }

        public override string ToString() => ColumnName;
    }

}