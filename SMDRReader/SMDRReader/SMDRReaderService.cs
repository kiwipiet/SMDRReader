using ReactiveSockets;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;

namespace SMDRReader
{
    internal class SMDRReaderService : IDisposable
    {
        readonly ReactiveListener _server;
        readonly IServiceConfiguration _config;
        public SMDRReaderService(IServiceConfiguration config)
        {
            _config = config;
            _server = new ReactiveListener(_config.Port);
            SetupSubscriptions(_server);
        }
        public SMDRReaderService()
            : this(new ServiceConfigurationImpl())
        {
        }

        private static void SetupSubscriptions(ReactiveListener server)
        {
            server.Connections.Subscribe(socket =>
            {
                var messages = Observable.Create<SMDRRecord>(observer =>
                {
                    var bom = Encoding.ASCII.GetChars(Encoding.ASCII.GetPreamble()).FirstOrDefault();
                    var sb = new StringBuilder();
                    var prev = default(char);

                    return socket.Receiver.Subscribe(b =>
                    {
                        var c = Convert.ToChar(b);
                        if (c == bom) { } // skip bom
                        else if (prev == '\r' && c == '\n') { } // when \r\n do nothing
                        else if (c == '\r' || c == '\n')   // reach at EndOfLine
                        {
                            var str = sb.ToString();
                            sb.Clear();
                            observer.OnNext(new SMDRRecord(str));
                        }
                        else sb.Append(c); // normally char

                        prev = c;
                    },
                    observer.OnError,
                    () =>
                    {
                        var str = sb.ToString();
                        if (!String.IsNullOrEmpty(str)) observer.OnNext(new SMDRRecord(str));
                        observer.OnCompleted();
                    });
                });

                messages.Subscribe(message =>
                {
                    Console.WriteLine("Message:");
                    Console.WriteLine("{0}", new SMDRRecordFormatter(message));
                });

            });

        }
        public void Start()
        {
            _server.Start();
        }

        #region Dispose
        ~SMDRReaderService()
        {
            Dispose(false);
        }

        protected void Dispose(bool disposing)
        {
            if (CheckIfDisposed())
            {
                if (disposing)
                {
                    if (_server != null)
                        _server.Dispose();
                }
            }
        }
        private int _disposed;
        /// <summary>
        /// Checks the disposed local variable.
        /// <para>NB!! Do not call this from anywhere except the Dispose method!</para>
        /// </summary>
        /// <returns></returns>
        private bool CheckIfDisposed()
        {
            return Interlocked.CompareExchange(ref _disposed, 1, 0) == 0;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
