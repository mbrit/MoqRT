//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using System.Threading.Tasks;
//using Moq.Driver;
//using Moq.Language.Flow;

//namespace Moq.Baking
//{
//    internal class BakingSetup<TMock, TResult> : ISetup<TMock, TResult>
//        where TMock : class
//    {
//        private RealISetupWrapper<TMock, TResult> RealSetup { get; set; }

//        internal BakingSetup(RealMockWrapper<TMock> real, Expression<Func<TMock, TResult>> expression)
//        {
//            // create a real setup...
//            object result = real.Invoke("Setup", new Type[] { typeof(TMock), typeof(TResult) }, 
//                new Type[] { typeof(Expression<Func<TMock, TResult>>) }, expression);
//            this.RealSetup = new RealISetupWrapper<TMock, TResult>(result);
//        }

//        public void Returns(object value)
//        {
//            // call...
//            this.RealSetup.Invoke("Returns", null, new Type[] { typeof(object) }, value);
//        }
//    }
//}
