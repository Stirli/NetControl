using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public static class Program
    {
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

        public static void Main(string[] args)
        {
            const int numConcurrent = 4;
            IgnoreFailure(() =>
                {
                    using (var hl = new HttpListener())
                    {
                        var tasks = new Task[numConcurrent];
                        var cts = new CancellationTokenSource();

                        try
                        {
                            hl.Prefixes.Add("http://+:3333/test/");
                            hl.Start();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            Environment.Exit(-1);
                        }
                        for (var i = 0; i < tasks.Length; ++i)
                        {
                            Start(tasks, i, cts.Token, hl);
                        }
                        Console.WriteLine("Press any key to stop server.");
                        Console.ReadKey();
                        cts.Cancel();
                        foreach (var t in tasks)
                        {
                            t.Wait(TimeSpan.FromSeconds(30));
                        }
                        hl.Stop();
                    }
                }
            );
        }

        private static void Start(Task[] tasks, int i, CancellationToken token, HttpListener hl)
        {
            tasks[i] =
                hl
                    .GetContextAsync()
                    .ContinueWith(ProcessRequest, token)
                    .ContinueWith(_ => Start(tasks, i, token, hl), token);
        }

        private static void ProcessRequest(Task<HttpListenerContext> task)
        {
            IgnoreFailure(() =>
                {
                    if (!task.IsCompleted)
                        return;
                    var ctx = task.Result;
                    var filename =
                            ctx.Request.Url.AbsolutePath.ToLowerInvariant()
                                .Split('/')
                                .SkipWhile(x => x != "test")
                                .Skip(1)
                                .First()
                        ;
                    var buffer = Encoding.UTF8.GetBytes(filename);
                    ctx.Response.ContentType = "text/plain";
                    ctx.Response.OutputStream.Write(buffer, 0, buffer.Length);
                    ctx.Response.Close();
                }
            );
        }
    }
}
