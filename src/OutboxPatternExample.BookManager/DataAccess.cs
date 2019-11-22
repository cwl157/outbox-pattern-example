using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace OutboxPatternExample.BookManager
{
    public static class DataAccess
    {
        private static string ConnectionString = @"Server=localhost\SQLEXPRESS;Database=OutboxExample_1;Trusted_Connection=True;";

        public static List<Book> LoadBooks()
        {
            List<Book> result = new List<Book>();

            using (var connection = new SqlConnection(ConnectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "GetBooks";

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(
                                 new Book()
                                 {
                                     Title = reader.GetString(0),
                                     Author = reader.GetString(1)
                                 });
                        }
                    }
                }
            }

            return result;
        }

        public static void SaveBook(Book book)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    command.Transaction = command.Connection.BeginTransaction();
                    command.CommandText = "SaveBook";
                    command.Parameters.AddWithValue("@Title", book.Title);
                    command.Parameters.AddWithValue("@Author", book.Author);
                    command.ExecuteNonQuery();

                    command.CommandText = "InsertOutbox";
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@RequestMessage", book.Title + "," + book.Author);
                    command.ExecuteNonQuery();

                    command.Transaction.Commit();
                }
            }
        }
    }
}
