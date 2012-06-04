//#if NETFX_CORE

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection.Emit;
//using System.Text;
//using System.Threading.Tasks;
//using Castle.DynamicProxy.Generators.Emitters;
//using Castle.DynamicProxy.Generators.Emitters.SimpleAST;

//namespace Castle.DynamicProxy.Generators.Emitters.SimpleAST
//{
//	public class ExplicitConvertExpression : ConvertExpression
//	{
//		public ExplicitConvertExpression(Type targetType, Expression right)
//			: base(targetType, right)
//		{
//		}

//		public override void Emit(IMemberEmitter member, ILGenerator gen)
//		{
//			if (fromType == typeof(object) && target == typeof(int))
//			{
//				throw new NotImplementedException("This operation has not been implemented.");
//			}
//			else
//				base.Emit(member, gen);
//		}
//	}
//}

//#endif