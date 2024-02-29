// See https://aka.ms/new-console-template for more information
using VIOVNL.CloudLogger;

CloudLogger cloudLogger = CloudLogger.Create("MmM3NDg1NDgtYjA0MS00Y2JiLTg2YTItNjFjZ");

cloudLogger.LogAsync([
    new("Date", "22-10-1994"),
    new("Country", "Netherlands")
], true).Wait();

Console.WriteLine("Hello, World!");
