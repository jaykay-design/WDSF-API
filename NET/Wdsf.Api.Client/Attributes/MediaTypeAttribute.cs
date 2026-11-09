namespace Wdsf.Api.Client.Attributes
{
    using System;

    /// <summary>
    /// The media type of a model. Every model has its own: the client picks the model of a response by it.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class MediaTypeAttribute : Attribute
    {
        public MediaTypeAttribute(string mediaType)
            : base()
        {
            this.MediaType = mediaType;
            this.IsCollection = false;
        }

        public bool IsCollection { get; set; }
        public string MediaType { get; set; }
    }
}
