using SmgAlumni.App.Logging;
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
using SmgAlumni.EF.Models;
using System.Collections.Generic;

namespace SmgAlumni.App.Api
{
    public class FileController : BaseApiController
    {
        private readonly IAttachmentRepository _attachmentRepsoitory;
        private readonly List<string> allowedExtensions = new List<string>() { "doc", "docx", "xls", "xlsx", "pdf", "jpg", "jpeg", "bmp", "png", "txt" };

        public FileController(ILogger logger, IUserRepository userRepository, IAttachmentRepository attachmentRepository)
            : base(logger, userRepository)
        {
            _attachmentRepsoitory = attachmentRepository;
        }

        [HttpPost, Route("api/attachment")]
        public async Task<IHttpActionResult> Attachment()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return BadRequest("Невалиден опит да се качи файл");
            }

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);
            List<object> files = new List<object>();

            foreach (var file in provider.Contents)
            {
                var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                var extension = filename.Split(new char[] { '.' }).Last().Trim();
                if (!allowedExtensions.Contains(extension))
                {
                    return BadRequest("Неразрешено разширение на файл");
                }
                var buffer = await file.ReadAsByteArrayAsync();

                if (buffer.Length > 2100000)
                {
                    return BadRequest("Файлът не може да е по-голям от 2 МБ");
                }

                var user = _userRepository.UsersByUserName(User.Identity.Name).SingleOrDefault();
                var tempkey = Guid.NewGuid();

                var attachment = new Attachment()
                {
                    TempKey = tempkey,
                    CreatedOn = DateTime.Now,
                    Name = filename,
                    Data = buffer,
                    Size = buffer.Length
                };

                try
                {
                    _attachmentRepsoitory.Add(attachment);
                    files.Add(new { tempkey, name = filename });
                }
                catch (Exception e)
                {
                    _logger.Error(e.Message);
                    return BadRequest("Възникна грешка - моля опитайте отново");
                }
            }

            return Ok(files);
        }

        [AllowAnonymous]
        [HttpGet, Route("api/attachment")]
        public HttpResponseMessage DownloadAttachment([FromUri]Guid tempkey)
        {
            var attachment = _attachmentRepsoitory.FindByTempKey(tempkey);
            if (attachment == null)
            {
                var badresponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
                return badresponse;
            }

            var extension = attachment.Name.Split(new char[] { '.' }).Last().Trim();
            string contentType = string.Empty;
            switch (extension)
            {
                case "doc":
                    contentType = @"application/msword";
                    break;
                case "docx":
                    contentType = @"application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    break;
                case "xls":
                    contentType = @"application/vnd.ms-excel";
                    break;
                case "xlsx":
                    contentType = @"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;
                case "pdf":
                    contentType = @"application/pdf";
                    break;
                case "jpg":
                case "jpeg":
                    contentType = @"image/jpeg";
                    break;
                case "txt":
                    contentType = @"text/plain";
                    break;
                case "png":
                    contentType = @"image/png";
                    break;
                default:
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            MemoryStream ms;

            ms = new MemoryStream(attachment.Data);

            ms.Position = 0;
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new ByteArrayContent(attachment.Data);
            response.Content.Headers.ContentLength = ms.Length;
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = attachment.Name
            };
            return response;
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
            var user = _userRepository.UsersByUserName(HttpContext.Current.Server.HtmlEncode(username)).SingleOrDefault();
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
