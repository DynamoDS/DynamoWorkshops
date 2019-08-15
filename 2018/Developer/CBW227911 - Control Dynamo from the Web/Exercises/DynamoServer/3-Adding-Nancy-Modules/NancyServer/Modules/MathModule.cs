using Nancy;
using System;

namespace NancyServer
{
    public class MathModule : NancyModule
    {
        public MathModule() : base("/math")
        {
            Get["/pi"] = x =>
            {
                return "Hello back, PI is " + Math.PI;
            };

            Get["/echo/{number}"] = EchoNumber;
            Get["/square/{number}"] = SquareNumber;
            Get["/root/{number}"] = RootNumber;

        }

        private dynamic EchoNumber(dynamic parameters)
        {
            return "You gave me this number :" + parameters.number;
        }

        private dynamic SquareNumber(dynamic parameters)
        {
            double n;
            try
            {
                if (!double.TryParse(parameters.number, out n)) throw new Exception();
            }
            catch (Exception)
            {
                return "Could not parse number";
            }
            return "You gave me this number :" + parameters.number + ", its square is : " + Math.Pow(n, 2).ToString();
        }

        private dynamic RootNumber(dynamic parameters)
        {
            double n;
            try
            {
                if (!double.TryParse(parameters.number, out n)) throw new Exception();
            }
            catch (Exception)
            {
                return "Could not parse number";
            }
            return "You gave me this number :" + parameters.number + ", its square root is : " + Math.Sqrt(n).ToString();
        }
    }
}
