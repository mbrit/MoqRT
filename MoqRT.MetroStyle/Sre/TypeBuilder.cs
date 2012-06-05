using Castle.DynamicProxy.Generators.Emitters;
using Moq.Reflection.Emit;
using MoqRT;
using MoqRT.Reflection;

namespace System.Reflection.Emit
{
    public class TypeBuilder : EmitWrapper
    {
        internal TypeBuilder(object inner)
            : base(inner)
        {
        }

        public string Name
        {
            get
            {
                return this.GetProperty<string>();
            }
        }

        public Type BaseType
        {
            get
            {
                return this.GetProperty<Type>();
            }
        }

        public bool IsInterface
        {
            get
            {
                return this.GetProperty<bool>();
            }
        }

        public bool IsGenericType
        {
            get
            {
                return this.GetProperty<bool>();
            }
        }

        public bool IsGenericTypeDefinition
        {
            get
            {
                return this.GetProperty<bool>();
            }
        }

        internal ApplyGenArgs DefineGenericParameters
        {
            get
            {
                return new ApplyGenArgs((names) =>
                {
                    var result = this.Invoke();
                    throw new NotImplementedException("This operation has not been implemented.");
                });
            }
        }

        internal void DefineMethodOverride(IMethodInfo method, IMethodInfo toOverride)
        {
            this.Invoke("DefineMethodOverride", method.AsMethodInfo(), toOverride.AsMethodInfo());
        }

        internal void SetCustomAttribute(CustomAttributeBuilder attribute)
        {
            throw new NotImplementedException();
        }

        internal Type CreateType()
        {
            return (Type)this.Invoke();
        }

        internal void AddInterfaceImplementation(Type inter)
        {
            this.Invoke("AddInterfaceImplementation", inter);
        }

        internal void SetParent(Type baseType)
        {
            this.Invoke("SetParent", baseType);
        }

        internal ConstructorBuilder DefineConstructor(MethodAttributes methodAttributes, CallingConventions callingConventions, 
            Type[] args)
        {
            var result = this.Invoke("DefineConstructor", new Type[] { typeof(MethodAttributes), typeof(CallingConventions), 
                typeof(Type[]) }, methodAttributes, callingConventions, args);
            return new ConstructorBuilder(result);
        }

        internal EventBuilder DefineEvent(string name, EventAttributes attributes, Type type)
        {
            var result = this.Invoke("DefineEvent", new Type[] { typeof(string), typeof(EventAttributes), 
                typeof(Type) }, name, attributes, type);
            return new EventBuilder(result);
        }

        internal MethodBuilder DefineMethod(string name, MethodAttributes attributes)
        {
            var result = this.Invoke("DefineMethod", new Type[] { typeof(string), typeof(MethodAttributes) },
                name, attributes);
            return new MethodBuilder(result);
        }

        internal TypeBuilder DefineNestedType(string name, TypeAttributes attributes, Type baseType, Type[] interfaces)
        {
            var result = this.Invoke("DefineNestedType", new Type[] { typeof(string), typeof(TypeAttributes), 
                typeof(Type), typeof(Type[]) }, name, attributes, baseType, interfaces);
            return new TypeBuilder(result);
        }

        internal FieldBuilder DefineField(string name, Type fieldType, FieldAttributes attributes)
        {
            var result = this.Invoke("DefineField", new Type[] { typeof(string), typeof(Type), typeof(FieldAttributes) },
                name, fieldType, attributes);
            return new FieldBuilder(result);
        }

        internal PropertyBuilder DefineProperty(string name, PropertyAttributes attributes, Type propertyType, Type[] args)
        {
            var result = this.Invoke("DefineProperty", new Type[] { typeof(string), typeof(PropertyAttributes), typeof(Type), 
                typeof(Type[]) }, name, attributes, propertyType, args);
            return new PropertyBuilder(result);
        }

        internal static ConstructorInfo GetConstructor(Type type, ConstructorInfo ctr)
        {
            return GetConstructor(type, ctr.AsIConstructorInfo());
        }

        internal static ConstructorInfo GetConstructor(Type type, IConstructorInfo ctr)
        {
            throw new NotImplementedException();
        }

        internal ConstructorBuilder DefineTypeInitializer()
        {
            var result = this.Invoke("DefineTypeInitializer");
            return new ConstructorBuilder(result);
        }
    }
}
