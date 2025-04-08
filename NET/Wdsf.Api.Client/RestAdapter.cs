namespace Wdsf.Api.Client
{
    using Serializer;
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using Wdsf.Api.Client.Exceptions;
    using Wdsf.Api.Client.Models;

    internal class RestAdapter : IDisposable
    {
        public bool IsAssigned { get; set; }
        public bool IsBusy { get; private set; }
        private readonly object busyLock = new object();

        public ContentTypes ContentType { get; set; }

        private readonly HttpClient client;

        public RestAdapter(string username, string password, string onBehalfOf = null)
        {
            HttpMessageHandler handler = new HttpClientHandler()
            {
                PreAuthenticate = true,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };
            this.client = new HttpClient(handler);


            client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip");
            client.DefaultRequestHeaders.Add("User-Agent", "WDSF API Client");

            var auth = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(username + ":" + password));
            client.DefaultRequestHeaders.Add("Authorization", auth);
            client.Timeout = TimeSpan.FromSeconds(600);

            if (!string.IsNullOrEmpty(onBehalfOf))
            {
                client.DefaultRequestHeaders.Add("X-OnBehalfOf", onBehalfOf);
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

            using (var response = GetResponse(resourceUri))
            {
                this.IsBusy = false;
                var contentTypeHeader = response.Content.Headers.ContentType.ToString();
                var receivedType = TypeHelper.GetApiModelType(contentTypeHeader);

                if (receivedType == null)
                {
                    throw new UnknownMediaTypeException(contentTypeHeader);
                }

                var serializer = SerializerFactory.GetSerializer(this.ContentType);
                var result = serializer.Deserialize(receivedType, response.Content.ReadAsStreamAsync().Result);

                response.Dispose();
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

            using (var response = GetResponse(resourceUri))
            {
                if (response == null)
                {
                    this.IsBusy = false;
                    throw new Exception("Connection failed");
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    this.IsBusy = false;
                    throw new UnauthorizedException("GET", resourceUri);
                }

                CheckResourceType<T>(response);
                T result = ReadReponseBody<T>(response);

                this.IsBusy = false;

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

            using (var response = GetResponse(resourceUri, "PUT", GetContentType(typeof(T)), model))
            {
                this.IsBusy = false;

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedException("PUT", resourceUri);
                }

                CheckResourceType<StatusMessage>(response);

                var message = ReadReponseBody<StatusMessage>(response);

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

            using (var response = GetResponse(resourceUri, "POST", GetContentType(typeof(T)), model))
            {
                this.IsBusy = false;

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedException("POST", resourceUri);
                }

                CheckResourceType<StatusMessage>(response);
                StatusMessage message = ReadReponseBody<StatusMessage>(response);

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

            using (var response = GetResponse<object>(resourceUri, "DELETE", string.Empty, null))
            {
                this.IsBusy = false;

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedException("DELETE", resourceUri);
                }

                CheckResourceType<StatusMessage>(response);

                var message = ReadReponseBody<StatusMessage>(response);

                return message;
            }
        }

        /// <summary>
        /// Verifies if the received content-type corresponds to the requested one.
        /// </summary>
        /// <typeparam name="T">The type expected</typeparam>
        /// <param name="response">The response containing a content-type header</param>
        private void CheckResourceType<T>(HttpResponseMessage response) where T : class
        {
            var contentTypeHeader = response.Content.Headers.ContentType.ToString();
            var receivedType = TypeHelper.GetApiModelType(contentTypeHeader);

            if (receivedType == null)
            {
                var content = new StreamReader(response.Content.ReadAsStringAsync().Result).ReadToEnd();
                throw new UnknownMediaTypeException(contentTypeHeader, content);
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

        private HttpResponseMessage GetResponse(Uri resourceUri)
        {
            client.DefaultRequestHeaders.Add("Accept", GetAcceptType());

            HttpResponseMessage response;
            try
            {
                response = client.GetAsync(resourceUri).Result;
            }
            catch (HttpRequestException)
            {
                response = null;
            }

            return response;
        }

        private HttpResponseMessage GetResponse<T>(Uri resourceUri, string method, string contentType = null, T data = default) where T : class
        {
            client.DefaultRequestHeaders.Add("Accept", GetAcceptType());

            HttpResponseMessage response;
            try
            {
                switch (method)
                {
                    case "PUT":
                        {
                            var content = GetRequestBody(data);
                            response = client.PutAsync(resourceUri, content).Result;
                            break;
                        }
                    case "POST":
                        {
                            var content = GetRequestBody(data);
                            response = client.PostAsync(resourceUri, content).Result;
                            break;
                        }
                    case "DELETE":
                        {
                            response = client.DeleteAsync(resourceUri).Result;
                            break;
                        }
                    default:
                    {
                        throw new Exception("Method not supported");
                    }
                }
            }
            catch (HttpRequestException)
            {
                response = null;
            }

            return response;
        }
        private T ReadReponseBody<T>(HttpResponseMessage response) where T : class
        {
            var serializer = SerializerFactory.GetSerializer(this.ContentType);
            T result = serializer.Deserialize(typeof(T), response.Content.ReadAsStreamAsync().Result) as T;

            return result;
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

        private HttpContent GetRequestBody<T>(T model) where T : class
        {
            var serializer = SerializerFactory.GetSerializer(this.ContentType);

            var data = new MemoryStream();
#if !DEBUG
                    using (var compressor = new GZipStream(data, CompressionMode.Compress, true))
                    {
                        serializer.Serialize(typeof(T), model, compressor);
                    }
#else
            serializer.Serialize(typeof(T), model, data);
#endif

            data.Position = 0;

            var content = new StreamContent(data);
            content.Headers.Add("Content-Type", GetContentType(typeof(T)));
#if !DEBUG
            content.Headers.Add("Content-Encoding", "gzip");
#endif

            return content;
        }

        #region IDisposable Members

        public void Dispose()
        {
            this.client.Dispose();
        }

#endregion
    }
}
