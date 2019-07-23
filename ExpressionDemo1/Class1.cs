using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpressionDemo1
{
    public interface IBar<out TOut>
    { }

    public class Bar<TOut> : IBar<TOut>
    {
        public TOut BarOut { get; set; }

        public TOut Output()
        {
            return BarOut;
        }
    }

    public interface IFoo<in TIn, out TOut> // TIn 就是反变，TOut就是协变
    {
        TOut Output();
        void Intput(TIn value);
    }

    public class Foo<TIn, TOut> : IFoo<TIn, TOut>
    {
        public TOut FooOut { get; set; }

        public TOut Output()
        {
            return FooOut;
        }

        public void Intput(TIn value)
        {
            Console.WriteLine(value.ToString());
        }
    }


}
