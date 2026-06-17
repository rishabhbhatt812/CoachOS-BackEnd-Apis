using System;
using System.Reflection;
using System.Linq;
using System.IO;

class Program
{
    static void Main()
    {
        var path = @"C:\Users\risha\.nuget\packages\swashbuckle.aspnetcore.swaggergen\10.2.1\lib\net10.0\Swashbuckle.AspNetCore.SwaggerGen.dll";
        if (File.Exists(path))
        {
            var asm = Assembly.LoadFrom(path);
            foreach(var t in asm.GetTypes().Where(x => x.Name.Contains("OpenApi")))
                Console.WriteLine(t.FullName);
        }
        
        var openapiPath = @"C:\Users\risha\.nuget\packages\microsoft.openapi\3.7.0\lib\netstandard2.0\Microsoft.OpenApi.dll";
        if (File.Exists(openapiPath))
        {
            var asm = Assembly.LoadFrom(openapiPath);
            foreach(var t in asm.GetExportedTypes().Where(x => x.Name.Contains("SecurityScheme")))
                Console.WriteLine(t.FullName);
        }
    }
}
