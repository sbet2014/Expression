using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpressionDemo1
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (int i in TestSet.Test())
                Console.WriteLine(i);

            IBar<Cat> bar = new Bar<Cat>();
            IBar<Animal> varianceBar = bar;//协变

            Func<Animal, string> cat = (ani) => {  return ani.Name; };
            Func<Cat,string> catVariance = cat;//逆变
            //Console.WriteLine(catVariance(new Cat { Name = "逆变猫" }));
            Console.WriteLine(GetCatName(cat));

            Func<object, string> fun = (name) => { return name.ToString(); };
            Func<string, object> funVariance = fun;

            //Func<int, bool> fun = (index) => { return index>0; };
            //Func<int, object> funVariance = fun;

            IFoo<Cat, Animal> catFoo = new Foo<Animal, Cat>();
            IFoo<object, string> source = new Foo<object, string>();
            IFoo<string, object> sourceVariance = source;//逆变
            source.Intput("ABC");
            object result = source.Output();

            IFoo<object, object> co = source;

            Console.ReadLine();
        }

        public static string GetCatName(Func<Animal,string> cat)
        {
           return cat(new Cat { Name = "猫" });
        }

        public class Animal
        {
            public string Name { get; set; }
        }

        public class Cat : Animal
        { }
    }
}
