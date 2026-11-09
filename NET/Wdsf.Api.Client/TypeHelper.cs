namespace Wdsf.Api.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Wdsf.Api.Client.Attributes;

    public static class TypeHelper
    {
        // One model per media type; ToDictionary fails at first use if two models share one.
        private static readonly Dictionary<string, Type> mediaTypeTypeMap =
            Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.Namespace == "Wdsf.Api.Client.Models")
            .Where<Type>(t => t.GetCustomAttributes(typeof(MediaTypeAttribute), false).Length != 0)
            .ToDictionary(
                k => ((MediaTypeAttribute)k.GetCustomAttributes(typeof(MediaTypeAttribute), false).First()).MediaType,
                v => v);

        /// <summary>
        /// Maps a API Model to a HTTP content-type.
        /// </summary>
        /// <param name="type">The API model.</param>
        /// <returns>the HTTP content-type string or null if there is no related type.</returns>
        public static string GetHttpContentType(Type type)
        {
            return mediaTypeTypeMap.FirstOrDefault(m => m.Value == type).Key;
        }

        /// <summary>
        /// Maps a HTTP content-type to an API Model.
        /// </summary>
        /// <param name="mediaType">the HTTP content-type value.</param>
        /// <returns>The API model or null if there is no related type.</returns>
        public static Type GetApiModelType(string mediaType)
        {
            if (string.IsNullOrEmpty(mediaType))
            {
                return null;
            }

            // remove parameters (; charset=...) and the media-type format (+xml, +json, etc)
            mediaType = mediaType.ToLowerInvariant().Split(';')[0].Split('+')[0].Trim();

            return mediaTypeTypeMap.TryGetValue(mediaType, out var type) ? type : null;
        }
    }
}
