using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using RazorEngine.Text;
using SmgAlumni.ServiceLayer.Interfaces;
using SmgAlumni.ServiceLayer.Models;
using SmgAlumni.Utils.Settings;
using System;
using System.IO;

namespace SmgAlumni.ServiceLayer
{
    public class NewsLetterGenerator : INewsLetterGenerator
    {
        private readonly IAppSettings _appSettings;

        public NewsLetterGenerator(IAppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public string GenerateNewsLetter(BiMonthlyNewsLetterDto newsLetterModel, string rootUrl)
        {
            var path = rootUrl + _appSettings.NewsLetterSettings.BiWeeklyNewsLetterTemplatePath;
            var template = File.ReadAllText(path,System.Text.Encoding.UTF8);

            var config = new TemplateServiceConfiguration();
            config.EncodedStringFactory = new RawStringFactory();
            var service = RazorEngineService.Create(config);
            Engine.Razor = service;

            var emailHtmlBody = Engine.Razor.RunCompile(template, Guid.NewGuid().ToString(),null, newsLetterModel);
            //TODO: to be deleted - only for testing purposes
            File.WriteAllText(@"C:\Users\ipashov\Desktop\razor.html", emailHtmlBody, System.Text.Encoding.UTF8);
            return emailHtmlBody;
        }
    }
}
