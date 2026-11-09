namespace Wdsf.Api.Client
{
    using System;

    /// <summary>
    /// A file the API returned.
    /// </summary>
    public class ApiFile
    {
        /// <summary>
        /// The file's content; null for a headers-only (HEAD) request.
        /// </summary>
        public byte[] Content { get; set; }

        /// <summary>
        /// The media type, e.g. audio/mpeg or application/zip.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// The file name the API suggests.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// When the file last changed. Pass it back as ifModifiedSince to download only changes.
        /// </summary>
        public DateTimeOffset? LastModified { get; set; }

        /// <summary>
        /// The file is a zip archive of several files.
        /// </summary>
        public bool IsArchive => string.Equals(ContentType, "application/zip", StringComparison.OrdinalIgnoreCase);
    }
}
