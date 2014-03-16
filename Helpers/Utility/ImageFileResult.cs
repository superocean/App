﻿using System.Web;
using System.Web.Mvc;

namespace DealSearch.Helpers.Resources
{
    public class ImageFileResult : FilePathResult
    {
        public ImageFileResult(string fileName) :
            base(fileName, string.Format("image/{0}", fileName.FileExtensionForContentType()))
        {
        }

        protected override void WriteFile(HttpResponseBase response)
        {
            response.SetDefaultImageHeaders();
            base.WriteFile(response);
        }
    }
}