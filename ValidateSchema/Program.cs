using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;

namespace ValidateSchema
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: ValidataSchema <XML_SCHEMA_FILE_PATH>");
                return 127;
            }

            var results = new List<ValidationEventArgs>();

            var schemaSet = new XmlSchemaSet();

            schemaSet.ValidationEventHandler += (sender, a) => results.Add(a);

            foreach (var path in args)
            {
                schemaSet.Add(null, path);
            }

            schemaSet.Compile();

            foreach (var result in results.OrderBy(r => r.Exception.SourceUri).ThenBy(r => r.Exception.LineNumber).ThenBy(r => r.Exception.LinePosition))
            {
                Console.Error.WriteLine(
                    "{0} ({1}:{2}): {3}: {4}",
                    result.Exception.SourceUri,
                    result.Exception.LineNumber,
                    result.Exception.LinePosition,
                    result.Severity,
                    result.Message
                );
            }

            return results.Count == 0 ? 0 : 1;
        }
    }
}
