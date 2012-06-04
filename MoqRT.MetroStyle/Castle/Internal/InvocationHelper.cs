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

namespace Castle.DynamicProxy.Internal
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Reflection;

	using Castle.Core.Internal;
	using Castle.DynamicProxy.Generators;
    using MoqRT;
    using MoqRT.Reflection;

	public static class InvocationHelper
	{
		private static readonly Dictionary<KeyValuePair<IMethodInfo, Type>, IMethodInfo> cache =
			new Dictionary<KeyValuePair<IMethodInfo, Type>, IMethodInfo>();

		private static readonly Lock @lock = Lock.Create();

		public static IMethodInfo GetMethodOnObject(object target, IMethodInfo proxiedMethod)
		{
			if (target == null)
			{
				return null;
			}

			return GetMethodOnType(target.GetType(), proxiedMethod);
		}

		public static IMethodInfo GetMethodOnType(Type type, IMethodInfo proxiedMethod)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}

			Debug.Assert(proxiedMethod.DeclaringType.IsAssignableFrom(type),
			             "proxiedMethod.DeclaringType.IsAssignableFrom(type)");
			using (var locker = @lock.ForReadingUpgradeable())
			{
				var methodOnTarget = GetFromCache(proxiedMethod, type);
				if (methodOnTarget != null)
				{
					return methodOnTarget;
				}
				locker.Upgrade();

				methodOnTarget = GetFromCache(proxiedMethod, type);
				if (methodOnTarget != null)
				{
					return methodOnTarget;
				}
				methodOnTarget = ObtainMethod(proxiedMethod, type);
				PutToCache(proxiedMethod, type, methodOnTarget);
				return methodOnTarget;
			}
		}

		private static IMethodInfo GetFromCache(IMethodInfo IMethodInfo, Type type)
		{
			var key = new KeyValuePair<IMethodInfo, Type>(IMethodInfo, type);
			IMethodInfo method;
			cache.TryGetValue(key, out method);
			return method;
		}

		private static IMethodInfo ObtainMethod(IMethodInfo proxiedMethod, Type type)
		{
			Type[] genericArguments = null;
			if (proxiedMethod.IsGenericMethod)
			{
				genericArguments = proxiedMethod.GetGenericArguments();
				proxiedMethod = proxiedMethod.GetGenericMethodDefinition();
			}
			var declaringType = proxiedMethod.DeclaringType;
			IMethodInfo methodOnTarget = null;
			if (declaringType.IsInterface())
			{
				var mapping = type.GetInterfaceMap(declaringType);
				var index = Array.IndexOf(mapping.InterfaceMethods, proxiedMethod);
				Debug.Assert(index != -1);
				methodOnTarget = mapping.TargetMethods[index].AsIMethodInfo();
			}
			else
			{
				// NOTE: this implementation sucks, feel free to improve it.
				var methods = MethodFinder.GetAllInstanceMethods(type, BindingFlags.Public | BindingFlags.NonPublic);
				foreach (var method in methods)
				{
					if (MethodSignatureComparer.Instance.Equals(method.GetBaseDefinition(), proxiedMethod))
					{
						methodOnTarget = method;
						break;
					}
				}
			}
			if (methodOnTarget == null)
			{
				throw new ArgumentException(
					string.Format("Could not find method overriding {0} on type {1}. This is most likely a bug. Please report it.",
					              proxiedMethod, type));
			}

			if (genericArguments == null)
			{
				return methodOnTarget;
			}
			return methodOnTarget.MakeGenericMethod(genericArguments);
		}

		private static void PutToCache(IMethodInfo IMethodInfo, Type type, IMethodInfo value)
		{
			var key = new KeyValuePair<IMethodInfo, Type>(IMethodInfo, type);
			cache.Add(key, value);
		}
	}
}