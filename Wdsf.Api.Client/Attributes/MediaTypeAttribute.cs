namespace Wdsf.Api.Client.Attributes
{
    using System;

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
