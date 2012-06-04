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

namespace Castle.DynamicProxy.Generators.Emitters.SimpleAST
{
	using System.Reflection;
	using System.Reflection.Emit;
    using MoqRT;
    using MoqRT.Reflection;

	public class MethodInvocationExpression : Expression
	{
		protected readonly Expression[] args;
		protected readonly IMethodInfo method;
		protected readonly Reference owner;
		
		public MethodInvocationExpression(IMethodInfo method, params Expression[] args) :
			this(SelfReference.Self, method, args)
		{
		}

		public MethodInvocationExpression(MethodEmitter method, params Expression[] args) :
			this(SelfReference.Self, method.MethodBuilder.AsIMethodInfo(), args)
		{
		}

		public MethodInvocationExpression(Reference owner, MethodEmitter method, params Expression[] args) :
			this(owner, method.MethodBuilder.AsIMethodInfo(), args)
		{
		}

		public MethodInvocationExpression(Reference owner, IMethodInfo method, params Expression[] args)
		{
			this.owner = owner;
			this.method = method;
			this.args = args;
		}

		public bool VirtualCall { get; set; }

		public override void Emit(IMemberEmitter member, ILGenerator gen)
		{
			ArgumentsUtil.EmitLoadOwnerAndReference(owner, gen);

			foreach (var exp in args)
			{
				exp.Emit(member, gen);
			}

			if (VirtualCall)
			{
				gen.Emit(OpCodes.Callvirt, method.AsMethodInfo());
			}
			else
			{
				gen.Emit(OpCodes.Call, method.AsMethodInfo());
			}
		}
	}
}