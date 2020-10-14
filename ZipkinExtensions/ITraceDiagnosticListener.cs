using System;
namespace ZipkinExtensions
{
    public interface ITraceDiagnosticListener
    {
        string DiagnosticName { get; }
    }
}
