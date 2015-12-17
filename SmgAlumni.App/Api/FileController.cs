﻿using SmgAlumni.App.Logging;
using SmgAlumni.Data.Interfaces;
using SmgAlumni.Utils;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web;

namespace SmgAlumni.App.Api
{
    public class FileController : BaseApiController
    {
        public FileController(ILogger logger, IUserRepository userRepository)
            : base(logger, userRepository)
        {
        }

        [HttpPost, Route("api/file/avatar")]
        public async Task<IHttpActionResult> Avatar()
        {
            if (!Request.Content.IsMimeMultipartContent())
                return BadRequest("Невалиден опит да се качи снимка");

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);

            foreach (var file in provider.Contents)
            {
                var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                var buffer = await file.ReadAsByteArrayAsync();
                var user = _userRepository.UsersByUserName(User.Identity.Name).SingleOrDefault();
                if (user == null)
                {
                    return BadRequest("Възникна грешка - моля опитайте отново");
                }

                user.AvatarImage = ResizeImage(buffer);
                try
                {
                    _userRepository.Update(user);
                    return Ok();
                }
                catch (Exception e)
                {
                    _logger.Error(e.Message);
                    return BadRequest("Възникна грешка - моля опитайте отново");
                }
            }

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet, Route("api/file/avatar")]
        public HttpResponseMessage Avatar([FromUri]string username)
        {
            var user = _userRepository.UsersByUserName(username).SingleOrDefault();
            if (user == null)
            {
                var badresponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
                return badresponse;
            }

            MemoryStream ms;

            if (user.AvatarImage == null)
            {
                ms = new MemoryStream(File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/Content/images/anonymoususer.png")));
            }
            else
            {
                ms = new MemoryStream(user.AvatarImage);
            }

            ms.Position = 0;
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(ms);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            response.Content.Headers.ContentLength = ms.Length;
            response.Headers.CacheControl = new CacheControlHeaderValue()
            {
                Public = false,
                NoStore = true,
                
            };
            return response;
        }

        private byte[] ResizeImage(byte[] arr)
        {
            var ms = new MemoryStream(arr);
            var image = Image.FromStream(ms);
            if (image.Height > 150 || image.Width > 150)
            {
                if (image.Height > 150)
                {
                    //actual versus wanted
                    double heightProportion = image.Height / 150.0D;
                    int wantedWidthSoThatPorportionIsKept = Convert.ToInt32(Convert.ToDouble(image.Width) / heightProportion);
                    image = new Bitmap(image, new Size(wantedWidthSoThatPorportionIsKept, 150));
                }
                if (image.Width > 150)
                {
                    double widthProportion = image.Width / 150.0D;
                    int wantedHeightSoThatPorportionIsKept = Convert.ToInt32(Convert.ToDouble(image.Height) / widthProportion);
                    image = new Bitmap(image, new Size(150, wantedHeightSoThatPorportionIsKept));
                }
            }

            var outms = new MemoryStream();
            try
            {
                image.Save(outms, ImageFormat.Png);
                return outms.ToArray();
            }
            catch (Exception e)
            {
                throw e;
            }

        }
    }
}
