
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;
using static System.Tuple;
using static System.ValueTuple;

namespace HOF_example
{
    public class Program
    {
        private List<Order> OrdersforProcessing = new List<Order>();
        public class Order
        {
            public decimal Discount;
            public Order(Order orderdata)
            {

            }
        }

        public void Main()
        {
            //Example :
            //calculate Disount for list of orders .
            //Each order has only one product
            //There are several rule to calculate Discount 
            // an order should qualify to criteria in order for its associated rule to apply
            //several rules my qualify to the same order 
            //the discount is the ave of lowest of three discount
            //the system should allow adding others rules and qualifying criteria the future without much difficulty 
           
            var OrderswithDiscounts = OrdersforProcessing.Select((x) => GetOrderwithDiscount(x, GetDiscountRules()));
        }

        public Order GetOrderwithDiscount(Order R, List<(Func<Order, bool> QualifyingCondition, Func<Order, decimal> GetDiscount)> Rules)
        {
            var discount = Rules.Where((a) => a.QualifyingCondition(R)).Select((b) => b.GetDiscount(R)).OrderBy((x) => x).Take(3).Average();
            var neworder = new Order(R);
            neworder.Discount = discount;
            return neworder;
        }
        public List<(Func<Order, bool> QualifyingCondition, Func<Order, decimal> GetDiscount)> GetDiscountRules()
        {
            //List of tuple List<()>
            List<(Func<Order, bool> QualifyingCondition, Func<Order, decimal> GetDiscount)> DiscountRules
              = new List<(Func<Order, bool> QualifyingCondition, Func<Order, decimal> GetDiscount)>
            { ((isAQualified, A)),
              ((isBQualified, B)),
              ((isCQualified, C)) };

            return DiscountRules;
        }
        public bool isAQualified(Order r)
        {
            return true;
        }
        public decimal A(Order r)
        {

            return 1M;
        }
        public bool isBQualified(Order r)
        {
            return true;
        }
        public decimal B(Order r)
        {

            return 1M;
        }
        public bool isCQualified(Order r)
        {
            return true;
        }
        public decimal C(Order r)
        {

            return 1M;
        }
    }
}