using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Moq.Properties
{
    // @mbrit - 2012-06-03 - shim that looks like a resources assembly...
    internal static class Resources
    {
        internal static ResourceManager ResourceManager { get; private set; }

        static Resources()
        {
            ResourceManager = new ResourceManager();
        }

        public static string FieldsNotSupported
        {
            get { return GetString(); }
        }

        public static string PropertyGetNotFound
        {
            get { return GetString(); }
        }

        public static string PropertySetNotFound
        {
            get { return GetString(); }
        }

        public static string EventNofFound
        {
            get { return GetString(); }
        }

        public static string NoMatchingCallsAtLeast
        {
            get { return GetString(); }
        }

        public static string NoMatchingCallsAtLeastOnce
        {
            get { return GetString(); }
        }

        public static string NoMatchingCallsAtMost
        {
            get { return GetString(); }
        }

        public static string NoMatchingCallsAtMostOnce
        {
            get { return GetString(); }
        }

        public static string NoMatchingCallsBetweenExclusive
        {
            get { return GetString(); }
        }

        public static string NoMatchingCallsBetweenInclusive
        {
            get { return GetString(); }
        }

        public static string NoMatchingCallsExactly
        {
            get { return GetString(); }
        }

        public static string NoMatchingCallsNever
        {
            get { return GetString(); }
        }

        public static string NoMatchingCallsOnce
        {
            get { return GetString(); }
        }

        public static string VerifyOnVirtualMember
        {
            get { return GetString(); }
        }

        public static string InvalidMockGetType
        {
            get { return GetString(); }
        }

        public static string ObjectInstanceNotMock
        {
            get { return GetString(); }
        }

        public static string SetupOnNonOverridableMember
        {
            get { return GetString(); }
        }

        public static string SetupNotSetter
        {
            get { return GetString(); }
        }

        public static string PropertyNotWritable
        {
            get { return GetString(); }
        }

        public static string PropertyNotReadable
        {
            get { return GetString(); }
        }

        public static string SetupNonOverrideableMember
        {
            get { return GetString(); }
        }

        public static string VerifyOnNonVirtualMember
        {
            get { return GetString(); }
        }

        public static string SetupOnNonMemberMethod
        {
            get { return GetString(); }
        }

        public static string RaisedUnassociatedEvent
        {
            get { return GetString(); }
        }

        public static string AlreadyInitialized
        {
            get { return GetString(); }
        }

        public static string AsMustBeInterface
        {
            get { return GetString(); }
        }

        public static string ConstructorArgsForInterface
        {
            get { return GetString(); }
        }

        public static string UnsupportedExpression
        {
            get { return GetString(); }
        }

        public static string SetupNotMethod
        {
            get { return GetString(); }
        }

        public static string SetupNotProperty
        {
            get { return GetString(); }
        }

        public static string InvalidMockClass
        {
            get { return GetString(); }
        }

        public static string TypeNotImplementInterface
        {
            get { return GetString(); }
        }

        public static string TypeNotInheritFromType
        {
            get { return GetString(); }
        }

        public static string VerficationFailed
        {
            get { return GetString(); }
        }

        public static string MemberMissing
        {
            get { return GetString(); }
        }

        public static string MethodIsPublic
        {
            get { return GetString(); }
        }

        public static string UnexpectedPublicProperty
        {
            get { return GetString(); }
        }

        public static string CantSetReturnValueForVoid
        {
            get { return GetString(); }
        }

        public static string Culture
        {
            get { return GetString(); }
        }

        public static string UnsupportedMember
        {
            get { return GetString(); }
        }

        public static string ConstructorNotFound
        {
            get { return GetString(); }
        }

        public static string ArgumentCannotBeEmpty
        {
            get { return GetString(); }
        }

        public static string LinqMethodNotSupported
        {
            get { return GetString(); }
        }

        public static string LinqMethodNotVirtual
        {
            get { return GetString(); }
        }

        public static string LinqBinaryOperatorNotSupported
        {
            get { return GetString(); }
        }

        public static string SetupLambda
        {
            get { return GetString(); }
        }

        public static string OutExpressionMustBeConstantValue
        {
            get { return GetString(); }
        }

        public static string RefExpressionMustBeConstantValue
        {
            get { return GetString(); }
        }

        public static string MockExceptionMessage
        {
            get { return GetString(); }
        }

        private static string GetString([CallerMemberName] string name = null)
        {
            return ResourceManager.GetString(name);
        }
    }
}
