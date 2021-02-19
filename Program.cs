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
				AddColumn(StatisticColumn.Min);
				AddColumn(new MFloatPerSecColumn());
			}
		}


		static void Main(string[] args)
		{
			var config = DefaultConfig.Instance.WithSummaryStyle( SummaryStyle.Default.WithMaxParameterColumnWidth(100));
			var summary = BenchmarkRunner.Run<Program>(config);
		}


		[Benchmark ( Baseline=true)]
	[ArgumentsSource(nameof(MultiColFiles))]
		public   int CsvH_Regular(string fileName,  int fileSize, int nbFloat)
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
		public  int CsvH_Zeroes(string fileName,  int fileSize, int nbFloat)
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
		public  int CsvH_FF(string fileName,  int fileSize, int nbFloat)
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

	internal object[] TestFileSpecs(string fileName, int floatPerLine)
	{
		int volume =0;
		 int nbFloat =0;

		var lines =	 System.IO.File.ReadAllLines(fileName);
		 foreach (string l in lines)
			{
				volume += l.Length;
			}
		nbFloat = lines.Count() * floatPerLine;

		return new object[] { fileName, volume/1024,nbFloat  };
	}


	[Benchmark ( Baseline=true)]
		 [ArgumentsSource(nameof(SingleColFiles))]
		public   int SingleCol_Regular(string fileName,  int fileSize, int nbFloat)
		{
			using (var reader = new StreamReader(fileName))
			using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
			{
				nbFloat = 0;
				var records = new List<double>();
				csv.Read();
				csv.ReadHeader();
				while (csv.Read())
				{
					nbFloat ++;
					records.Add(csv.GetField<double>(0));
				}

				return records.Count;

			}
		}

		[Benchmark ()]
		 [ArgumentsSource(nameof(SingleColFiles))]
		public  int SingleCol_Zeroes(string fileName,  int fileSize, int nbFloat )
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



		[Benchmark ()]
		 [ArgumentsSource(nameof(SingleColFiles))]
		public  int SingleCol_FF(string fileName,  int fileSize, int nbFloat)
		{

			var parser = new csFFDoubleConverter();

			using (var reader = new StreamReader(fileName))
			using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
			{
				nbFloat = 0;
				var records = new List<double>();
					csv.Read();
				csv.ReadHeader();
				while (csv.Read())
				{
				nbFloat ++;
					records.Add(csv.GetField<double>(0,parser));
				}
				
				return records.Count;

			}
		}


 		public IEnumerable<object[]> MultiColFiles() // for single argument it's an IEnumerable of objects (object)
        {

			 yield return TestFileSpecs( @"TestData/w-c-100K.csv",2) ;
			 yield return TestFileSpecs( @"TestData/w-c-300K.csv",2) ;
        }

		 public IEnumerable<object[]> SingleColFiles() // for multiple arguments it's an IEnumerable of array of objects (object[])
        {
            yield return TestFileSpecs( @"TestData/canada.txt",1) ;
			yield return TestFileSpecs( @"TestData/mesh.txt",1) ;
			yield return TestFileSpecs( @"TestData/synthetic.csv",1) ;
        }

		
	}
}
