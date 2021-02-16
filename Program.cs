using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

using TinyCsvParser;
using TinyCsvParser.Mapping;
using TinyCsvParser.TypeConverter;


using FastFloatTestBench.Mappings;
using FastFloatTestBench.Converters;


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
//	[SimpleJob( warmupCount: 100, targetCount: 100)]
	[Config(typeof(Config))]
	public class Program
  {
		private class Config : ManualConfig
		{
			public Config()
			{

			

				AddColumn(
						StatisticColumn.Min);
				// Todo : add MB/s + MFloat/s stats columns
			}
		}


		static void Main(string[] args)
		{



			var summary = BenchmarkRunner.Run<Program>();

	
		}

		public List<string> _filenames => new List<string> {  @"w-c-100K.csv", @"w-c-300K.csv"  };

	
		
	   [ParamsSource(nameof(_filenames))]
        public string FileName { get; set; }



		[Benchmark()]
		public double Tiny_Regular(){

			CsvParserOptions csvParserOptions = new CsvParserOptions(true, ',',1,true);
            CsvCityMapping csvMapper = new CsvCityMapping();
            CsvParser<WorldCity> csvParser = new CsvParser<WorldCity>(csvParserOptions, csvMapper);


 			var result = csvParser
                .ReadFromFile(FileName, Encoding.ASCII).AsEnumerable().Max(i => i.Result.Longitude);
	 		//.ToList();

				return result;


		}

	[Benchmark ()]
		public double Tiny_Zeroes(){

			CsvParserOptions csvParserOptions = new CsvParserOptions(true, ',', 1, true);
            CustomZeroCsvCityMapping csvMapper = new CustomZeroCsvCityMapping();
            CsvParser<WorldCity> csvParser = new CsvParser<WorldCity>(csvParserOptions, csvMapper);



 			var result = csvParser
                .ReadFromFile(FileName, Encoding.ASCII).AsEnumerable().Max(i => i.Result.Longitude);
                //.ToList();

					return result;

		}

	[Benchmark]
		public double Tiny_CustomMap(){

			CsvParserOptions csvParserOptions = new CsvParserOptions(true, ',', 1, true);
            CustomCsvCityMapping csvMapper = new CustomCsvCityMapping();
            CsvParser<WorldCity> csvParser = new CsvParser<WorldCity>(csvParserOptions, csvMapper);



 			var result = csvParser
                .ReadFromFile(FileName, Encoding.ASCII).AsEnumerable().Max(i => i.Result.Longitude);
                //.ToList();

					return result;

		}




	[Benchmark]
		public double Tiny_FF(){

			CsvParserOptions csvParserOptions = new CsvParserOptions(true, ',', 1, true);
            CustomFFCsvCityMapping csvMapper = new CustomFFCsvCityMapping();
            CsvParser<WorldCity> csvParser = new CsvParser<WorldCity>(csvParserOptions, csvMapper);



 			var result = csvParser
                .ReadFromFile(FileName, Encoding.ASCII).AsEnumerable().Max(i => i.Result.Longitude);
                //.ToList();

					return result;

		}


		[Benchmark ( Baseline=true)]
		public   int CsvH_Regular()
		{
			using (var reader = new StreamReader(FileName))
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

	[Benchmark]
		public  int CsvH_Zeroes()
		{


			var parser = new ZeroDoubleConverter();

			using (var reader = new StreamReader(FileName))
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

		[Benchmark]
		public  int CsvH_FF()
		{


			var parser = new csFFDoubleConverter();

			using (var reader = new StreamReader(FileName))
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



	


		
	}
}
