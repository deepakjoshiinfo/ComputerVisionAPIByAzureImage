using ImageUpload.Utility;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace ImageUpload.Controllers
{
    public class SoftwarePackageModel
    {

    }
    public class UploadController : ApiController
    {
        [Route("user/PostUserImage")]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> PostUserImage()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {

                var httpRequest = HttpContext.Current.Request;

                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {

                        int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB  

                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {

                            var message = string.Format("Please Upload image of type .jpg,.gif,.png.");

                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {

                            var message = string.Format("Please Upload a file upto 1 mb.");

                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else
                        {



                            var filePath = HttpContext.Current.Server.MapPath("~/Userimage/" + postedFile.FileName + extension);

                            postedFile.SaveAs(filePath);

                        }
                    }

                    var message1 = string.Format("Image Updated Successfully.");
                    return Request.CreateErrorResponse(HttpStatusCode.Created, message1); ;
                }
                var res = string.Format("Please Upload a image.");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
            catch (Exception ex)
            {
                var res = string.Format("some Message");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
        }

        [HttpPost]
        [ResponseType(typeof(string))]
        [SwaggerParameter("myFile", "A file", Required = true, Type = "file")]
        public async Task<IHttpActionResult> Upload(int Id)
        {
            //URL: http://tahirhassan.blogspot.in/2017/12/web-apiswagger-file-upload.html
           
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = await Request.Content.ReadAsMultipartAsync();
            var bytes = await provider.Contents.First().ReadAsByteArrayAsync();


            //Storage account: deeppawncloudcustomer
            //Container: customer
            //Install package: Install-Package WindowsAzure.Storage -Version 9.1.0


            // Create storagecredentials object by reading the values from the configuration (appsettings.json)
            //deeppawncloudcustomer - Access keys
            StorageCredentials storageCredentials = new StorageCredentials("deeppawncloudcustomer", "r0OatxcT/TZ4AMOFgjZR81oQb4Jt1vBUNupANKXnwP9PA7SNGnvylxF36BsDdIOm0lUx+OKZahEfO17UIJnDFQ==");

            // Create cloudstorage account by passing the storagecredentials
            CloudStorageAccount storageAccount = new CloudStorageAccount(storageCredentials, true);

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Get reference to the blob container by passing the name by reading the value from the configuration (appsettings.json)
            CloudBlobContainer container = blobClient.GetContainerReference("customer");

            // Get the reference to the block blob from the container
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(Id+".jpg");

            // Upload the file
            blockBlob.UploadFromByteArray(bytes, 0, bytes.Length);

            //// Your existing code for azure blob access
            //CloudStorageAccount storageAccount = CloudStorageAccount.Parse("deeppawncloudcustomer");//https://deeppawncloudcustomer.blob.core.windows.net/
            //// Create the blob client.
            //CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            //// Retrieve a reference to a container.
            ////specified container name
            //CloudBlobContainer container = blobClient.GetContainerReference("customer");
            //container.CreateIfNotExists();
            //// Retrieve reference to a blob named "myblob".
            //CloudBlockBlob blockBlob = container.GetBlockBlobReference("myblob");
            //blockBlob.UploadFromByteArray(bytes,0,bytes.Length);
            ////container.UploadFromStream
            return Ok();
        }
    }
}
