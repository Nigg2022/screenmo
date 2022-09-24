using System;
using System.Data.SQLite;

namespace StaffMoniteringSystem
{
    internal class SQLiteConnection
    {
        private string v;

        public SQLiteConnection(string v)
        {
            this.v = v;
        }

        public SQLiteConnection()
        {
        }

        internal void Open()
        {
            throw new NotImplementedException();
        }

        internal void Close()
        {
            throw new NotImplementedException();
        }

        public static implicit operator System.Data.SQLite.SQLiteConnection(SQLiteConnection v)
        {
            throw new NotImplementedException();
        }

        internal void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}