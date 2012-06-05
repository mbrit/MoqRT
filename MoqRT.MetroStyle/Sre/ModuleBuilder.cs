using Moq.Reflection.Emit;
namespace System.Reflection.Emit
{
    public class ModuleBuilder : EmitWrapper
    {
        internal ModuleBuilder(object inner)
            : base(inner)
        {
        }

        internal TypeBuilder DefineType(string name, TypeAttributes flags)
        {
            var result = this.Invoke("DefineType", new Type[] { typeof(string), typeof(TypeAttributes) },
                name, flags);
            return new TypeBuilder(result);
        }

        internal string FullyQualifiedName
        {
            get
            {
                return this.GetProperty<string>();
            }
        }

        internal Assembly Assembly()
        {
            return this.GetProperty<Assembly>("Assembly");
        }
    }
}
