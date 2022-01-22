
using System.Reflection;
var assembly = Assembly.LoadFrom(@"C:\Users\USER\source\repos\PoorMansNet6\PoorMansNet6Simple\bin\Release\net6.0\PoorMansNet6Easy.dll");
dynamic ff = assembly.CreateInstance("ExtraFormatter.Formatter");

Console.WriteLine(ff.DoFormat(File.ReadAllText(@"C:\Users\dusko\sqls\to_format.sql")));

