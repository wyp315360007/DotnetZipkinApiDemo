using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ZipkinExtensions
{
    public class TraceObserver : IObserver<DiagnosticListener>
    {
        private IEnumerable<ITraceDiagnosticListener> _traceDiagnostics;
        public TraceObserver(IEnumerable<ITraceDiagnosticListener> traceDiagnostics)
        {
            _traceDiagnostics = traceDiagnostics;
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(DiagnosticListener listener)
        {
            System.Diagnostics.Debug.WriteLine(listener.Name);
            var traceDiagnostic = _traceDiagnostics.FirstOrDefault(i => i.DiagnosticName == listener.Name);
            if (traceDiagnostic != null)
            {
                //适配订阅
                listener.SubscribeWithAdapter(traceDiagnostic);
            }
        }
    }
}
