using System;
using RestSharp;
using SmgAlumni.ServiceLayer.Interfaces;
using RestSharp.Authenticators;
using SmgAlumni.Utils.Settings;

namespace SmgAlumni.ServiceLayer
{
    public class RequestSender : IRequestSender
    {
        private RestClient client;
        private RestRequest request;
        private readonly IAppSettings _appSettings;

        public RequestSender(IAppSettings appSettings)
        {
            client = new RestClient();
            request = new RestRequest();
            _appSettings = appSettings;
        }

        public IRequestSender AddParameter(string key, object param)
        {
            request.AddParameter(key, param);
            return this;
        }

        public IRequestSender AddParameter(string key, object param, ParameterType paramType)
        {
            request.AddParameter(key, param, paramType);
            return this;
        }

        public IRestResponse Execute()
        {
            return client.Execute(request);
        }

        public IRequestSender InitializeClient()
        {
            client.BaseUrl = new Uri(_appSettings.MailgunSettings.BaseUrl);
            client.Authenticator =
                    new HttpBasicAuthenticator("api",
                                               _appSettings.MailgunSettings.ApiKey);
            return this;
        }

        public IRequestSender SetMethod(Method method)
        {
            request.Method = method;
            return this;
        }

        public IRequestSender SetResource(string resource)
        {
            request.Resource = resource;
            return this;
        }
    }
}
