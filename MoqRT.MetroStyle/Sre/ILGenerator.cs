using Moq.Reflection.Emit;
using MoqRT.Reflection;

namespace System.Reflection.Emit
{
    public class ILGenerator : EmitWrapper
    {
        internal ILGenerator(object inner)
            : base(inner)
        {
        }

        internal void Emit(OpCode opCode)
        {
            this.Invoke("Emit", opCode);
        }

        internal void Emit(OpCode opCode, byte b)
        {
            this.Invoke("Emit", new Type[] { typeof(OpCode), typeof(byte) }, opCode, b);
        }

        internal void Emit(OpCode opCode, double d)
        {
            this.Invoke("Emit", new Type[] { typeof(OpCode), typeof(double) }, opCode, d);
        }

        internal void Emit(OpCode opCode, short s)
        {
            this.Invoke("Emit", new Type[] { typeof(OpCode), typeof(short) }, opCode, s);
        }

        internal void Emit(OpCode opCode, int i)
        {
            this.Invoke("Emit", new Type[] { typeof(OpCode), typeof(int) }, opCode, i);
        }

        internal void Emit(OpCode opCode, long l)
        {
            this.Invoke("Emit", new Type[] { typeof(OpCode), typeof(long) }, opCode, l);
        }

        internal void Emit(OpCode opCode, string s)
        {
            this.Invoke("Emit", new Type[] { typeof(OpCode), typeof(string) }, opCode, s);
        }

        internal void Emit(OpCode opCode, Type type)
        {
            this.Invoke("Emit", new Type[] { typeof(OpCode), typeof(Type) }, opCode, type);
        }

        internal void Emit(OpCode opCode, MemberInfo info)
        {
            // clunky - but the WinRT checking shim cannot dereference RuntimeTypes
            // back into normal types... (i.e. you can't put info.GetType() in the check
            // parameter)...
            if (info is ConstructorInfo)
                this.Invoke("Emit", new Type[] { typeof(OpCode), typeof(ConstructorInfo) }, opCode, info);
            else if (info is FieldInfo)
                this.Invoke("Emit", new Type[] { typeof(OpCode), typeof(FieldInfo) }, opCode, info);
            else if (info is MethodInfo)
                this.Invoke("Emit", new Type[] { typeof(OpCode), typeof(MethodInfo) }, opCode, info);
            else
                throw new NotSupportedException(string.Format("Cannot handle '{0}'.", info.GetType()));
        }

        internal void Emit(OpCode opCode, LocalBuilder builder)
        {
            var real = Mscorlib.GetType("System.Reflection.Emit.LocalBuilder");
            this.Invoke("Emit", new Type[] { typeof(OpCode), real }, opCode, builder.Inner);
        }

        internal void Emit(OpCode opCode, Label builder)
        {
            var real = Mscorlib.GetType("System.Reflection.Emit.Label");
            this.Invoke("Emit", new Type[] { typeof(OpCode), real }, opCode, builder.Inner);
        }

        internal LocalBuilder DeclareLocal(Type type)
        {
            var result = this.Invoke("DeclareLocal", type);
            return new LocalBuilder(result);
        }

        internal void EndExceptionBlock()
        {
            this.Invoke();
        }

        internal void BeginFinallyBlock()
        {
            this.Invoke();
        }

        internal Label DefineLabel()
        {
            var result = this.Invoke();
            return new Label(result);
        }

        internal void MarkLabel(Label label)
        {
            this.Invoke("MarkLabel", label);
        }

        internal void BeginExceptionBlock()
        {
            this.Invoke();
        }
    }
}
