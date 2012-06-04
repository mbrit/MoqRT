// Copyright 2004-2011 Castle Project - http://www.castleproject.org/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Castle.DynamicProxy.Tokens
{
	using System;
	using System.Reflection;

	using Castle.DynamicProxy.Internal;
    using MoqRT;
    using MoqRT.Reflection;

	/// <summary>
	///   Holds <see cref = "IMethodInfo" /> objects representing methods of <see cref = "AbstractInvocation" /> class.
	/// </summary>
	public static class InvocationMethods
	{
		public static readonly IConstructorInfo CompositionInvocationConstructorNoSelector =
			typeof(CompositionInvocation).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null,
			                                             new[]
			                                             {
			                                             	typeof(object),
			                                             	typeof(object),
			                                             	typeof(IInterceptor[]),
			                                             	typeof(MethodInfo),
			                                             	typeof(object[])
			                                             },
			                                             null).AsIConstructorInfo();

		public static readonly IConstructorInfo CompositionInvocationConstructorWithSelector =
			typeof(CompositionInvocation).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null,
			                                             new[]
			                                             {
			                                             	typeof(object),
			                                             	typeof(object),
			                                             	typeof(IInterceptor[]),
			                                             	typeof(MethodInfo),
			                                             	typeof(object[]),
			                                             	typeof(IInterceptorSelector),
			                                             	typeof(IInterceptor[]).MakeByRefType()
			                                             },
                                                         null).AsIConstructorInfo();

		public static readonly IMethodInfo EnsureValidTarget =
			typeof(CompositionInvocation).GetMethod("EnsureValidTarget", BindingFlags.Instance | BindingFlags.NonPublic).AsIMethodInfo();

		public static readonly IMethodInfo GetArgumentValue =
            typeof(AbstractInvocation).GetMethod("GetArgumentValue").AsIMethodInfo();

		public static readonly IMethodInfo GetArguments =
            typeof(AbstractInvocation).GetMethod("get_Arguments").AsIMethodInfo();

		public static readonly IMethodInfo GetReturnValue =
            typeof(AbstractInvocation).GetMethod("get_ReturnValue").AsIMethodInfo();

		public static readonly IConstructorInfo InheritanceInvocationConstructorNoSelector =
			typeof(InheritanceInvocation).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null,
			                                             new[]
			                                             {
			                                             	typeof(Type),
			                                             	typeof(object),
			                                             	typeof(IInterceptor[]),
			                                             	typeof(MethodInfo),
			                                             	typeof(object[])
			                                             },
                                                         null).AsIConstructorInfo();

		public static readonly IConstructorInfo InheritanceInvocationConstructorWithSelector =
			typeof(InheritanceInvocation).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null,
			                                             new[]
			                                             {
			                                             	typeof(Type),
			                                             	typeof(object),
			                                             	typeof(IInterceptor[]),
			                                             	typeof(MethodInfo),
			                                             	typeof(object[]),
			                                             	typeof(IInterceptorSelector),
			                                             	typeof(IInterceptor[]).MakeByRefType()
			                                             },
                                                         null).AsIConstructorInfo();

		public static readonly IMethodInfo Proceed =
            typeof(AbstractInvocation).GetMethod("Proceed", BindingFlags.Instance | BindingFlags.Public).AsIMethodInfo();

		public static readonly IFieldInfo ProxyObject =
			typeof(AbstractInvocation).GetField("proxyObject", BindingFlags.Instance | BindingFlags.NonPublic).AsIFieldInfo();

		public static readonly IMethodInfo SetArgumentValue =
			typeof(AbstractInvocation).GetMethod("SetArgumentValue").AsIMethodInfo();

		public static readonly IMethodInfo SetGenericMethodArguments =
            typeof(AbstractInvocation).GetMethod("SetGenericMethodArguments", new[] { typeof(Type[]) }).AsIMethodInfo();

		public static readonly IMethodInfo SetReturnValue =
            typeof(AbstractInvocation).GetMethod("set_ReturnValue").AsIMethodInfo();

		public static readonly IFieldInfo Target =
			typeof(CompositionInvocation).GetField("target", BindingFlags.Instance | BindingFlags.NonPublic).AsIFieldInfo();

		public static readonly IMethodInfo ThrowOnNoTarget =
			typeof(AbstractInvocation).GetMethod("ThrowOnNoTarget", BindingFlags.Instance | BindingFlags.NonPublic).AsIMethodInfo();
	}
}