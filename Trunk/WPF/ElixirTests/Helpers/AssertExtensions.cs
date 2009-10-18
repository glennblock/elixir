using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if !SILVERLIGHT
using AssertionException = NUnit.Framework.AssertionException;
#else
using AssertionException = Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException;
#endif

namespace ElixirTests
{
    public static class AssertExt
    {
        public static void ExpectException<TExc>(Action action) where TExc : Exception
        {
            ExpectException<TExc>(action, string.Empty);
        }

        public static void ExpectException<TExc>(Action action, string message) where TExc : Exception
        {
            try
            {
                action();
                throw new AssertionException(string.Format("Expected exception of type '{0}', but no exception was caught.", typeof(TExc).Name));
            }
            catch (TExc)
            {
            }
            catch (Exception exc)
            {
                throw new AssertionException(string.Format("Expected exception of type '{0}', but an exception of type '{1}' was caught.", typeof(TExc).Name, exc.GetType().Name));
            }
        }
    }
}
