using System;
using System.Diagnostics.CodeAnalysis;

namespace MC.LocalStorage
{
    [ExcludeFromCodeCoverage]
    public class ChangingEventArgs : ChangedEventArgs
    {
        public bool Cancel { get; set; }
    }
}
