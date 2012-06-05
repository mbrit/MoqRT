namespace System.Reflection.Emit
{
    public class EventBuilder : MemberBuilder
    {
        internal EventBuilder(object inner)
            : base(inner)
        {
        }

        internal void SetAddOnMethod(MethodBuilder methodBuilder)
        {
            throw new NotImplementedException();
        }

        internal void SetRemoveOnMethod(MethodBuilder methodBuilder)
        {
            throw new NotImplementedException();
        }
    }
}
