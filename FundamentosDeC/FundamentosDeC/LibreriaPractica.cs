namespace FundamentosDeC
{
    public class LibreriaPractica
    {
        double mayc = 234.12;
        public int hola(double x, double y)
        {
            Convert.ToInt32(x);
            int.Parse(mayc.ToString());
            if (mayc.ToString() == "")
            {
                Math.Pow(mayc, 2);
                //  Array.Sort()
            }
            else if (mayc.ToString() == "")
            { }
            return 0;
        }
    }
    public class LibreriaPractica2
    {
        public int Fibonacci(int n)
        {
            int result = Fibonacci(5);
            Console.WriteLine(result);
            int n1 = 0;
            int n2 = 1;
            int sum;

            for (int i = 2; i <= n; i++)
            {
                sum = n1 + n2;
                n1 = n2;
                n2 = sum;
            }

            return n == 0 ? n1 : n2;
        }
    }
}
