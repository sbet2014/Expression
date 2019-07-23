using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace ExpressionShow
{
    class Program
    {
        static void Main(string[] args)
        {
            //GetEmploye(CreateExpressionWithAge(25));
            //GetEmploye(CreateExpressionWithName("C"));


            //ShowAction(100);
            //ShowTwoAction(1000);

            Console.WriteLine(ETFact(190));

            //IQueryable<int> values = new int[] { 1 }.AsQueryable();
            //int last = 0;
            //var query = from i in values
            //            where  Create(i)
            //            select i;
            //    //{
            //        //last = item;
            //        //Console.WriteLine("Last value ={0}", last);
            //        //return (item % 2) == 0;
            //    //}
            ////);
            //var result = query.ToArray();

            GetAllEmploye().Where(e => e.Age > 25);

            Console.ReadLine();
        }

        //static Expression<Func<int, bool>> Create(int item, int last)
        //{
        //    last = item;
        //    Console.WriteLine("Last value ={0}", item);
        //    return (i % 2) == 0;

        //    Expression.Assign(last, Expression.Constant(item));

        //    ParameterExpression parameter = Expression.Parameter(typeof(int), "item");
        //    MethodCallExpression methodCall = Expression.Call(typeof(Console).GetMethod("WriteLine", new Type[] { typeof(int) }),
        //                                                  parameter);

        //    var result = Expression.Parameter(typeof(int), "result");
        //    Expression.Assign(last, Expression.Return();

        //    BlockExpression block = Expression.Block(methodCall);
      

        //    var expression = Expression.Lambda<Func<int, bool>>(block, parameter);
        //    return expression;
        //}

        //e=>e.Age==25
        static Expression<Func<Employe, bool>> CreateExpressionWithAge(int age)
        {
            ParameterExpression parameterEmploye = Expression.Parameter(typeof(Employe), "e");
            MemberExpression left = LambdaExpression.PropertyOrField(parameterEmploye, "Age");
            ConstantExpression right = Expression.Constant(age);

            var body = Expression.Equal(left, right);
            //ExpressionType op = ExpressionType.Equal;
            //var body = Expression.MakeBinary(op, left, right);

            Expression<Func<Employe, bool>> expression = Expression.Lambda<Func<Employe, bool>>(body,
                new ParameterExpression[] { parameterEmploye });

            return expression;
        }

        //e=>e.Name.StartsWith("C")
        //e=>e.Name.ToUpper(CultureInfo.CurrentCulture).StartsWith("C")
        static Expression<Func<Employe, bool>> CreateExpressionWithName(string matchName)
        {
            ParameterExpression parameterEmploye = Expression.Parameter(typeof(Employe), "e");
            MemberExpression left = LambdaExpression.PropertyOrField(parameterEmploye, "Name");

            MethodCallExpression body = Expression.Call(left,
                             typeof(string).GetMethod("ToUpper", new Type[] { typeof(CultureInfo) }),
                             Expression.Constant(CultureInfo.InvariantCulture));

            ConstantExpression right = Expression.Constant(matchName.ToUpper(CultureInfo.InvariantCulture));
            body = Expression.Call(body,
                             typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) }),
                             right);


            Expression<Func<Employe, bool>> expression = Expression.Lambda<Func<Employe, bool>>(body,
               new ParameterExpression[] { parameterEmploye });

            return expression;
        }

        static void GetEmploye(Expression<Func<Employe, bool>> predicate)
        {
            var es = GetAllEmploye().Where(predicate.Compile());
            foreach (var e in es)
                Console.WriteLine(e.Name);
        }

        static IEnumerable<Employe> GetAllEmploye()
        {
            return new List<Employe>()
            { 
                new Employe{ Name="Cary", Age=25},
                new Employe{ Name="James", Age=24},
                new Employe{ Name="zhengming", Age=28},
                new Employe{ Name="Tom", Age=21},
                new Employe{ Name="Jack", Age=20} 
            };
        }

        static void ShowExpression()
        {
            var ops = from m in typeof(Expression).GetMethods()
                      where m.IsStatic
                      group m by m.Name into g
                      select new { name = g.Key, OverLaod = g.Count() };

            foreach (var op in ops)
                Console.WriteLine(op);
        }

        //Expression<Action<int>> printExpr = (arg) => Console.WriteLine(arg);
        static void ShowAction(int arg)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(int), "arg");

            MethodCallExpression methodCall = Expression.Call(typeof(Console).GetMethod("WriteLine", new Type[] { typeof(int) }),
                                                           parameter);

            var actionLambda = Expression.Lambda<Action<int>>(methodCall, parameter);
            actionLambda.Compile()(arg);
        }

        //Expression<Action<int>> printTwoLinesExpr = (arg) =>
        //{
        //   Console.WriteLine("Print arg:");
        //   Console.WriteLine("Hello World "+arg);
        //}
        static void ShowTwoAction(int arg)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(int), "arg");

            MethodCallExpression firstMethodCall = Expression.Call(typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }),
                                                          Expression.Constant("Print arg:"));

            MethodCallExpression secondMethodCall = Expression.Call(typeof(Console).GetMethod("WriteLine", new Type[] { typeof(int) }),
                                                          parameter);

            BlockExpression block = Expression.Block(firstMethodCall, secondMethodCall);

            var actionLambda = Expression.Lambda<Action<int>>(block, parameter);
            actionLambda.Compile()(arg);
        }

        static int ETFact(int arg)
        {
            var value = Expression.Parameter(typeof(int), "value");
            var label = Expression.Label(typeof(int));//中断标志
            var result = Expression.Parameter(typeof(int), "result");

            var block = Expression.Block
             (
                //实例化一个局部变量
                 new[] { result },
                //初始化局部变量
                 Expression.Assign(result, Expression.Constant(1)),
                //创建一个方法体
                 Expression.Loop
                 (
                     // Adding a conditional block into the loop.
                     Expression.IfThenElse
                     (
                        //条件：value > 1
                        Expression.GreaterThan(value, Expression.Constant(1)),
                         //If true: result *= value --
                         //Expression.Block
                         //(
                         //   Expression.PostDecrementAssign(value),
                         //   Expression.ModuloAssign(result, value)
                         // ),
                         Expression.MultiplyAssign(result,Expression.PostDecrementAssign(value)),
                        // If false, exit from loop and go to a label.
                        Expression.Break(label, result)
                     ),
                     label
                 )
             );

            var expression = Expression.Lambda<Func<int, int>>(block, value);
            return expression.Compile()(arg);
        }
    }

    //public interface IBusinessObject<in U> where U : class
    //{
    //    IEnumerable<U> GetBusinessObjects(Expression<Func<U, bool>> predicate, IEnumerable<U> list);
    //}

    //public class BusinessObject<U> : IBusinessObject<U>
    //{
    //    //Expression<Func<T, bool>> Rules { get;}

    //    public IEnumerable<U> GetBusinessObjects(Expression<Func<U, bool>> predicate, IEnumerable<U> list)
    //    {
    //        return list.Where(predicate.Compile());
    //    }
    //}

    public class Employe
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public Employe()
        {
        }

        public Employe(string name, int age)
        {
            this.Name = name;
            this.Age = age;
        }

    }
}
