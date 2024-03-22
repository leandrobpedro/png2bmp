using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace png2bmp
{
    public class Program
    {
        public static int Main(string[] args)
               => CommandLineApplication.Execute<Program>(args);

        [Required]
        [Argument(0, Description = "File path.")]
        public string Files { get; set; }

        [Argument(1, Description = "Color to find in hex format, e.g.: #ff00ff.")]
        [IsColor]
        public string FromColor { get; set; }

        [Argument(2, Description = "Color to replace in hex format, e.g.: #ff00ff.")]
        [IsColor]
        public string ToColor { get; set; }

        public static string helpText = "";

        public void OnExecute(CommandLineApplication app)
        {
            helpText = app.GetHelpText();

            string path = Files;
            string filter;
            List<string> files = new List<string>();

            if (!string.IsNullOrEmpty(FromColor) && string.IsNullOrEmpty(ToColor))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("When FromColor is passed as an argument, ToColor is mandatory");
                Console.ResetColor();
                app.ShowHint();
                return;
            }

            // List files.
            if (path.Contains("*"))
            {
                if (path.Equals("*"))
                {
                    filter = "*";
                    path = Directory.GetCurrentDirectory().ToString();
                }
                else
                {
                    if (path.Contains('\\'))
                    {
                        filter = path.Substring(path.LastIndexOf('\\') + 1);
                        path = path.Remove(path.LastIndexOf('\\'));
                    }
                    else
                    {
                        filter = path;  // "*"
                        path = Directory.GetCurrentDirectory().ToString();
                    }
                }

                if (Directory.Exists(path))
                {
                    foreach (var file in Directory.GetFiles(path, filter))
                    {
                        files.Add(file);
                    }
                }
            }
            else if (File.Exists(path))
            {
                files.Add(path);
            }

            // 
            foreach (var s in files.Where(s => Regex.IsMatch(s, ".*\\.png$",
                    RegexOptions.IgnoreCase)))
            {
                try
                {
                    Image img = Image.FromFile(s);
                    img = CreateBmp(img, FromColor, ToColor);

                    // Save bmp.
                    img.Save(Path.ChangeExtension(s, ".bmp"), ImageFormat.Bmp);
                    Console.WriteLine("'" + Path.ChangeExtension(s, ".bmp") + "' done!");
                }
                catch (Exception)
                {
                    Console.Error.WriteLine("Error processing file: '" + s + "'");
                }
            }
        }
        public static Bitmap CreateBmp(Image img
                                    , string fromColor
                                    , string toColor)
        {
            Bitmap bmp = new Bitmap(img);

            if (!string.IsNullOrEmpty(fromColor) && !string.IsNullOrEmpty(toColor))
            {
                Color f = ColorTranslator.FromHtml(fromColor);  // find
                Color r = ColorTranslator.FromHtml(toColor);    // replace
                bmp = ChangeColor(bmp, f, r);
            }

            return bmp;
        }

        public static Bitmap ChangeColor(Bitmap image
                                        , Color fromColor
                                        , Color toColor)
        {
            ImageAttributes attributes = new ImageAttributes();
            attributes.SetRemapTable(new ColorMap[]
            {
                new ColorMap()
                {
                    OldColor = fromColor,
                    NewColor = toColor,
                }
            }, ColorAdjustType.Bitmap);

            using (Graphics g = Graphics.FromImage(image))
            {
                g.DrawImage(
                    image,
                    new Rectangle(Point.Empty, image.Size),
                    0, 0, image.Width, image.Height,
                    GraphicsUnit.Pixel,
                    attributes);
            }

            return image;
        }
    }

    class IsColor : ValidationAttribute
    {
        public IsColor()
            : base("The value for '{0}' must be a valid color hex, e.g.: #ff0000")
        {
        }

        protected override ValidationResult IsValid(object value,
                                                    ValidationContext context)
        {
            if (value is string && !Regex.IsMatch(value as string,
                    @"^#([0-9A-F]{3}){1,2}$", RegexOptions.IgnoreCase))
            {
                return new ValidationResult(FormatErrorMessage(context.DisplayName));
            }

            return ValidationResult.Success;
        }
    }
}