using System;
using System.Drawing;
using System.Threading;
using Iot.Device.CpuTemperature;
using Iot.Device.SenseHat;
using Iot.Device.SenseHatText;

namespace Sample
{
    public class Sample
    {
        static void Main(string[] args)
        {
            using var ledMatrix = new SenseHatLedMatrixSysFs();
            Console.WriteLine("Please watch your Sense Hat now.");
            ledMatrix.ShowMessage("WELCOME TO C#!", 90, Color.DarkBlue, Color.DarkRed);
            ledMatrix.ShowMessage("WELCOME TO .NET!", 90, Color.Yellow, Color.Black, true);

            ledMatrix.ShowLetter("@", Color.Green);
            Thread.Sleep(2500);
            for (int i = 0; i < 3; i++)
            {
                ledMatrix.ShowLetter("P", Color.Green, Color.DarkRed);
                Thread.Sleep(1000);
                ledMatrix.ShowLetter("I", Color.Green, Color.DarkRed);
                Thread.Sleep(1000);
            }
            ledMatrix.Clear();

            using var sh = new SenseHat();
            ledMatrix.ShowMessage($"Temperature now is { ((sh.Temperature.DegreesCelsius + sh.Temperature2.DegreesCelsius) / 2).ToString("n1") } C", 90, Color.Blue, Color.Black);
            ledMatrix.Clear();

            ledMatrix.SetPixel(2, 2, Color.Blue);
            ledMatrix.SetPixel(4, 2, Color.Blue);
            ledMatrix.SetPixel(3, 4, Color.White);
            ledMatrix.SetPixel(1, 5, Color.Red);
            ledMatrix.SetPixel(2, 6, Color.Red);
            ledMatrix.SetPixel(3, 6, Color.Red);
            ledMatrix.SetPixel(4, 6, Color.Red);
            ledMatrix.SetPixel(5, 5, Color.Red);

            for (int i = 0; i < 6; i++)
            {
                Thread.Sleep(750);
                if (i % 2 == 0)
                {
                    ledMatrix.SetPixel(2, 2, Color.Black);
                }
                else
                {
                    ledMatrix.SetPixel(2, 2, Color.Blue);
                }
            }

            Thread.Sleep(3000);
            ledMatrix.Clear();
            Console.WriteLine("End!");
        }
    }
}
