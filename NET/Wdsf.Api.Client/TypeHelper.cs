/*  Copyright (C) 2011-2012 JayKay-Design S.C.
    Author: John Caprez jay@jaykay-design.com

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU LEsser General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/lgpl.html>.
 */

namespace Wdsf.Api.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Wdsf.Api.Client.Attributes;

    internal static class TypeHelper
    {
        private static Dictionary<string, Type> mediaTypeTypeMap =
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
            if (mediaTypeTypeMap.ContainsValue(type))
            {
                return mediaTypeTypeMap.First(m => m.Value == type).Key;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Maps a HTTP content-type to an API Model.
        /// </summary>
        /// <param name="mediaType">the HTTP content-type value.</param>
        /// <returns>The API model or null if there is no related type.</returns>
        public static Type GetApiModelType(string mediaType)
        {
            // remove media-type format (+xml, +json, etc)
            mediaType = mediaType.ToLower().Split('+')[0];

            if (mediaTypeTypeMap.ContainsKey(mediaType))
            {
                return mediaTypeTypeMap[mediaType];
            }
            else
            {
                return null;
            }
        }
    }
}
