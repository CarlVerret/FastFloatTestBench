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


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;


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

		[Benchmark ()]
		public   int ParseRegularCsvH()
		{
			using (var reader = new StreamReader(@"worldcitiespop-100K.csv"))
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

		private class CsvCityMapping : CsvMapping<WorldCity>
{
    public CsvCityMapping()
        : base()
    {
       // MapProperty(0, x => x.City);
        MapProperty(1, x => x.Latitude);
        MapProperty(2, x => x.Longitude);
    }



}
	private class CustomCsvCityMapping : CsvMapping<WorldCity>
{
    public CustomCsvCityMapping()
        : base()
    {

		var myConverter = new 	FFDoubleConverter();

      //  MapProperty(0, x => x.City);
        MapProperty(1, x => x.Latitude, myConverter );
        MapProperty(2, x => x.Longitude, myConverter);
    }


	
}



		[Benchmark(Baseline =true)]
		public int ParseRegularTiny(){

			CsvParserOptions csvParserOptions = new CsvParserOptions(true, ';');
            CsvCityMapping csvMapper = new CsvCityMapping();
            CsvParser<WorldCity> csvParser = new CsvParser<WorldCity>(csvParserOptions, csvMapper);



 			var result = csvParser
                .ReadFromFile(@"worldcitiespop-100K.csv", Encoding.ASCII)
                .ToList();

				return result.Count();

		}

	//	[Benchmark]
		public int ParseOverrideTiny(){

			CsvParserOptions csvParserOptions = new CsvParserOptions(true, ';');
            CustomCsvCityMapping csvMapper = new CustomCsvCityMapping();
            CsvParser<WorldCity> csvParser = new CsvParser<WorldCity>(csvParserOptions, csvMapper);



 			var result = csvParser
                .ReadFromFile(@"worldcitiespop-100K.csv", Encoding.ASCII)
                .ToList();

				return result.Count();

		}







		//[Benchmark]
		public  int ParseOverrideCsvH()
		{


			var parser = new csFFDoubleConverter();

			using (var reader = new StreamReader(@"worldcitiespop-100K.csv"))
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



	  public class FFDoubleConverter : NonNullableConverter<Double>
    {
      private readonly IFormatProvider formatProvider;
      private readonly NumberStyles numberStyles;

      public FFDoubleConverter()
          : this(CultureInfo.InvariantCulture)
      {
      }

      public FFDoubleConverter(IFormatProvider formatProvider)
          : this(formatProvider, NumberStyles.Float | NumberStyles.AllowThousands)
      {
      }

      public FFDoubleConverter(IFormatProvider formatProvider, NumberStyles numberStyles)
      {
        this.formatProvider = formatProvider;
        this.numberStyles = numberStyles;
      }

      protected override bool InternalConvert(string value, out Double result)
      {
          result = csFastFloat.FastDoubleParser.ParseDouble(value);
        return true;

      }
    }



		public  class csFFDoubleConverter : DefaultTypeConverter
		{
			private Lazy<string> defaultFormat = new Lazy<string>(() => double.TryParse(double.MaxValue.ToString("R"), out var _) ? "R" : "G17");

			/// <summary>
			/// Converts the object to a string.
			/// </summary>
			/// <param name="value">The object to convert to a string.</param>
			/// <param name="row">The <see cref="IWriterRow"/> for the current record.</param>
			/// <param name="memberMapData">The <see cref="MemberMapData"/> for the member being written.</param>
			/// <returns>The string representation of the object.</returns>
			public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
			{
				var format = memberMapData.TypeConverterOptions.Formats?.FirstOrDefault() ?? defaultFormat.Value;

				if (value is double d)
				{
					return d.ToString(format, memberMapData.TypeConverterOptions.CultureInfo);
				}

				return base.ConvertToString(value, row, memberMapData);
			}

			/// <summary>
			/// Converts the string to an object.
			/// </summary>
			/// <param name="text">The string to convert to an object.</param>
			/// <param name="row">The <see cref="IReaderRow"/> for the current record.</param>
			/// <param name="memberMapData">The <see cref="MemberMapData"/> for the member being created.</param>
			/// <returns>The object created from the string.</returns>
			public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
			=> csFastFloat.FastDoubleParser.ParseDouble(text);
		}


		public class WorldCity
		{


//			Country,City,AccentCity,Region,Population,Latitude,Longitude
//ad, aixas, Aixàs,06,,42.4833333,1.4666667
			public string Country { get; set; }
			public string City { get; set; }
			public string AccentCity { get; set; }
			public string Region { get; set; }
			public string Population { get; set; }
			public double Latitude { get; set; }
			public double Longitude { get; set; }



		}
	}
}
