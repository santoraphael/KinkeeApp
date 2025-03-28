using ImageResizer;
using System;
using System.Linq;
using System.Web;

namespace Plataforma.Helper
{
    public static class CommonHelper
    {
        public static string SalvarImagemChat(HttpPostedFileBase file, string UserName)
        {
            string path = "";

            if (file != null && file.ContentLength > 0)
                try
                {
                    if (IsImage(file))
                    {
                        var instructions = new Instructions
                        {
                            //Width = 800,
                            //Height = 600,
                            Mode = FitMode.Max,
                            Format = "jpg",
                            JpegQuality = 60,
                        };

                        var i = new ImageJob(file, "~/Chats/" + UserName + "/<guid>_<guid>",
                            instructions, false, true);
                        i.CreateParentDirectory = true;
                        i.Build();

                        path = ImageResizer.Util.PathUtils.GuessVirtualPath(i.FinalPath);
                    }
                }
                catch (Exception ex)
                {
                    var t = ex.Message;
                }
            else
            {

            }


            return path;
        }

        private static bool IsImage(HttpPostedFileBase file)
        {
            if (file.ContentType.Contains("image"))
            {
                return true;
            }

            string[] formats = new string[] { ".jpg", ".png", ".gif", ".jpeg" }; // add more if u like...

            // linq from Henrik Stenbæk
            return formats.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
        }


        public static int CalculateAge(DateTime dateOfBirth)
        {
            int age = 0;
            age = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
                age = age - 1;

            return age;
        }
    }
}