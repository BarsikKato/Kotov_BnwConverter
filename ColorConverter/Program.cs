using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorConverter
{
    class Program
    {
        static async void ConvertToBnw(Bitmap image, string fileName)
        {
            Bitmap convertedImage = new Bitmap(image.Width, image.Height);
            using (Graphics g = Graphics.FromImage(convertedImage))
            {
                ColorMatrix colorMatrix = new ColorMatrix(
                   new float[][]
                   {
                        new float[] {.5f, .5f, .5f, 0, 0},
                        new float[] {.5f, .5f, .5f, 0, 0},
                        new float[] {.5f, .5f, .5f, 0, 0},
                        new float[] {0, 0, 0, 1, 0},
                        new float[] {0, 0, 0, 0, 1}
                   });
                using (ImageAttributes attributes = new ImageAttributes())
                {
                    attributes.SetColorMatrix(colorMatrix);
                    g.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
                }
            }
            convertedImage.Save(@"Result\" + fileName);
            Console.WriteLine(fileName + " завершил конвертацию");
        }
        
        static void ConvertAndSave(string[] fileNames)
        {
            Task[] tasks = new Task[fileNames.Length];
            for(int i = 0; i < fileNames.Length; i++)
            {
                Bitmap image = new Bitmap(Image.FromFile(fileNames[i]));
                string fileName = Path.GetFileName(fileNames[i]);
                var task = Task.Run(() => ConvertToBnw(image, fileName));
                tasks[i] = task;
            }
            Task.WaitAll(tasks);
        }

        static void Main(string[] args)
        {
            string[] fileNames = Directory.GetFiles(@"Source\");    //Считывает из папки с билдом
            ConvertAndSave(fileNames);
            Process.Start(@"Result\");
        }
    }
}
