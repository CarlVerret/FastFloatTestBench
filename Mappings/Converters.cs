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


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

using System.Runtime.CompilerServices;


 namespace FastFloatTestBench.Converters
 {

//   public sealed class FFDoubleConverter : NonNullableConverter<double>
//     {
//       private readonly IFormatProvider formatProvider;
//       private readonly NumberStyles numberStyles;

//       public FFDoubleConverter()
//           : this(CultureInfo.InvariantCulture)
//       {
//       }

//       public FFDoubleConverter(IFormatProvider formatProvider)
//           : this(formatProvider, NumberStyles.Float | NumberStyles.AllowThousands)
//       {
//       }

//       public FFDoubleConverter(IFormatProvider formatProvider, NumberStyles numberStyles)
//       {
//         this.formatProvider = formatProvider;
//         this.numberStyles = numberStyles;
//       }
      
//       protected override bool InternalConvert(string value, out Double result)
//       {
//           result = csFastFloat.FastDoubleParser.ParseDouble(value);
//         return true;

//       }
//     }



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
		{ 
			if(csFastFloat.FastDoubleParser.TryParseDouble(text, out double d))
      {
			return d;
      }
			throw new Exception($"cannot read double {text}");
		}
		}
		
		public  class ZeroDoubleConverter : DefaultTypeConverter
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
			=> 0d;
		}

 }