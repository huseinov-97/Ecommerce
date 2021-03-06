﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace And.ECommerce.UI.WEB.Helpers
{
    public static class ImageHelper
    {
        static string Name(HttpPostedFileBase file)
        {
            var guid = Guid.NewGuid().ToString();
            var extension = Path.GetExtension(file.FileName);
            var fileName = Path.GetFileNameWithoutExtension(file.FileName);
            return $"{guid}_{DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss")}_{fileName}{extension}";
        }

        public static void Save<T>(T obj, HttpPostedFileBase file, string contentType = null)
        {
            var name = Name(file);
            string folderName = "ProductImages";

            if (contentType != null)
            {
                switch (contentType)
                {
                    case "UserAvatars": folderName = "UserAvatars"; break;
                }
            }

            string path = Path.Combine(HttpContext.Current.Server.MapPath($"~/Documents/{folderName}/"), name);
            obj.GetType().GetProperty("ImageUrl").SetValue(obj, name);
            file.SaveAs(path);
        }
    }
}