using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using Iot.Device.SenseHat;

namespace Iot.Device.SenseHatText
{
    public class SenseHatText
    {
        private static readonly SenseHatText instance = new SenseHatText();
        private Dictionary<char, List<Color>> text_dict = new Dictionary<char, List<Color>>();

        static SenseHatText()
        {
        }

        private SenseHatText()
        {
            var assembly = typeof(SenseHatText).Assembly;
            var streamText = assembly.GetManifestResourceStream("SenseHatText.sense_hat_text.txt");
            if (streamText == null)
            {
                throw new Exception("Embedded text not found.");
            }
            string text_file;
            using (StreamReader reader = new StreamReader(streamText))
            {
                text_file = reader.ReadToEnd();
            }

            var streamPng = assembly.GetManifestResourceStream("SenseHatText.sense_hat_text.png");
            if (streamPng == null)
            {
                throw new Exception("Embedded PNG not found.");
            }
            using Bitmap bitmap = new Bitmap(streamPng);
            {
                // Get the color of a pixel within myBitmap.
                List<Color> listColor = new List<System.Drawing.Color>();

                for (int height = 0; height < 640; height++)
                {
                    for (int width = 0; width < 8; width++)
                    {
                        listColor.Add(bitmap.GetPixel(width, height));
                    }
                }

                int index = 0;
                foreach (char c in text_file)
                {
                    int start = index++ * 40;
                    List<Color> temp = new List<Color>();
                    text_dict.Add(c, listColor.GetRange(start, 40));
                }
            }
        }

        public static SenseHatText Instance
        {
            get { return instance; }
        }

        public void ShowMessage(SenseHatLedMatrix ledMatrix, string message, int scroll_speed = 90, Color? text_colour = null, Color? back_colour = null, bool isVertical = false)
        {
            text_colour = text_colour ?? Color.White;
            back_colour = back_colour ?? Color.Black;

            List<Color> pixel_list = new List<Color>();
            //String Padding
            for (int i = 0; i < 64; i++)
            {
                pixel_list.Add(Color.Black);
            }

            foreach (char c in message)
            {
                List<Color> temp_list = new List<Color>();
                for (int i = 0; i < 8; i++)
                {
                    temp_list.Add(Color.Black);
                }
                temp_list.AddRange(GetCharPixels(c.ToString()));

                if (!isVertical)
                {
                    pixel_list.AddRange(temp_list);
                }
                else
                {
                    for (int i = 0; i < 16; i++)
                    {
                        temp_list.Add(Color.Black);
                    }

                    //Rotate
                    for (int i = 7; i >= 0; i--)
                    {
                        for (int j = 7; j >= 0; j--)
                        {
                            pixel_list.Add(temp_list[(8 - 1 - j) * 8 + (i)]);
                        }
                    }
                }
            }

            //String Padding
            for (int i = 0; i < 72; i++)
            {
                pixel_list.Add(Color.Black);
            }

            List<Color> coloured_pixels = new List<Color>();
            foreach (var color in pixel_list)
            {
                if (color.R == 255 && color.G == 255 && color.B == 255)
                {
                    coloured_pixels.Add(text_colour.Value);
                }
                else
                {
                    coloured_pixels.Add(back_colour.Value);
                }
            }

            // Shift right/bottom by 8 pixels per frame to scroll
            var scroll_length = coloured_pixels.Count / 8;
            for (int i = 0; i < scroll_length - 8; i++)
            {
                if (!isVertical)
                {
                    int index = 0;
                    for (int x = 0; x < 8; x++)
                    {
                        for (int y = 7; y >= 0; y--)
                        {
                            ledMatrix.SetPixel(x, y, coloured_pixels[index++]);
                        }
                    }
                    coloured_pixels.RemoveRange(0, 8);
                }
                else
                {
                    int start = i * 8;
                    ledMatrix.Write(coloured_pixels.GetRange(start, 64).ToArray());
                }

                Thread.Sleep(scroll_speed);
            }
        }

        public void ShowLetter(SenseHatLedMatrix ledMatrix, string letter, Color? text_colour = null, Color? back_colour = null)
        {
            text_colour = text_colour ?? Color.White;
            back_colour = back_colour ?? Color.Black;

            List<Color> pixel_list = new List<Color>();
            for (int i = 0; i < 8; i++)
            {
                pixel_list.Add(Color.Black);
            }
            pixel_list.AddRange(GetCharPixels(letter));
            for (int i = 0; i < 16; i++)
            {
                pixel_list.Add(Color.Black);
            }

            //Rotate
            List<Color> new_list = new List<Color>();
            for (int i = 7; i >= 0; i--)
            {
                for (int j = 7; j >= 0; j--)
                {
                    new_list.Add(pixel_list[(8 - 1 - j) * 8 + (i)]);
                }
            }

            List<Color> coloured_pixels = new List<Color>();
            foreach (var color in new_list)
            {
                if (color.R == 255 && color.G == 255 && color.B == 255)
                {
                    coloured_pixels.Add(text_colour.Value);
                }
                else
                {
                    coloured_pixels.Add(back_colour.Value);
                }
            }

            ledMatrix.Write(coloured_pixels.ToArray());
        }

        public void Clear(SenseHatLedMatrix ledMatrix)
        {
            ledMatrix.Fill(Color.Black);
        }

        private List<Color> GetCharPixels(string letter)
        {
            if (letter.Length == 1 && text_dict.Keys.Contains(letter[0]))
            {
                return text_dict[letter[0]];
            }
            else
            {
                return text_dict['?'];
            }
        }
    }
}