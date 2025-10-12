using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Book_Tests;

internal static class LogginProvider
{
    public static LoggingObj[] GetLoggingArr()
    {
        string secrets = File.ReadAllText(@"..\..\..\..\secrets.json");
        var LoggingArr = JsonDocument.Parse(secrets).RootElement.GetProperty("LoggingArr");
        return LoggingArr.Deserialize<LoggingObj[]>()!;
    }
}
