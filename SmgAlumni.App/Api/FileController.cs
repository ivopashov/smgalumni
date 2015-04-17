using NLog;
using SmgAlumni.Data.Repositories;
using SmgAlumni.Utils.Membership;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SmgAlumni.App.Api
{
    public class FileController : BaseApiController
    {
        private readonly UserRepository _userRepository;

        public FileController(Logger logger, UserRepository userRepository)
            : base(logger)
        {
            _userRepository = userRepository;
            VerifyNotNull(_userRepository);
        }

        [HttpPost, Route("api/file/upload")]
        public async Task<IHttpActionResult> Upload()
        {
            if (!Request.Content.IsMimeMultipartContent())
                return BadRequest("Невалиден опит да се качи снимка");

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);

            foreach (var file in provider.Contents)
            {
                var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                var buffer = await file.ReadAsByteArrayAsync();
                var user = _userRepository.Find(a => a.UserName.Equals(User.Identity.Name)).SingleOrDefault();
                if (user == null) return BadRequest("Възникна грешка - моля опитайте отново");
                user.AvatarImage =ResizeImage(buffer);
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
        [HttpGet, Route("api/file/download")]
        public HttpResponseMessage DownloadFile([FromUri]string username)
        {
            var user = _userRepository.Find(a => a.UserName.Equals(username)).SingleOrDefault();
            if (user == null)
            {
                var badresponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
                return badresponse;
            }

            var ms = new MemoryStream(user.AvatarImage);
            ms.Position = 0;
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(ms);
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
            response.Content.Headers.ContentLength = ms.Length;
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
                    double widthProportion = image.Width/ 150.0D;
                    int wantedHeightSoThatPorportionIsKept = Convert.ToInt32(Convert.ToDouble(image.Height) / widthProportion);
                    image = new Bitmap(image, new Size(150, wantedHeightSoThatPorportionIsKept));
                }
            }

            var outms = new MemoryStream();
            try
            {
                image.Save(outms, System.Drawing.Imaging.ImageFormat.Png);
                return outms.ToArray();
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }
    }
}
