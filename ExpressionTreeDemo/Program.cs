using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ExpressionTreeDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Expression<Func<int, bool>> exprTree = num => num > 5;

            ParameterExpression param = (ParameterExpression)exprTree.Parameters[0];
            BinaryExpression operation = (BinaryExpression)exprTree.Body;
            ParameterExpression left = (ParameterExpression)operation.Left;
            ConstantExpression right = (ConstantExpression)operation.Right;

            Console.WriteLine("Decomposed expression: {0} => {1} {2} {3}", param.Name, left.Name, operation.NodeType, right.Value);

            ParameterExpression numParam = Expression.Parameter(typeof(int), "num");
            ConstantExpression five = Expression.Constant(5, typeof(int));
            BinaryExpression numLessThanFive = Expression.LessThan(numParam, five);
            Expression<Func<int, bool>> lambda1 = Expression.Lambda<Func<int, bool>>(numLessThanFive, new ParameterExpression[] { numParam });

            // Let the compiler generate the expression tree for
            // the lambda expression num => num < 5.
            Expression<Func<int, bool>> lambda2 = num => num < 5;

            //创建一个 lambda 表达式并执行它
            // The expression tree to execute.
            BinaryExpression be = Expression.Power(Expression.Constant(2D), Expression.Constant(3D));

            // Create a lambda expression.
            Expression<Func<double>> le = Expression.Lambda<Func<double>>(be);

            // Compile the lambda expression.
            Func<double> compiledExpression = le.Compile();

            // Execute the lambda expression.
            double result = compiledExpression();

            Console.WriteLine(result);

            //修改表达式目录树

            Expression<Func<string, bool>> expr = name =>name.Length > 10 && name.StartsWith("G"); ;
            Console.WriteLine(expr);

            AndAlsoModifier treeModifier = new AndAlsoModifier();
            Expression modifiedExpr = treeModifier.Modify(expr);

            Console.WriteLine(modifiedExpr);

            Console.ReadLine();
        }
    }

    

}
