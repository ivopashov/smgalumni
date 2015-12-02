using RestSharp;

namespace SmgAlumni.ServiceLayer.Interfaces
{
    public interface IRequestSender
    {
        IRequestSender InitializeClient();
        IRequestSender AddParameter(string key, object param);
        IRequestSender AddParameter(string key, object param, ParameterType paramType);
        IRestResponse Execute();
        IRequestSender SetResource(string resource);
        IRequestSender SetMethod(Method method);
    }
}
