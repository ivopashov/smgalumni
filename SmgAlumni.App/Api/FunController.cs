using System.Collections.Generic;
using System.Web.Http;

namespace SmgAlumni.App.Api
{
    public class FunController : ApiController
    {
        private Dictionary<char, string> dict; 
        public FunController()
        {
            dict=new Dictionary<char, string>();
            dict.Add('а', "&#945;");
            dict.Add('б', "&#948;");
            dict.Add('в', "&#946;");
            dict.Add('г', "&#967;");
            dict.Add('д', "&#948;");
            dict.Add('е', "&#949;");
            dict.Add('ж', "&#968;");
            dict.Add('з', "&#950;");
            dict.Add('и', "&#953;");
            dict.Add('й', "&#977;");
            dict.Add('к', "&#954;");
            dict.Add('л', "&#955;");
            dict.Add('м', "&#956;");
            dict.Add('н', "&#951;");
            dict.Add('о', "&#959;");
            dict.Add('п', "&#960;");
            dict.Add('р', "&#961;");
            dict.Add('с', "&#962;");
            dict.Add('т', "&#964;");
            dict.Add('у', "&#965;");
            dict.Add('ф', "&#968;");
            dict.Add('х', "&#967;");
            dict.Add('ц', "&#964;&#962;");
            dict.Add('ч', "&#950;");
            dict.Add('ш', "&#969;");
            dict.Add('щ', "&#969;&#964;");
            dict.Add('ю', "&#978;&#965;");
            dict.Add('я', "&#966;");
            dict.Add(' ', "&nbsp;");
            dict.Add(',', "&#44;");
            dict.Add('.', "&#46;");
            dict.Add('?', "&#63;");
            dict.Add('!', "&#33;");
            dict.Add(':', "&#58;");
            dict.Add(';', "&#59;");
            dict.Add('А', "&#913;");
            dict.Add('Б', "&#914;");
            dict.Add('В', "&#914;");
            dict.Add('Г', "&#915;");
            dict.Add('Д', "&#916;");
            dict.Add('Е', "&#917;");
            dict.Add('Ж', "&#936;");
            dict.Add('З', "&#918;");
            dict.Add('И', "&#921;");
            dict.Add('Й', "&#1031;");
            dict.Add('К', "&#922;");
            dict.Add('Л', "&#923;");
            dict.Add('М', "&#924;");
            dict.Add('Н', "&#919;");
            dict.Add('О', "&#920;");
            dict.Add('П', "&#928;");
            dict.Add('Р', "&#929;");
            dict.Add('С', "&#931;");
            dict.Add('Т', "&#932;");
            dict.Add('У', "&#933;");
            dict.Add('Ф', "&#934;");
            dict.Add('Х', "&#935;");
            dict.Add('Ц', "&#932;&#931;");
            dict.Add('Ч', "&#950;");
            dict.Add('Ш', "&#936;");
            dict.Add('Щ', "&#931;&#936;");
            dict.Add('Ю', "&#921;&#933;");
            dict.Add('Я', "&#921;&#913;");
        }
        [Route("api/fun/bgtogreek")]
        [HttpGet]
        public IHttpActionResult ConvertFromBgToGreek(string text)
        {
            var outStr = string.Empty;
            for (int i = 0; i < text.Length; i++)
            {
                if (dict.ContainsKey(text[i]))
                {
                    outStr += dict[text[i]];
                }
               
            }

            return Ok(outStr);
        }
    }
}
