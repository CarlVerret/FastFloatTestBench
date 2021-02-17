using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;




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
	


	[SimpleJob(RuntimeMoniker.NetCoreApp50)]
	//[SimpleJob( warmupCount: 1, targetCount: 1)]
	[Config(typeof(Config))]
	public class Program
  {
		private class Config : ManualConfig
		{
			public Config()
			{
				AddColumn(
						StatisticColumn.Min);

				AddColumn(new SizeOfFileColumn());

				

				
				// Todo : add MB/s + MFloat/s stats columns
			}
		}


		static void Main(string[] args)
		{
			var config = DefaultConfig.Instance.With(SummaryStyle.Default.WithMaxParameterColumnWidth(100));
			var summary = BenchmarkRunner.Run<Program>(config);
		}


		[Benchmark ( Baseline=true)]
	[ArgumentsSource(nameof(MultiColFiles))]
		public   int CsvH_Regular(string fileName)
		{
			using (var reader = new StreamReader(fileName))
			using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
			{
				var records = new List<WorldCity>();
				csv.Read();
				csv.ReadHeader();
				while (csv.Read())
				{
					var record = new WorldCity
					{
					//	City = csv.GetField<string>("City"),
						Latitude = csv.GetField<double>("Latitude"),
						Longitude = csv.GetField<double>("Longitude")
					};
					records.Add(record);
				}

				return records.Count;

			}
		}

		[Benchmark ()]
	[ArgumentsSource(nameof(MultiColFiles))]
		public  int CsvH_Zeroes(string fileName)
		{

			var parser = new ZeroDoubleConverter();

			using (var reader = new StreamReader(fileName))
			using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
			{
				var records = new List<WorldCity>();
				csv.Read();
				csv.ReadHeader();
				while (csv.Read())
				{
					var record = new WorldCity
					{
					//	City = csv.GetField<string>("City"),
						Latitude =  csv.GetField<double>("Latitude", parser),
						Longitude = csv.GetField<double>("Longitude", parser)
					};
					records.Add(record);
				}

				return records.Count;

			}
		}

		[Benchmark ()]
	 [ArgumentsSource(nameof(MultiColFiles))]
		public  int CsvH_FF(string fileName)
		{


			var parser = new csFFDoubleConverter();

			using (var reader = new StreamReader(fileName))
			using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
			{
				var records = new List<WorldCity>();
				csv.Read();
				csv.ReadHeader();
				while (csv.Read())
				{
					var record = new WorldCity
					{
					//	City = csv.GetField<string>("City"),
						Latitude =  csv.GetField<double>("Latitude", parser),
						Longitude = csv.GetField<double>("Longitude", parser)
					};
					records.Add(record);
				}

				return records.Count;

			}
		}



	[Benchmark ( Baseline=true)]
		 [ArgumentsSource(nameof(SingleColFiles))]
		public   int SingleCol_Regular(string fileName)
		{
			using (var reader = new StreamReader(fileName))
			using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
			{
				var records = new List<double>();
				csv.Read();
				csv.ReadHeader();
				while (csv.Read())
				{
					records.Add(csv.GetField<double>(0));
				}

				return records.Count;

			}
		}

		[Benchmark ()]
		 [ArgumentsSource(nameof(SingleColFiles))]
		public  int SingleCol_Zeroes(string fileName)
		{

			var parser = new ZeroDoubleConverter();

			using (var reader = new StreamReader(fileName))
			using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
			{
				var records = new List<double>();
				csv.Read();
				csv.ReadHeader();
				while (csv.Read())
				{
					records.Add(csv.GetField<double>(0, parser));
				}

				return records.Count;
			}
		}

 		public IEnumerable<string> MultiColFiles() // for single argument it's an IEnumerable of objects (object)
        {
            yield return	@"TestData/w-c-100K.csv" ;
            yield return    @"TestData/w-c-300K.csv";
        }
  		 public IEnumerable<string> SingleColFiles() // for single argument it's an IEnumerable of objects (object)
        {
            yield return	@"TestData/canada.txt" ;
            yield return    @"TestData/synthetic.csv";
        }

		[Benchmark ()]
		 [ArgumentsSource(nameof(SingleColFiles))]
		public  int SingleCol_FF(string fileName)
		{

			var parser = new csFFDoubleConverter();

			using (var reader = new StreamReader(fileName))
			using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
			{
				var records = new List<double>();
					csv.Read();
				csv.ReadHeader();
				while (csv.Read())
				{
					records.Add(csv.GetField<double>(0,parser));
				}
				
				return records.Count;

			}
		}



		
	}
}
