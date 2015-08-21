using System;
using System.Collections.Generic;
using System.Linq;

namespace EvilDuck.Framework.Core
{
    public class ResultCollection
    {
        private readonly IList<Result> _results = new List<Result>();
        public IEnumerable<Result> Results
        {
            get { return _results; }
        }

        public void Add(Result result)
        {
            _results.Add(result);
        }

        public bool IsSuccess
        {
            get { return _results.All(e => e.IsSuccess); }
        }

        public Failure FirstFailure
        {
            get { return _results.OfType<Failure>().FirstOrDefault(r => !r.IsSuccess); }
        }

        public IEnumerable<Failure> Failures
        {
            get { return _results.OfType<Failure>(); }
        }

        public Result MergeFailures()
        {
            return Result.Failure(String.Join(Environment.NewLine, Failures));
        }
    }

    public abstract class Result
    {
        public bool IsSuccess
        {
            get { return this is Success; }
        }

        public string Message { get; protected set; }

        public static Success Success(string message)
        {
            return new Success(message);
        }

        public static Failure Failure(string message, Exception exception = null)
        {
            return new Failure(message, exception);
        }

        public abstract void ThrowIfFailure();
    }

    public class Success : Result
    {
        public Success(string message)
        {
            Message = message;
        }

        public override void ThrowIfFailure()
        {
            
        }
    }

    public class Failure : Result
    {
        public Exception Exception { get; set; }

        public Failure(string message, Exception exception)
        {
            Message = message;
            Exception = exception;
        }

        public override void ThrowIfFailure()
        {
            if (Exception == null)
            {
                throw new Exception(String.Empty, Exception);
            }
            throw new Exception(Message);
        }
    }
}
