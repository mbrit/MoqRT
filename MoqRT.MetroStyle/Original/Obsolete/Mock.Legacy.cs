﻿//Copyright (c) 2007. Clarius Consulting, Manas Technology Solutions, InSTEDD
//http://code.google.com/p/moq/
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

//    * Neither the name of Clarius Consulting, Manas Technology Solutions or InSTEDD nor the 
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

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Moq
{
	// Keeps legacy implementations.
	public partial class Mock
	{
		[Obsolete]
		internal static SetterMethodCall<T, TProperty> SetupSet<T, TProperty>(
			Mock mock,
			Expression<Func<T, TProperty>> expression, TProperty value)
			where T : class
		{
			var prop = expression.ToPropertyInfo();
			ThrowIfPropertyNotWritable(prop);

			var setter = prop.GetSetMethod();
			ThrowIfCantOverride(expression, setter);

			var call = new SetterMethodCall<T, TProperty>(mock, expression, setter, value);
			var targetInterceptor = GetInterceptor(((MemberExpression)expression.Body).Expression, mock);

			targetInterceptor.AddCall(call, SetupKind.PropertySet);

			return call;
		}

		[Obsolete]
		internal static void VerifySet<T, TProperty>(
			Mock mock,
			Expression<Func<T, TProperty>> expression,
			Times times,
			string failMessage)
			where T : class
		{
			var method = expression.ToPropertyInfo().GetSetMethod();
			ThrowIfVerifyNonVirtual(expression, method);

			var expected = new SetterMethodCall<T, TProperty>(mock, expression, method)
			{
				FailMessage = failMessage
			};
			VerifyCalls(GetInterceptor(((MemberExpression)expression.Body).Expression, mock), expected, expression, times);
		}

		[Obsolete]
		internal static void VerifySet<T, TProperty>(
			Mock mock,
			Expression<Func<T, TProperty>> expression,
			TProperty value,
			Times times,
			string failMessage)
			where T : class
		{
			var method = expression.ToPropertyInfo().GetSetMethod();
			ThrowIfVerifyNonVirtual(expression, method);

			var expected = new SetterMethodCall<T, TProperty>(mock, expression, method, value)
			{
				FailMessage = failMessage
			};
			VerifyCalls(GetInterceptor(((MemberExpression)expression.Body).Expression, mock), expected, expression, times);
		}
	}
}