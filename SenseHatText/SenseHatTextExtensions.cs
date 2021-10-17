using System.Drawing;
using Iot.Device.SenseHat;

namespace Iot.Device.SenseHatText
{
    public static class SenseHatTextExtensions
    {
        public static void ShowMessage(this SenseHatLedMatrix ledMatrix, string message, int scroll_speed = 90, Color? text_colour = null, Color? back_colour = null, bool isVertical = false)
        {
            SenseHatText.Instance.ShowMessage(ledMatrix, message, scroll_speed, text_colour, back_colour, isVertical);
        }

        public static void ShowLetter(this SenseHatLedMatrix ledMatrix, string letter, Color? text_colour = null, Color? back_colour = null)
        {
            SenseHatText.Instance.ShowLetter(ledMatrix, letter, text_colour, back_colour);
        }

        public static void Clear(this SenseHatLedMatrix ledMatrix)
        {
            SenseHatText.Instance.Clear(ledMatrix);
        }
    }
}