using Castle.DynamicProxy.Generators.Emitters;
using MoqRT;
using MoqRT.Reflection;

namespace System.Reflection.Emit
{
    public class TypeBuilder
    {
        public string Name
        {
            get
            {
                throw new NotImplementedException("This operation has not been implemented.");
            }
        }

        public Type BaseType
        {
            get
            {
                throw new NotImplementedException("This operation has not been implemented.");
            }
        }

        public bool IsInterface
        {
            get
            {
                throw new NotImplementedException("This operation has not been implemented.");
            }
        }

        public bool IsGenericType
        {
            get
            {
                throw new NotImplementedException("This operation has not been implemented.");
            }
        }

        public bool IsGenericTypeDefinition
        {
            get
            {
                throw new NotImplementedException("This operation has not been implemented.");
            }
        }

        internal ApplyGenArgs DefineGenericParameters
        {
            get
            {
                throw new NotImplementedException("This operation has not been implemented.");
            }
        }

        internal void DefineMethodOverride(MethodBuilder methodBuilder, IMethodInfo MethodToOverride)
        {
            throw new NotImplementedException();
        }

        internal void SetCustomAttribute(CustomAttributeBuilder attribute)
        {
            throw new NotImplementedException();
        }

        internal Type CreateType()
        {
            throw new NotImplementedException();
        }

        internal static void AddInterfaceImplementation(Type inter)
        {
            throw new NotImplementedException();
        }

        internal static void SetParent(Type baseType)
        {
            throw new NotImplementedException();
        }

        internal ConstructorBuilder DefineConstructor(MethodAttributes methodAttributes, CallingConventions callingConventions, Type[] args)
        {
            throw new NotImplementedException();
        }

        internal EventBuilder DefineEvent(string name, EventAttributes attributes, Type type)
        {
            throw new NotImplementedException();
        }

        internal MethodBuilder DefineMethod(string name, MethodAttributes attributes)
        {
            throw new NotImplementedException();
        }

        internal TypeBuilder DefineNestedType(string name, TypeAttributes attributes, Type baseType, Type[] interfaces)
        {
            throw new NotImplementedException();
        }

        internal FieldBuilder DefineField(string name, Type fieldType, FieldAttributes attributes)
        {
            throw new NotImplementedException();
        }

        internal PropertyBuilder DefineProperty(string name, PropertyAttributes attributes, Type propertyType, object[] args)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}
