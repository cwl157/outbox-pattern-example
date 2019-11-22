using System;
using System.Collections.Generic;
using System.Threading;

namespace OutboxPatternExample.OutboxProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputKey = "";
            List<OutboxRecord> records = new List<OutboxRecord>();

            while (inputKey != "e")
            {
                Console.WriteLine("Menu. Press 'e' to exit.");
                Console.WriteLine("1. Process Outbox");
                Console.Write("Input: ");
                inputKey = Console.ReadLine();

                if (inputKey == "1")
                {
                    Console.WriteLine("Process Outbox");
                    records = DataAccess.LoadQueuedOutbox();

                    if (records.Count == 0)
                    {
                        Console.WriteLine("Nothing to process in outbox");
                    }
                    else
                    {

                        // In this case "Process" means print message to console
                        // Once that is done, records can be marked as processed
                        foreach (OutboxRecord r in records)
                        {
                            Console.WriteLine("Processing " + r.RequestMessage);
                            r.ProcessedDate = DateTime.Now;
                            r.ProcessedStatus = "processed";
                            Thread.Sleep(200);
                        }

                        DataAccess.UpdateOutbox(records);
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input");
                }
            }

            Console.WriteLine("Hello World!");
        }
    }
}
