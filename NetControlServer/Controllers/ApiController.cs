using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;
using NetControlCommon.Interfaces;
using NetControlCommon.StandartResponses;
using NetControlCommon.Utils;
using NetControlServer.Classes;
using NetControlServer.Responses;

namespace NetControlServer.Controllers
{
    public class ApiController : IController
    {
        public IRequestResponse Version()
        {
            return new StringResponse("1.0");
        }

        public IRequestResponse PrtSc()
        {
            return new PngResponse(ScreenCapturer.Take());
        }

        public IRequestResponse PrtSc(string size)
        {
            if (Size.TryParse(size, out var s))
                return new PngResponse(ScreenCapturer.Take(s.w, s.h));
            return new PngResponse(ScreenCapturer.Take());
        }

        public IRequestResponse Suspend(string token)
        {
            if ((Environment.UserDomainName + DateTime.Today).VerifyHash<SHA256Cng>(token))
            {
                var action = new Action(() =>
                {
                    Thread.Sleep(5000);
                    ProcessStartInfo psi = new ProcessStartInfo("rundll32.exe", " Powrprof.dll,SetSuspendState 1,,1");
                    Process.Start(psi);
                });
                action.BeginInvoke(null, null);
            }
            return new StringResponse("GoodBye");
        }
    }
}