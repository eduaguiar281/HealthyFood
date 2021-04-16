using System;

namespace HowToDevelop.Core.Infraestrutura.Helpers
{
    public class QueryStatementException : Exception
    {
        public QueryStatementException()
            : base()
        {

        }
        public QueryStatementException(string message)
            : base(message)
        {

        }
        public QueryStatementException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
