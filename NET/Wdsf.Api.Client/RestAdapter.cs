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
    using System.IO;
    using System.Net;
    using System.Security;
    using System.Runtime.InteropServices;
    using System.Xml.Serialization;
    using Wdsf.Api.Client.Exceptions;
    using Wdsf.Api.Client.Models;

    internal class RestAdapter : IDisposable
    {
        public bool IsBusy { get; private set; }
        private object busyLock = new object();

        private readonly WebClient client = new WebClient();
        private readonly ICredentials credentials;

        public RestAdapter(string username, SecureString password)
        {
            IntPtr strPointer = IntPtr.Zero;
            try
            {
                strPointer = Marshal.SecureStringToBSTR(password);
                this.credentials = new NetworkCredential(username, Marshal.PtrToStringBSTR(strPointer));
            }
            finally
            {
                if (IntPtr.Zero != strPointer)
                {
                    Marshal.ZeroFreeBSTR(strPointer);
                    strPointer = IntPtr.Zero;
                }
            }

        }

        /// <summary>
        /// Gets a resource of unknown type.
        /// </summary>
        /// <param name="resourceUri">The Uri to the resource.</param>
        /// <exception cref="UnknownMediaTypeException">The received resource type was not recognized.</exception>
        /// <exception cref="BusyException">The adapter is busy, use a new instance or check the IsBusy property.</exception>
        /// <exception cref="HttpWebException">A general failure.</exception>
        /// <returns>The resource.</returns>
        internal object Get(Uri resourceUri)
        {
            CheckAndSetBusy();

            HttpWebRequest request = GetRequest(resourceUri);

            HttpWebResponse response = GetResponse(request);
            this.IsBusy = false;
            Type receivedType = TypeHelper.GetApiModelType(response.ContentType);
            if (receivedType == null)
            {
                throw new UnknownMediaTypeException(response.ContentType);
            }

            XmlSerializer serializer = new XmlSerializer(receivedType);
            object result =  serializer.Deserialize(response.GetResponseStream());
            response.Close();

            return result;
        }

        /// <summary>
        /// Gets a resource.
        /// </summary>
        /// <typeparam name="T">The expected resource type.</typeparam>
        /// <param name="resourceUri">The Uri to the resource.</param>
        /// <exception cref="UnknownMediaTypeException">The received resource type was not recognized.</exception>
        /// <exception cref="UnexpectedMediaTypeException">The received resource type was not expected.</exception>
        /// <exception cref="RestException">The API returned a message instead of a resource.</exception>
        /// <exception cref="BusyException">The adapter is busy, use a new instance or check the IsBusy property.</exception>
        /// <exception cref="HttpWebException">A general failure.</exception>
        /// <returns>The resource.</returns>
        public T Get<T>(Uri resourceUri) where T : class
        {
            CheckAndSetBusy();

            HttpWebRequest request = GetRequest(resourceUri);
            
            HttpWebResponse response = GetResponse(request);
            this.IsBusy = false;
            CheckResourceType<T>(response);
            T result =  ReadReponseBody<T>(response);
            response.Close();

            return result;
        }

        /// <summary>
        /// Updates a resource.
        /// </summary>
        /// <typeparam name="T">The resource type.</typeparam>
        /// <param name="resourceUri">The Uri to the resource.</param>
        /// <exception cref="UnknownMediaTypeException">The received message type was not recognized.</exception>
        /// <exception cref="UnexpectedMediaTypeException">The received message type was not expected.</exception>
        /// <exception cref="BusyException">The adapter is busy, use a new instance or check the IsBusy property.</exception>
        /// <exception cref="HttpWebException">A general failure.</exception>
        /// <returns>A status message.</returns>
        public StatusMessage Put<T>(Uri resourceUri, T model) where T : class
        {
            CheckAndSetBusy();

            HttpWebRequest request = GetRequestForSending(resourceUri);
            request.Method = "PUT";
            request.ContentType = string.Format("{0}+xml", TypeHelper.GetHttpContentType(typeof(T)));
            WriteRequestBody<T>(model, request);

            HttpWebResponse response = GetResponse(request);
            this.IsBusy = false;
            CheckResourceType<StatusMessage>(response);
            StatusMessage message = ReadReponseBody<StatusMessage>(response);
            response.Close();

            return message;
        }

        /// <summary>
        /// Saves a new resource.
        /// </summary>
        /// <typeparam name="T">The resource type.</typeparam>
        /// <param name="resourceUri">The Uri to the resource.</param>
        /// <exception cref="UnknownMediaTypeException">The received message type was not recognized.</exception>
        /// <exception cref="UnexpectedMediaTypeException">The received message type was not expected.</exception>
        /// <exception cref="BusyException">The adapter is busy, use a new instance or check the IsBusy property.</exception>
        /// <exception cref="HttpWebException">A general failure.</exception>
        /// <returns>A status message containing the Uri to the new resource.</returns>
        public StatusMessage Post<T>(Uri resourceUri, T model) where T : class
        {
            CheckAndSetBusy();

            HttpWebRequest request = GetRequestForSending(resourceUri);
            request.Method = "POST";
            request.ContentType = string.Format("{0}+xml", TypeHelper.GetHttpContentType(typeof(T)));
            WriteRequestBody<T>(model, request);

            HttpWebResponse response = GetResponse(request);
            this.IsBusy = false;
            CheckResourceType<StatusMessage>(response);
            StatusMessage message = ReadReponseBody<StatusMessage>(response);
            response.Close();

            return message;
        }

        /// <summary>
        /// Deletes a resource.
        /// </summary>
        /// <param name="resourceUri">The Uri to the resource.</param>
        /// <exception cref="UnknownMediaTypeException">The received message type was not recognized.</exception>
        /// <exception cref="UnexpectedMediaTypeException">The received message type was not expected.</exception>
        /// <exception cref="BusyException">The adapter is busy, use a new instance or check the IsBusy property.</exception>
        /// <exception cref="HttpWebException">A general failure.</exception>
        /// <returns>A status message.</returns>
        public StatusMessage Delete(Uri resourceUri)
        {
            CheckAndSetBusy();

            HttpWebRequest request = GetRequestForSending(resourceUri);
            request.Method = "DELETE";

            HttpWebResponse response = GetResponse(request);
            this.IsBusy = false;
            CheckResourceType<StatusMessage>(response);
            StatusMessage message = ReadReponseBody<StatusMessage>(response);
            response.Close();

            return message;
        }

        /// <summary>
        /// Verifies if the received content-type corresponds to the requested one.
        /// </summary>
        /// <typeparam name="T">The type expected</typeparam>
        /// <param name="response">The response containing a content-type header</param>
        private void CheckResourceType<T>(HttpWebResponse response) where T : class
        {
            Type receivedType = TypeHelper.GetApiModelType(response.ContentType);

            if (receivedType == null)
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new UnauthorizedAccessException();

                throw new UnknownMediaTypeException(response.ContentType);
            }

            if(receivedType != typeof(T))
            {
                if (receivedType == typeof(StatusMessage))
                {
                    StatusMessage message = ReadReponseBody<StatusMessage>(response);
                    throw new RestException(message);
                }
                else
                {
                    throw new UnexpectedMediaTypeException(typeof(T), receivedType);
                }
            }
        }

        private void CheckAndSetBusy()
        {
            lock (this.busyLock)
            {
                if (this.IsBusy)
                {
                    throw new BusyException();
                }

                this.IsBusy = true;
            }
        }

        private HttpWebResponse GetResponse(HttpWebRequest request)
        {
            WebResponse response;
            try
            {
                response = request.GetResponse();
            }
            catch (WebException ex)
            {
                response = ex.Response;
            }

            return response as HttpWebResponse;
        }
        private T ReadReponseBody<T>(HttpWebResponse response) where T:class
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            T result = serializer.Deserialize(response.GetResponseStream()) as T;

            return result;
        }

        private HttpWebRequest GetRequest(Uri resourceUri)
        {
            HttpWebRequest request = WebRequest.Create(resourceUri) as HttpWebRequest;

            request.Credentials = this.credentials;
            request.PreAuthenticate = true;
            request.Accept = "application/xml";
            request.UserAgent = "WDSF API Client";
            request.AutomaticDecompression = DecompressionMethods.GZip;

            return request;
        }
        private HttpWebRequest GetRequestForSending(Uri resourceUri)
        {
            HttpWebRequest request = WebRequest.Create(resourceUri) as HttpWebRequest;

            request.Credentials = this.credentials;
            request.PreAuthenticate = true;
            request.Accept = "application/xml";
            request.UserAgent = "WDSF API Client";
            request.Headers.Add("Content-Encoding", "gzip");

            request.AutomaticDecompression = DecompressionMethods.GZip;

            return request;
        }
        private void WriteRequestBody<T>(T model, HttpWebRequest request) where T : class
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("xlink", "http://www.w3.org/1999/xlink");

            Stream requestStream = request.GetRequestStream();

            serializer.Serialize(requestStream, model, ns);

            requestStream.Flush();
            requestStream.Close();
        }

        #region IDisposable Members

        public void Dispose()
        {
 	        this.client.Dispose();
        }

        #endregion
    }
}
