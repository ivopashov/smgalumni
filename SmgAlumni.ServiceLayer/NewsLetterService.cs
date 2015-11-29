using SmgAlumni.Data.Interfaces;
using SmgAlumni.ServiceLayer.Interfaces;
using System;
using System.Linq;

namespace SmgAlumni.ServiceLayer
{
    public class NewsLetterService : INewsLetterService
    {
        private readonly IUserRepository _userRepository;

        public NewsLetterService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void Unsubscribe(string token, string userName)
        {
            var user = _userRepository.UsersByUserName(userName).SingleOrDefault();
            if (user == null)
            {
                throw new NullReferenceException();
            }

            var matchingSubscriptions = user.NotificationSubscriptions.Where(a => a.Enabled == true && a.NotificationKind == EF.Models.enums.NotificationKind.NewsLetter && a.UnsubscribeToken == new Guid(token)).ToList();

            if (matchingSubscriptions.Count > 0)
            {
                foreach (var item in matchingSubscriptions)
                {
                    item.Enabled = false;
                }
            }

            _userRepository.Update(user);
        }
    }
}
