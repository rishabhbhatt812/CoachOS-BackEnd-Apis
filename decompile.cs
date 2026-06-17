using System;
using System.Reflection;
using System.Linq;

class Program
{
    static void Main()
    {
        var openapiPath = @"C:\Users\risha\.nuget\packages\microsoft.openapi\2.7.5\lib\net8.0\Microsoft.OpenApi.dll";
        var asm = Assembly.LoadFrom(openapiPath);
        var type = asm.GetExportedTypes().FirstOrDefault(x => x.Name == "OpenApiSecurityScheme");
        if (type != null)
        {
            Console.WriteLine("Properties of OpenApiSecurityScheme:");
            foreach(var prop in type.GetProperties())
            {
                Console.WriteLine($"  {prop.PropertyType.Name} {prop.Name}");
            }
        }
        else
        {
            Console.WriteLine("OpenApiSecurityScheme not found!");
            foreach(var t in asm.GetExportedTypes().Where(x => x.Name.Contains("Security")))
            {
                Console.WriteLine("Found: " + t.FullName);
            }
        }
    }
}
