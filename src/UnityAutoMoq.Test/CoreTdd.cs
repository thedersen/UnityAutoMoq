
    /* Usage: just add/link this file to your NUnit test project.
 * Class and method attributes are:
 * 
 * [TestClass]
 * public class MyFixture
 * {
 *   [TestMethod]
 *   public void ShouldWork()
 *   {
 *     // arrange
 *     // act
 *     // assert
 *     result.ShouldNotBeNull();
 *   }
 * }
 * 
 * "Dotting" on the results you want to verify, 
 * you'll get the ShouldXXX assertions to use.
 * Don't add a "using NUnit.Framework;" import 
 * at the top of the file. It's not needed and 
 * will limit portability of your tests in the 
 * future.
 * 
 * To port the tests to a different test framework, simply replace 
 * the corresponding CoreTDD file.
 * 
 * Report issues and suggestions to http://code.google.com/p/coretdd.
 * 
 * Daniel Cazzulino - http://clariusconsulting.net/kzu
 */

    #region New BSD License
    //Copyright (c) 2007, CoreTDD Team 
    //http://code.google.com/p/coretdd/
    //All rights reserved.

    //Redistribution and use in source and binary forms, 
    //with or without modification, are permitted provided 
    //that the following conditions are met:

    //    * Redistributions of source code must retain the 
    //    above copyright notice, this list of conditions and 
    //    the following disclaimer.

    //    * Redistributions in binary form must reproduce 
    //    the above copyright notice, this list of conditions 
    //    and the following disclaimer in the documentation 
    //    and/or other materials provided with the distribution.

    //    * Neither the name of the CoreTDD Team nor the 
    //    names of its contributors may be used to endorse 
    //    or promote products derived from this software 
    //    without specific prior written permission.

    //THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND 
    //CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, 
    //INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF 
    //MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE 
    //DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR 
    //CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
    //SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, 
    //BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR 
    //SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
    //INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
    //WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING 
    //NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE 
    //OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF 
    //SUCH DAMAGE.

    //[This is the BSD license, see
    // http://www.opensource.org/licenses/bsd-license.php]
    #endregion

    using System;
    using System.Linq;
    using System.Collections;
    using NUnit.Framework;

    /// <summary>
    /// Marks a class that contains tests.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TestClassAttribute : TestFixtureAttribute { }

    /// <summary>
    /// Marks a test method.
    /// </summary>
    public class TestMethodAttribute : TestAttribute { }

    public static class TestExtensions
    {
        /// <summary>
        /// Asserts that the given object satisfies the condition.
        /// </summary>
        /// <typeparam name="T">Type of the object, omitted as it can be inferred from the value.</typeparam>
        /// <param name="object">The object to test with the condition.</param>
        /// <param name="condition">The condition expresion.</param>
        /// <example>
        /// <code>
        /// Order order = repository.CreateOrder(...);
        /// 
        /// order.ShouldSatisfy(o =&gt; o.Created >= now);
        /// </code>
        /// </example>
        public static T ShouldSatisfy<T>(this T @object, Predicate<T> condition)
        {
            Assert.IsTrue(condition(@object));

            return @object;
        }

        /// <summary>
        /// Asserts that the given object satisfies the condition.
        /// </summary>
        /// <typeparam name="T">Type of the object, omitted as it can be inferred from the value.</typeparam>
        /// <param name="object">The object to test with the condition.</param>
        /// <param name="condition">The condition expresion.</param>
        /// <param name="failMessage">Message to show if the assertion fails.</param>
        /// <example>
        /// <code>
        /// Order order = repository.CreateOrder(...);
        /// 
        /// order.ShouldSatisfy(o =&gt; o.Created >= now, "Creation date should be equal or greater than now!");
        /// </code>
        /// </example>
        public static T ShouldSatisfy<T>(this T @object, Predicate<T> condition, string failMessage)
        {
            Assert.IsTrue(condition(@object), failMessage);

            return @object;
        }

        /// <summary>
        /// Asserts that the given condition is false.
        /// </summary>
        /// <example>
        /// <code>
        /// bool result = service.Execute();
        /// result.ShouldBeFalse();
        /// </code>
        /// </example>
        public static void ShouldBeFalse(this bool condition)
        {
            Assert.IsFalse(condition);
        }

        /// <summary>
        /// Asserts that the given condition is true.
        /// </summary>
        /// <example>
        /// <code>
        /// bool result = service.Execute();
        /// result.ShouldBeTrue();
        /// </code>
        /// </example>
        public static void ShouldBeTrue(this bool condition)
        {
            Assert.IsTrue(condition);
        }

        /// <summary>
        /// Asserts that the actual object equals the given value.
        /// </summary>
        /// <example>
        /// <code>
        /// var order = repository.CreateOrder(...);
        /// 
        /// order.GrandTotal.ShouldEqual(expectedSum);
        /// </code>
        /// </example>
        public static T ShouldEqual<T>(this T actual, object expected)
        {
            Assert.AreEqual(expected, actual);
            return actual;
        }

        /// <summary>
        /// Asserts that the actual object does not equal the given value.
        /// </summary>
        /// <example>
        /// <code>
        /// var order = repository.CreateOrder(...);
        /// 
        /// order.GrandTotal.ShouldNotEqual(sumItemsWithoutStock);
        /// </code>
        /// </example>
        public static T ShouldNotEqual<T>(this T actual, object expected)
        {
            Assert.AreNotEqual(expected, actual);
            return actual;
        }

        /// <summary>
        /// Asserts that the value is null.
        /// </summary>
        /// <example>
        /// <code>
        /// var order = repository.Find(nonExistentId);
        /// 
        /// order.ShouldBeNull();
        /// </code>
        /// </example>
        public static void ShouldBeNull(this object @object)
        {
            Assert.IsNull(@object);
        }

        /// <summary>
        /// Asserts that the value is not null.
        /// </summary>
        /// <example>
        /// <code>
        /// var order = repository.Find(existentId);
        /// 
        /// order.ShouldNotBeNull();
        /// </code>
        /// </example>
        public static T ShouldNotBeNull<T>(this T @object)
        {
            Assert.IsNotNull(@object);
            return @object;
        }

        /// <summary>
        /// Asserts that the value is the same as the given one.
        /// </summary>
        /// <example>
        /// <code>
        /// var order = repository.CreateOrder(...);
        /// 
        /// order.Customer.ShouldBeSameAs(customer);
        /// </code>
        /// </example>
        public static T ShouldBeSameAs<T>(this T actual, object expected)
        {
            Assert.AreSame(expected, actual);
            return actual;
        }

        /// <summary>
        /// Asserts that the value is not the same as the given one.
        /// </summary>
        /// <example>
        /// <code>
        /// var order = repository.CreateOrder(...);
        /// 
        /// order.Customer.ShouldNotBeSameAs(customer);
        /// </code>
        /// </example>
        public static T ShouldNotBeSameAs<T>(this T actual, object expected)
        {
            Assert.AreNotSame(expected, actual);
            return actual;
        }

        /// <summary>
        /// Asserts that the value is of the given type, inherits or implements it.
        /// </summary>
        /// <example>
        /// <code>
        /// IPrincipal principal = factory.CreateGroupPrincipal("foo");
        /// 
        /// principal.ShouldBeOfType(typeof(GroupPrincipal));
        /// </code>
        /// </example>
        public static object ShouldBeOfType(this object actual, Type expected)
        {
            Assert.IsAssignableFrom(expected, actual);
            return actual;
        }

        /// <summary>
        /// Asserts that the value is of the given type, inherits or implements it.
        /// </summary>
        /// <example>
        /// <code>
        /// IPrincipal principal = factory.CreateGroupPrincipal("foo");
        /// 
        /// principal.ShouldBeOfType&lt;GroupPrincipal&gt;();
        /// </code>
        /// </example>
        public static TExpected ShouldBeOfType<TExpected>(this object actual)
        {
            Assert.IsAssignableFrom(typeof(TExpected), actual);
            return (TExpected)actual;
        }

        /// <summary>
        /// Asserts that the value is of the given type (not a descendant).
        /// </summary>
        /// <example>
        /// <code>
        /// IPrincipal principal = factory.CreateGroupPrincipal("foo");
        /// 
        /// principal.ShouldBeOfExactType&lt;GroupPrincipal&gt;();
        /// </code>
        /// </example>
        public static object ShouldBeOfExactType(this object actual, Type expected)
        {
            ShouldNotBeNull(actual);
            Assert.AreEqual(expected, actual.GetType());
            return actual;
        }

        /// <summary>
        /// Asserts that the value is of the given type (not a descendant).
        /// </summary>
        /// <example>
        /// <code>
        /// IPrincipal principal = factory.CreateGroupPrincipal("foo");
        /// 
        /// principal.ShouldBeOfExactType&lt;GroupPrincipal&gt;();
        /// </code>
        /// </example>
        public static TExpected ShouldBeOfExactType<TExpected>(this object actual)
        {
            return (TExpected)ShouldBeOfExactType(actual, typeof(TExpected));
        }

        /// <summary>
        /// Asserts that the value is not of the given type, or inherits or implements it.
        /// </summary>
        /// <example>
        /// <code>
        /// IPrincipal principal = factory.CreatePrincipal("foo");
        /// 
        /// principal.ShouldNotBeOfType(typeof(GroupPrincipal));
        /// </code>
        /// </example>
        public static object ShouldNotBeOfType(this object actual, Type expected)
        {
            Assert.IsNotAssignableFrom(expected, actual);
            return actual;
        }

        /// <summary>
        /// Asserts that the value is not of the given type, or inherits or implements it.
        /// </summary>
        /// <example>
        /// <code>
        /// IPrincipal principal = factory.CreatePrincipal("foo");
        /// 
        /// principal.ShouldNotBeOfType&lt;GroupPrincipal&gt;();
        /// </code>
        /// </example>
        public static object ShouldNotBeOfType<TExpected>(this object actual)
        {
            Assert.IsNotAssignableFrom(typeof(TExpected), actual);
            return actual;
        }

        /// <summary>
        /// Asserts that the given collection contains the value.
        /// </summary>
        /// <example>
        /// <code>
        /// var order = repository.CreateOrder(...);
        /// 
        /// repository.GetOrders().ShouldContain(order);
        /// </code>
        /// </example>
        public static IEnumerable ShouldContain(this IEnumerable collection, object expected)
        {
            Assert.Contains(expected, collection.Cast<object>().ToList());
            return collection;
        }

        /// <summary>
        /// Asserts that the given collection does not contains the value.
        /// </summary>
        /// <example>
        /// <code>
        /// var order = repository.CreateOrder(.../* IsPending = true */);
        /// 
        /// repository.GetOrders().ShouldNotContain(order);
        /// </code>
        /// </example>
        public static IEnumerable ShouldNotContain(this IEnumerable collection, object expected)
        {
            Assert.IsFalse(collection.Cast<object>().Contains(expected),
                    "Collection contains the value.");
            return collection;
        }

        /// <summary>
        /// Asserts that the given string is empty.
        /// </summary>
        /// <example>
        /// <code>
        /// string id = repository.FindId(noMatchCriteria);
        /// 
        /// id.ShouldBeEmpty();
        /// </code>
        /// </example>
        public static void ShouldBeEmpty(this string @string)
        {
            Assert.IsEmpty(@string);
        }

        /// <summary>
        /// Asserts that the given string is not empty.
        /// </summary>
        /// <example>
        /// <code>
        /// string id = repository.FindId(matchCriteria);
        /// 
        /// id.ShouldNotBeEmpty();
        /// </code>
        /// </example>
        public static string ShouldNotBeEmpty(this string @string)
        {
            Assert.IsNotEmpty(@string);
            return @string;
        }

        /// <summary>
        /// Asserts that the given collection is empty.
        /// </summary>
        /// <example>
        /// <code>
        /// repository.Clear();
        /// 
        /// repository.GetOrders().ShouldBeEmpty();
        /// </code>
        /// </example>
        public static void ShouldBeEmpty(this IEnumerable collection)
        {
            Assert.IsEmpty(collection.Cast<object>().ToList());
        }

        /// <summary>
        /// Asserts that the given collection is not empty.
        /// </summary>
        /// <example>
        /// <code>
        /// var order = repository.CreateOrder(...);
        /// 
        /// repository.GetOrders().ShouldNotBeEmpty();
        /// </code>
        /// </example>
        public static IEnumerable ShouldNotBeEmpty(this IEnumerable collection)
        {
            Assert.IsNotEmpty(collection.Cast<object>().ToList());
            return collection;
        }

        /// <summary>
        /// Asserts that the value contains the expected substring.
        /// </summary>
        /// <example>
        /// <code>
        /// string message = messaging.GetWelcomeMessage();
        /// 
        /// message.ShouldContain("Joe");
        /// </code>
        /// </example>
        public static void ShouldContain(this string actual, string expected)
        {
            Assert.IsTrue(actual.Contains(expected),
                    String.Format("\"{0}\" does not contain \"{1}\".", actual, expected));
        }

        /// <summary>
        /// Asserts that the value does not contains the expected substring.
        /// </summary>
        /// <example>
        /// <code>
        /// string message = messaging.GetWelcomeMessage();
        /// 
        /// message.ShouldNotContain("Mary");
        /// </code>
        /// </example>
        public static void ShouldNotContain(this string actual, string expected)
        {
            Assert.IsFalse(actual.Contains(expected),
                    String.Format("\"{0}\" contains \"{1}\".", actual, expected));
        }

        /// <summary>
        /// Asserts that the exception contains the given string.
        /// </summary>
        /// <example>
        /// <code>
        /// try
        /// {
        ///   repository.CreateOrder(...);
        /// }
        /// catch (Exception ex)
        /// {
        ///   ex.ShouldContainErrorMessage("invalid CustomerId");
        /// }
        /// </code>
        /// </example>
        public static void ShouldContainErrorMessage(this Exception exception, string expected)
        {
            ShouldContain(exception.Message, expected);
        }

        /// <summary>
        /// Asserts that the given exception type was thrown by the action.
        /// </summary>
        /// <example>
        /// <code>
        /// typeof(RepositoryException).ShouldBeThrownBy(() => repository.CreateOrder(...));
        /// </code>
        /// </example>
        public static Exception ShouldBeThrownBy(this Type exceptionType, Action actionThatThrows)
        {
            Exception exception = actionThatThrows.GetException();

            Assert.IsNotNull(exception);
            Assert.AreEqual(exceptionType, exception.GetType());

            return exception;
        }

        /// <summary>
        /// Asserts that the given exception type was thrown by the action, and that 
        /// the exception satisfies the condition.
        /// </summary>
        /// <example>
        /// <code>
        /// typeof(RepositoryException)
        ///   .ShouldBeThrownBy(
        ///             () => repository.CreateOrder(...), 
        ///             e => e.Reason == Reason.InvalidData);
        /// </code>
        /// </example>
        public static Exception ShouldBeThrownBy(this Type exceptionType, Action actionThatThrows,
                Predicate<Exception> exceptionCondition)
        {
            Exception exception = actionThatThrows.GetException();

            Assert.IsNotNull(exception);
            Assert.AreEqual(exceptionType, exception.GetType());

            if (!exceptionCondition(exception))
                throw new ArgumentException("Exception throw did not satisfy the condition.");

            return exception;
        }

        /// <summary>
        /// Retrieves the exception throw by the action, or <see langword="null"/>.
        /// </summary>
        public static Exception GetException(this Action actionThatThrows)
        {
            Exception exception = null;

            try
            {
                actionThatThrows();
            }
            catch (Exception e)
            {
                exception = e;
            }

            return exception;
        }
    }
