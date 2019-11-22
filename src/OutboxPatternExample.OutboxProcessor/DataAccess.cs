using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace OutboxPatternExample.OutboxProcessor
{
    public static class DataAccess
    {
        private static string ConnectionString = @"Server=localhost\SQLEXPRESS;Database=OutboxExample_1;Trusted_Connection=True;";

        public static List<OutboxRecord> LoadQueuedOutbox()
        {
            List<OutboxRecord> records = new List<OutboxRecord>();

            using (var connection = new SqlConnection(ConnectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "GetQueuedOutbox";

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            records.Add(
                                 new OutboxRecord()
                                 {
                                     Id = reader.GetInt32(0),
                                     RequestMessage = reader.GetString(1),
                                     QueuedDate = reader.GetDateTime(2),
                                     ProcessedDate = reader.IsDBNull(3) ? new DateTime?() : reader.GetDateTime(3),
                                     ProcessedStatus = reader.GetString(4),
                                     ErrorMessage = reader.IsDBNull(5) ? "" : reader.GetString(5)
                                 });
                        }
                    }
                }
            }

            return records;
        }

        public static void UpdateOutbox(List<OutboxRecord> records)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    command.CommandText = "UpdateOutboxRecord";

                    foreach (OutboxRecord r in records)
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@outboxId", r.Id);
                        command.Parameters.AddWithValue("@ProcessedDate", r.ProcessedDate);
                        command.Parameters.AddWithValue("@ProcessedStatus", r.ProcessedStatus);
                        command.Parameters.AddWithValue("@ErrorMessage", r.ErrorMessage);
                        command.ExecuteNonQuery();   
                    }
                }
            }
        }
    }
}
