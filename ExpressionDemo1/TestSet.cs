using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Data;

namespace ExpressionDemo1
{
    public class TestSet
    {
        //简单类型 int
        static List<int> list = new List<int>() { 1, 2, 3, 4 };
        public static IEnumerable<int> Test()
        {
            ParameterExpression parameter = Expression.Parameter(typeof(int), "p");

            var body = Expression.MakeBinary(ExpressionType.GreaterThan, parameter, Expression.Constant(1));

            var expression = Expression.Lambda<Func<int, bool>>(body, parameter);
            DataTable dt = new DataTable();
            DataColumn dl= new DataColumn();

            return TestSub(expression);
        }

        public static IEnumerable<int> TestSub(Expression<Func<int, bool>> predicate)
        {
            return list.Where(predicate.Compile());
        }
        
        // (a,n)=>a>1 && n
        //public static IEnumerable<int> TestComplex()
        //{ }
    }

    public class Person
    {
        public int Age { get; set; }
        public string Name { get; set; }

        public bool AgeIn(string name)
        {
            return name == "22";
        }
    }
}