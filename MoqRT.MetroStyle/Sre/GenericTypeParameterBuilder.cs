namespace System.Reflection.Emit
{
    public class GenericTypeParameterBuilder
    {
        public string Name
        {
            get
            {
                throw new NotImplementedException("This operation has not been implemented.");
            }
        }

        internal Type MakeArrayType()
        {
            throw new NotImplementedException();
        }

        internal Type MakeArrayType(int rank)
        {
            throw new NotImplementedException();
        }

        internal void SetGenericParameterAttributes(GenericParameterAttributes attributes)
        {
            throw new NotImplementedException();
        }

        internal void SetInterfaceConstraints(Type[] constraints)
        {
            throw new NotImplementedException();
        }

        internal void SetCustomAttribute(CustomAttributeBuilder attribute)
        {
            throw new NotImplementedException();
        }
    }
}
