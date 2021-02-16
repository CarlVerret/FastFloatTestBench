
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;



using FastFloatTestBench.Mappings;
using FastFloatTestBench.Converters;


using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;


 namespace FastFloatTestBench.Mappings
{

// 	internal class simpleCsvCityMapping : CsvMapping<WorldCity>
// {
//     public simpleCsvCityMapping()
//         : base()
//     {
//        // MapProperty(0, x => x.City);
//         MapProperty(5, x => x.Latitude, new FFDoubleConverter());
//         MapProperty(6, x => x.Longitude, new FFDoubleConverter());
//     }



// }

internal class WorldCity
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



// internal sealed class CustomCsvCityMapping : CsvMapping<WorldCity>
// {
//     public CustomCsvCityMapping()
//         : base()
//     {

//        MapUsing((entity,values)=> {
// 					entity.Latitude = Double.Parse(values.Tokens[5]);
// 					entity.Longitude = Double.Parse(values.Tokens[6]);
// 					return true;
// 		       });
//     }


	
// }
// 	internal sealed class CustomFFCsvCityMapping : CsvMapping<WorldCity>
// {
//     public CustomFFCsvCityMapping()
//         : base()
//     {

//        MapUsing((entity,values)=> {
// 					entity.Latitude =  csFastFloat.FastDoubleParser.ParseDouble(values.Tokens[5]);
// 					entity.Longitude = csFastFloat.FastDoubleParser.ParseDouble(values.Tokens[6]);
// 					return true;
// 		       });
//     }


	
// }

// 	internal sealed class CustomZeroCsvCityMapping : CsvMapping<WorldCity>
// {
//     public CustomZeroCsvCityMapping()
//         : base()
//     {

//        MapUsing((entity,values)=> {
// 					entity.Latitude =  0;
// 					entity.Longitude = 0;
// 					return true;
// 		       });
//     }


	
// }



}