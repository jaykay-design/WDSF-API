﻿namespace Wdsf.Api.Client
{
    using Serializer;
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Net;
    using System.Text;
    using Wdsf.Api.Client.Exceptions;
    using Wdsf.Api.Client.Models;

    internal class RestAdapter : IDisposable
    {
        public bool IsAssigned { get; set; }
        public bool IsBusy { get; private set; }
        private readonly object busyLock = new object();

        public ContentTypes ContentType { get; set; }

        private readonly WebClient client = new WebClient();
        private readonly string username;
        private readonly string password;
        private readonly string onBehalfOf;

        public RestAdapter(string username, string password, string onBehalfOf = null)
        {
            this.username = username;
            this.password = password;
            this.onBehalfOf = onBehalfOf;

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

            var request = GetRequest(resourceUri);
            using (var response = GetResponse(request))
            {
                this.IsBusy = false;
                var receivedType = TypeHelper.GetApiModelType(response.ContentType);
                if (receivedType == null)
                {
                    throw new UnknownMediaTypeException(response.ContentType);
                }

                var serializer = SerializerFactory.GetSerializer(this.ContentType);
                var result = serializer.Deserialize(receivedType, response.GetResponseStream());

                response.Close();
                return result;
            }
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

            var request = GetRequest(resourceUri);

            using (var response = GetResponse(request))
            {
                this.IsBusy = false;

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedException("GET", resourceUri);
                }

                CheckResourceType<T>(response);
                T result = ReadReponseBody<T>(response);
                response.Close();

                return result;
            }
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

            var request = GetRequestForSending(resourceUri);
            request.Method = "PUT";
            request.ContentType = GetContentType(typeof(T));
            WriteRequestBody(model, request);

            using (var response = GetResponse(request))
            {
                this.IsBusy = false;

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedException("PUT", resourceUri);
                }

                CheckResourceType<StatusMessage>(response);

                var message = ReadReponseBody<StatusMessage>(response);
                response.Close();

                return message;
            }
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

            var request = GetRequestForSending(resourceUri);
            request.Method = "POST";
            request.ContentType = GetContentType(typeof(T));
            WriteRequestBody(model, request);

            using (var response = GetResponse(request))
            {
                this.IsBusy = false;

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedException("POST", resourceUri);
                }

                CheckResourceType<StatusMessage>(response);
                StatusMessage message = ReadReponseBody<StatusMessage>(response);
                response.Close();

                return message;
            }
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

            var request = GetRequestForSending(resourceUri);
            request.Method = "DELETE";

            using (var response = GetResponse(request))
            {
                this.IsBusy = false;

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedException("DELETE", resourceUri);
                }

                CheckResourceType<StatusMessage>(response);

                var message = ReadReponseBody<StatusMessage>(response);
                response.Close();

                return message;
            }
        }

        /// <summary>
        /// Verifies if the received content-type corresponds to the requested one.
        /// </summary>
        /// <typeparam name="T">The type expected</typeparam>
        /// <param name="response">The response containing a content-type header</param>
        private void CheckResourceType<T>(HttpWebResponse response) where T : class
        {
            var receivedType = TypeHelper.GetApiModelType(response.ContentType);

            if (receivedType == null)
            {
                var content = new StreamReader(response.GetResponseStream()).ReadToEnd();
                throw new UnknownMediaTypeException(response.ContentType, content);
            }

            if (receivedType != typeof(T))
            {
                if (receivedType == typeof(StatusMessage))
                {
                    var message = ReadReponseBody<StatusMessage>(response);
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
        private T ReadReponseBody<T>(HttpWebResponse response) where T : class
        {
            var serializer = SerializerFactory.GetSerializer(this.ContentType);
            T result = serializer.Deserialize(typeof(T), response.GetResponseStream()) as T;

            return result;
        }

        private HttpWebRequest GetRequest(Uri resourceUri)
        {
            var request = WebRequest.Create(resourceUri) as HttpWebRequest;
            var auth = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(username + ":" + password));

            request.PreAuthenticate = true;
            request.Headers.Add("Authorization", auth);

            request.Accept = GetAcceptType();
            request.Headers.Add("Accept-Encoding", "gzip");

            request.UserAgent = "WDSF API Client";
            request.AutomaticDecompression = DecompressionMethods.GZip;

            if (!string.IsNullOrEmpty(onBehalfOf))
            {
                request.Headers.Add("X-OnBehalfOf", onBehalfOf);
            }

            return request;
        }
        private HttpWebRequest GetRequestForSending(Uri resourceUri)
        {
            var request = GetRequest(resourceUri);

#if !DEBUG
            request.Headers.Add("Content-Encoding", "gzip");
#endif

            return request;
        }
        private string GetAcceptType()
        {
            switch (this.ContentType)
            {
                case ContentTypes.Xml: { return "application/xml"; }
                case ContentTypes.Json: { return "application/json"; }
                default:
                    {
                        throw new Exception("ApiDataFormat invalid.");
                    }
            }
        }
        private string GetContentType(Type contentType)
        {
            string typeName = TypeHelper.GetHttpContentType(contentType);

            switch (this.ContentType)
            {
                case ContentTypes.Xml: { return typeName + "+" + "xml"; }
                case ContentTypes.Json: { return typeName + "+" + "json"; }
                default:
                    {
                        throw new Exception("ApiDataFormat invalid.");
                    }
            }
        }

        private void WriteRequestBody<T>(T model, HttpWebRequest request) where T : class
        {
            var serializer = SerializerFactory.GetSerializer(this.ContentType);

            using (var data = new MemoryStream())
            {
                if (request.Headers["Content-Encoding"] == "gzip")
                {
                    using (var compressor = new GZipStream(data, CompressionMode.Compress, true))
                    {
                        serializer.Serialize(typeof(T), model, compressor);
                    }
                }
                else
                {
                    serializer.Serialize(typeof(T), model, data);
                }

                data.Position = 0;

                using (var requestStream = request.GetRequestStream())
                {
                    data.WriteTo(requestStream);
                    requestStream.Flush();
                    requestStream.Close();
                }
            }
        }

#region IDisposable Members

        public void Dispose()
        {
            this.client.Dispose();
        }

#endregion
    }
}
