using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    internal static class Program
    {
        public static List<double> Data = new List<double>() {7,5,4,7,8,58,54 };
        public enum ProductType
        {
            Food,
            Beverage,
            RawMaterial

        }
        public class Order
        {

            public int OrderID;
            public int ProductIndex;
            public double Quantity;
            public double UnitPrice;


        }
        // our pointer to function (Delegates)
        private static Func<int, (double x1, double x2)> A = ProductParamtersFood;
        private static Func<int, (double x1, double x2)> B = ProductParamtersBeverage;
        private static Func<int, (double x1, double x2)> C = ProductParamtersRawMaterial;
        private static Order R = new Order { OrderID = 10, ProductIndex = 100, Quantity = 20, UnitPrice = 4 };

        public static Func<double, double> MyComposedFunction =
           ComposeFunction(addOne, square, SubtractTen);

        static void Main(string[] args)
        {
            //Example :take Data List as input and first step add 1 to every item in the list 
            //second step pow(item) to every item in the list , third step subtract 10 to all item in list .

            // solve by Imparative 
            #region Imparative
            foreach (var item in Data)
            {
                Console.WriteLine(SubtractTen(square(addOne(item))).ToString());
            }

            #endregion

            //solve by Declarative
            #region Declarative
            Data.Select(addOne).Select(square).Select(SubtractTen).ToList().
                ForEach((x) => Console.WriteLine(x.ToString()));


            // solve it but not execute step 3 if the answer of first two step >15 and take first smallest two item
            // this is very easy with Declarative but hard in Imparative
            Data.Select(addOne).Select(square).Where(x => x > 15).OrderBy(x => x).Take(2).Select(SubtractTen).ToList().
               ForEach((x) => Console.WriteLine(x.ToString()));

            #endregion


            // Higher order function is a function that takes one or more functions
            // as arguments, or returns a function, or both.We are used to passing as
            // function parameters simple objects like integers or strings,
            // or more complex objects like collections and custom types.
            // But C# also has good support for HOFs. This is done
            // using delegates and lambda expressions.

            #region Higher-order function passing function as parm
            // delegate is pointer to function here: delegate take double as input and return double as output
            Func<double, double> DlgtTest1 = Test1;
            Func<double, double> DlgtTest2 = Test2;
            //create list of delagate to store delegates pointers of function
            List<Func<Double, Double>> z = new List<Func<Double, Double>>
            {
                DlgtTest1,
                DlgtTest2
            };
            // with normal one
            Console.WriteLine(Test2(Test1(5)).ToString());
            Console.WriteLine(Test1(Test2(5)).ToString());

            // with Delegate
            Console.WriteLine(z[0](5).ToString()); //call test1 and pass 5 as parameter
            Console.WriteLine(z[1](5).ToString());//call test2 and pass 5 as parameter
            Console.WriteLine(z[1].Invoke(5).ToString()); //same

            // with Higher-order function
            Console.WriteLine(Test3(Test1, 5).ToString());//call test3 and pass test1 as delegate and 5 as parms
            Console.WriteLine(Test3(Test2, 5).ToString());//call test3 and pass test2 as delegate and 5 as parms

            // Q :Why and when shuold i use Higher-order function ?
            // see the next example to calc discount using HOF's :
            var product = ProductType.Food;

            Func<int, (double x1, double x2)> P = (product == ProductType.Food) ? A : ((product == ProductType.Beverage) ? B : C);
            Console.WriteLine(ClaculateDiscount(P, R).ToString());
            Console.ReadLine();


            #endregion

            #region Higher-order function return function 
            // store delagte of function Test()
            Func<Double, Double> MyFunctionTest = Test();
            // invoke the delagate and pass to it 4 as input 
            Console.WriteLine(MyFunctionTest(4).ToString()); // 5
            Console.ReadLine();
            //------------
            // Steps of evalution 
            Data.Select(addOne).Select(square).Select(SubtractTen).ToList().ForEach((x) => Console.WriteLine(x.ToString()));
            Console.ReadLine();
            //------------
            Data.Select((x) => SubtractTen(square(addOne(x)))).ToList().ForEach((x) => Console.WriteLine(x.ToString()));
            Console.ReadLine();
            //------------
            Data.Select(MyComposedFunction).ToList().ForEach((x) => Console.WriteLine(x.ToString()));
            Console.ReadLine();
            //------------
            Data.Select(AddoneSquareSubtractTen()).ToList().ForEach((x) => Console.WriteLine(x.ToString()));
            Console.ReadLine();
            #endregion


        }

        public static double SubtractTen(double x)
        {
            return x - 10;
        }
        public static double square(double x)
        {
            return Math.Pow(x,2);
        }
        public static double addOne(double x)
        {
            return x + 1;
        }
        public static double Test1(double x)
        {
            return x / 2;
        }
        public static double Test2(double x)
        {
            return x / 4 + 1;
        }
        //------------------
        // Test3 take two parms : first one function (delegate) as param and second one double 
        public static double Test3(Func<double, double> F, double Value)
        {
            return F(Value) + Value;
        }
        // tgis function take function as parm and the function take int input and return tuple of x1,x2 , and take order that will calac on it the discount 
        public static double ClaculateDiscount(Func<int, (double x1, double x2)> ProductParamterCalc, Order Order)
        {
            (double x1, double x2) paramters = ProductParamterCalc(Order.ProductIndex);
            return paramters.x1 * Order.Quantity + paramters.x2 * Order.UnitPrice;
        }
        public static (double x1, double x2) ProductParamtersFood(int ProductIndex)
        {
            return (x1: ProductIndex / (double)(ProductIndex + 100), x2: ProductIndex / (double)(ProductIndex + 300));
        }
        public static (double x1, double x2) ProductParamtersBeverage(int ProductIndex)
        {
            return (x1: ProductIndex / (double)(ProductIndex + 300), x2: ProductIndex / (double)(ProductIndex + 400));
        }
        public static (double x1, double x2) ProductParamtersRawMaterial(int ProductIndex)
        {
            return (x1: ProductIndex / (double)(ProductIndex + 400), x2: ProductIndex / (double)(ProductIndex + 700));
        }
        //------------------
        public static Func<double, Double> Test()
        {
            //this is lamada expression function that take x and return x+1
            return x => x + 1;
        }

        public static Func<double, double> ComposeFunction(Func<double, double> F1, Func<double, double> F2, Func<double, double> F3)
        {
            return (x) =>
            {
                return F3(F2(F1(x)));
            };
        }
        public static Func<double, double> AddoneSquareSubtractTen()
        {
            Func<double, double> q1 = addOne;
            Func<double, double> q2 = square;
            Func<double, double> q3 = SubtractTen;

            return q1.Compose(q2).Compose(q3);
        }
        public static Func<T1, T3> Compose<T1, T2, T3>(this Func<T1, T2> f, Func<T2, T3> g)
        {
            return (x) => g(f(x));
        }

    }
}
