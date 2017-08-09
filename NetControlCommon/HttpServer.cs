using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetControlCommon
{
    public class HttpServer
    {
        private Action<string, string> log;
        public event EventHandler Stopped;
        private readonly W<bool> _cancelationPending = false;
        private bool _isListening;

        public bool IsListening
        {
            get { return _isListening; }
        }

        public HttpServer(Action<string, string> log)
        {
            this.log = log;
        }

        private static void IgnoreFailure(Action a)
        {
            try
            {
                a();
            }
            // ReSharper disable EmptyGeneralCatchClause
            catch
            // ReSharper restore EmptyGeneralCatchClause
            {
            }
        }

        private void DefCatchFailure(Action a, string description = "")
        {
            try
            {
                a();
            }
            // ReSharper disable EmptyGeneralCatchClause
            catch (Exception ex)
            // ReSharper restore EmptyGeneralCatchClause
            {
                log(ex.Message + "\n" + description, "Произошла ошибка");
            }
        }

        public async Task StartAsync(IEnumerable<string> prefixes, int numConcurrent = 4)
        {
            try
            {
                using (var hl = new HttpListener())
                {
                    var tasks = new Task[numConcurrent];
                    var cts = new CancellationTokenSource();

                    try
                    {
                        foreach (var prefix in prefixes)
                        {
                            hl.Prefixes.Add(prefix);
                        }
                        hl.Start();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        Environment.Exit(-1);
                    }
                    for (var i = 0; i < tasks.Length; ++i)
                    {
                        StartTask(tasks, i, cts.Token, hl);
                    }
                    _isListening = true;
                    await Task.Factory.StartNew(o =>
                    {
                        while (!_cancelationPending)
                        {
                            Thread.Sleep(500);
                        }
                    }, _cancelationPending, TaskCreationOptions.LongRunning);

                    cts.Cancel();

                    _isListening = false;
                    foreach (var t in tasks)
                    {
                        t.Wait(TimeSpan.FromSeconds(15));
                    }
                    hl.Stop();
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException is TaskCanceledException &&
                    (ex.InnerException as TaskCanceledException).Task.IsCanceled)
                {
                    log("Сервер остановлен успешно", "Сервер.");
                    Stopped?.Invoke(this, new EventArgs());
                }
                else
                    log(ex.Message, "Ошибка");
            }
        }



        private void StartTask(Task[] tasks, int i, CancellationToken token, HttpListener hl)
        {
            tasks[i] =
                hl
                    .GetContextAsync()
                    .ContinueWith(ProcessRequest, token)
                    .ContinueWith(_ => StartTask(tasks, i, token, hl), token);
        }

        private void ProcessRequest(Task<HttpListenerContext> task)
        {
            ControllerFactory cf = new ControllerFactory();
            DefCatchFailure(() =>
                {
                    if (!task.IsCompleted)
                        return;
                    var ctx = task.Result;
                    try
                    {
                        var addr = ctx.Request.Url.AbsolutePath.ToLowerInvariant()
                                .Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)
                            ;
                        if (addr.First().Equals("service"))
                        {
                            ProcessService(ctx,addr);
                        }
                        var controller = cf.GetController(addr.First());
                        IRequestResponse resp = null;
                        if (ctx.Request.HttpMethod.Equals("GET"))
                        {
                            foreach (var methodInfo in controller.GetType().GetMethods())
                            {
                                if (methodInfo.Name.ToLowerInvariant() == addr.Last()) // Если это метод с нужным названием
                                    if (methodInfo.GetParameters().Length == ctx.Request.QueryString.Count && ctx.Request.QueryString.Count == 0   // Если у метода 0 параметров
                                    || methodInfo.GetParameters().Select(p => p.Name).Intersect(ctx.Request.QueryString.AllKeys).Any()) // Или они совпадают
                                    {
                                        resp = methodInfo.Invoke(controller, ctx.Request.QueryString.AllKeys.Select(k => GetReqValue(ctx.Request.Url, k)) //выполняем этот метод
                                          .ToArray()) as IRequestResponse;
                                        break;
                                    }

                            }
                            if (resp == null) resp = new NotFoundResponse();
                            var buffer = resp.GetBytes();
                            ctx.Response.ContentType = resp.ContentType;
                            ctx.Response.OutputStream.Write(buffer, 0, buffer.Length);
                        }
                    }
                    catch (Exception ex)
                    {
                        ctx.Response.ContentType = "text/plain; charset=utf-8";
                        var buffer = Encoding.UTF8.GetBytes(ex.Message);
                        ctx.Response.OutputStream.Write(buffer, 0, buffer.Length);
                    }
                    finally
                    {
                        ctx.Response.Close();
                    }

                }
            , "Обработка запроса");
        }

        private void ProcessService(HttpListenerContext ctx, string[] addr)
        {
            if (addr.Last().Equals("terminate"))
            {
                Stop();
            }
        }
        private object GetReqValue(Uri uri, string key)
        {
            return uri.GetComponents(UriComponents.Query, UriFormat.SafeUnescaped).Split('&')
                .First(kv => kv.Split('=').First().Equals(key)).Split('=').Last();
        }

        public void Stop()
        {
            _cancelationPending.Value = true;
        }
    }
}