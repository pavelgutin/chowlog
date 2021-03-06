﻿using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Chowlog.Web.App_Code
{
    public class AmazonFileUploadService : IFileUploadService
    {
        public string UploadFile(HttpPostedFileBase file, string fileName)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    //fileName += Path.GetExtension(file.FileName);
                    if (Secret.AWSSecretKey == "" || Secret.AWSAccessKey == "")
                    {
                        throw (new Exception("AWS Keys not specified. Did you forget to edit Secret.cs?"));
                    }
                    using (var client = Amazon.AWSClientFactory.CreateAmazonS3Client(Secret.AWSAccessKey, Secret.AWSSecretKey, Amazon.RegionEndpoint.USEast1))
                    {
                        PutObjectRequest request = new PutObjectRequest();

                        request.BucketName = ConfigurationManager.AppSettings["bucketname"];
                        request.CannedACL = S3CannedACL.PublicRead;
                        request.Key = fileName;

                        request.InputStream = file.InputStream;

                        var response = client.PutObject(request);
                    }
                }
                return fileName;
            }
            catch (Exception e)
            {
                //TempData["Result"] = "Error!" + e.Message;
                throw new Exception("File Upload Failed! " + e.Message, e);
            }
            return "";
        }


        public string UploadFile(byte[] file, string fileName)
        {
            throw new NotImplementedException();
        }
    }
}