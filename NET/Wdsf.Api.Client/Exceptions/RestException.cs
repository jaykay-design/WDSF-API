namespace Wdsf.Api.Client.Exceptions
{
    using System;
    using Wdsf.Api.Client.Models;

    /// <summary>
    /// The API answered with a status message instead of the requested resource.
    /// </summary>
    public class RestException : Exception
    {

        public RestException(StatusMessage message)
            : base(string.IsNullOrEmpty(message.Error) ? message.Message : message.Message + " " + message.Error)
        {
            this.Code = message.Code;
            this.MessageId = message.MessageId;
            this.Error = message.Error;
            this.StatusMessage = message;
        }

        public RestException(string message, int code, int messageId)
            : base(message)
        {
            this.Code = code;
            this.MessageId = messageId;
        }

        /// <summary>
        /// The HTTP status code.
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// The id of the condition (see the API wiki, Localisation).
        /// </summary>
        public int MessageId { get; set; }

        /// <summary>
        /// The details of the condition, if any.
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// The status message the API returned, if any.
        /// </summary>
        public StatusMessage StatusMessage { get; }
    }
}
