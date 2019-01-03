using System;

namespace WebApplication8.Models
{
    /// <summary>
    /// This is the application error view model.
    /// </summary>
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}