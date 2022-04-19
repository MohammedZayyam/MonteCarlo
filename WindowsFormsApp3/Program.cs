using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
    abstract class Option
    {
        public double U { get; set; }
        public double T { get; set; }
        public double V { get; set; }
        public double D { get; set; }
        public double R { get; set; }
        public double S { get; set; }
        public int Trials { get; set; }
        public int Steps { get; set; }
        public String CorP { get; set; }
        public double[,] matrix1 { get; set; }
    }
    class European_Option : Option
    {

        public double Option_Price { get => GetOptionPrice(CorP, S, V, D, T, Trials, Steps, U, R, matrix1); set => Option_Price = value; }
        public double Delta { get => GetDelta(CorP, S, V, D, T, Trials, Steps, U, R, matrix1); set => Delta = value; }
        public double Gamma { get => GetGamma(CorP, S, V, D, T, Trials, Steps, U, R, matrix1); set => Gamma = value; }
        public double Vega { get => GetVega(CorP, S, V, D, T, Trials, Steps, U, R, matrix1); set => Vega = value; }
        public double Theta { get => GetTheta(CorP, S, V, D, T, Trials, Steps, U, R, matrix1); set => Theta = value; }
        public double Rho { get => GetRho(CorP, S, V, D, T, Trials, Steps, U, R, matrix1); set => Rho = value; }
        public double Standard_Deviation { get => GetStandard_deviation(CorP, V, D, T, Steps, U, Option_Price, matrix1, Trials); set => Standard_Deviation = value; }



        private double[,] GenerateSimulations(double V, double D, double T, int Trials, int Steps, double U, double[,] matrix1)
        {
            //Console.WriteLine("V{0}, D{1}, T{2}, Trials{3}, Steps{4}, U{5}", V, D, T, Trials, Steps, U);
            //Console.ReadLine();
            double[,] matrix2 = new double[Trials, Steps];
            for (int i = 0; i < Trials; i++)
            {
                matrix2[i, 0] = U;
                //Console.WriteLine("{0},{1}",i,matrix2[i, 0]);

            }
            double deltaT = (T / Convert.ToDouble(Steps));
            //Console.WriteLine("DeltaT {0}", deltaT);
            for (int i = 0; i < Trials; i++)
                for (int j = 0; j < Steps - 1; j++)
                {
                    matrix2[i, j + 1] = matrix2[i, j] * Math.Exp(((D - ((V * V) / 2)) * deltaT) + (V * Math.Sqrt(deltaT) * matrix1[i, j + 1]));
                    //Console.WriteLine("{0},{1}:{2},RandomNumber:{3} ", i, j, matrix2[i, j + 1], matrix1[i, j + 1]);
                }
            Console.ReadLine();
            //matrix3 = matrix2;
            return matrix2;
        }
        private double GetOptionPrice(String C, double S, double V, double D, double T, int Trials, int Steps, double U, double R, double[,] matrix1)
        {

            double[,] matrix2 = GenerateSimulations(V, D, T, Trials, Steps, U, matrix1);
            double temp = 0;
            if (CorP == "C")
            {
                for (int z = 0; z < Trials; z++)
                {
                    matrix2[z, Steps - 1] = Math.Max(matrix2[z, Steps - 1] - S, 0);
                    temp = matrix2[z, Steps - 1] + temp;
                    //Console.WriteLine(temp);
                }
            }
            else
            {
                for (int z = 0; z < Trials; z++)
                {
                    matrix2[z, Steps - 1] = Math.Max(S - matrix2[z, Steps - 1], 0);
                    temp = matrix2[z, Steps - 1] + temp;
                }
            }
            //Console.WriteLine("temp{0} -R{1} T{2}", temp, -R, T);
            //Console.ReadLine();

            return temp / Trials * Math.Exp(-R * T);
        }
        private double GetDelta(string CorP, double S, double V, double D, double T, int Trials, int Steps, double U, double R, double[,] matrix1)
        {
            double deltaS = U * 0.001;

            return (GetOptionPrice(CorP, S, V, D, T, Trials, Steps, U + deltaS, R, matrix1) - GetOptionPrice(CorP, S, V, D, T, Trials, Steps, U - deltaS, R, matrix1)) / (2 * deltaS);
        }
        private double GetGamma(string CorP, double S, double V, double D, double T, int Trials, int Steps, double U, double R, double[,] matrix1)
        {
            double deltaS = U * 0.001;
            return (GetOptionPrice(CorP, S, V, D, T, Trials, Steps, U + deltaS, R, matrix1) - 2 * GetOptionPrice(CorP, S, V, D, T, Trials, Steps, U, R, matrix1) + GetOptionPrice(CorP, S, V, D, T, Trials, Steps, U - deltaS, R, matrix1)) / (deltaS * deltaS);
        }
        private double GetVega(string CorP, double S, double V, double D, double T, int Trials, int Steps, double U, double R, double[,] matrix1)
        {
            double deltaV = V * 0.001;
            return (GetOptionPrice(CorP, S, V + deltaV, D, T, Trials, Steps, U, R, matrix1) - GetOptionPrice(CorP, S, V - deltaV, D, T, Trials, Steps, U, R, matrix1)) / (2 * deltaV);
        }
        private double GetTheta(string CorP, double S, double V, double D, double T, int Trials, int Steps, double U, double R, double[,] matrix1)
        {
            double deltaT = T * 0.001;
            return -(GetOptionPrice(CorP, S, V, D, T + deltaT, Trials, Steps, U, R, matrix1) - GetOptionPrice(CorP, S, V, D, T, Trials, Steps, U, R, matrix1)) / (deltaT);
        }
        private double GetRho(string CorP, double S, double V, double D, double T, int Trials, int Steps, double U, double R, double[,] matrix1)
        {
            double deltaR = R * 0.001;
            return (GetOptionPrice(CorP, S, V, D+ deltaR, T, Trials, Steps, U, R + deltaR, matrix1) - GetOptionPrice(CorP, S, V, D-deltaR, T, Trials, Steps, U, R - deltaR, matrix1)) / (deltaR * 2);
        }
        private double GetStandard_deviation(String CorP, double V, double D, double T, int Steps, double U, double option, double[,] matrix2, int trials)
        {
            //Console.WriteLine("CorP{0}, V{1}, D{2},T{3}, Steps{4}, U{5}, option{6}, trials{7}", CorP, V, D, T, Steps, U, option, trials);
            double[,] matrix = GenerateSimulations(V, D, T, trials, Steps, U, matrix2);
            double[] intrinsic = new double[trials];
            double temp = 0;
            if (CorP == "C")
            {
                for (int i = 0; i < trials; i++)
                {
                    intrinsic[i] = Math.Max(matrix[i, Steps - 1] - S, 0);
                    temp += Math.Pow((intrinsic[i] - option), 2);
                    //Console.WriteLine("Yes");


                }
            }
            else
            {
                for (int i = 0; i < trials; i++)
                {
                    intrinsic[i] = Math.Max(S - matrix[i, Steps - 1], 0);
                    temp += Math.Pow((intrinsic[i] - option), 2);
                    //Console.WriteLine(temp);

                }
            }

            double x = ((Math.Sqrt(temp / (trials - 1)))/Math.Sqrt(trials));
            //Console.WriteLine("temp{0} Trials{1} x{2}",temp, trials, x);
            //Console.WriteLine(x);
            Console.ReadLine();
            return x;
        }

    }

    public static class RandomNumberGenerator
    {


        //Polar rejection formula
        public static double[,] NormalRandomNumbers(int Trials, int Steps)
        {
            double[,] matrix = new double[Trials, Steps];
            Random random1 = new Random();
            for (int i = 0; i < Trials; i++)
                for (int j = 0; j < Steps - 1; j += 2)
                {
                    double x1, x2;
                    x1 = random1.NextDouble();
                    x2 = random1.NextDouble();
                    //Console.WriteLine("Uniform Random Number {0} |{1}   ", x1, x2);
                    matrix[i, j] = (Math.Sqrt(-2 * Math.Log(x1))) * Math.Cos(2 * Math.PI * x2);
                    matrix[i, j + 1] = (Math.Sqrt(-2 * Math.Log(x1))) * Math.Sin(2 * Math.PI * x2);

                    //Console.WriteLine("{0},{1}:{2}", i, j, matrix[i, j]);
                    //Console.WriteLine("{0},{1}:{2}", i, j+1, matrix[i, j + 1]);
                }
            //Console.ReadLine();
            return matrix;
        }
    }
}
