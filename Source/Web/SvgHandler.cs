using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Web;

namespace Svg.Web
{
    /// <inheritdoc />
    /// <summary>
    /// A handler to asynchronously render Scalable Vector Graphics files (usually *.svg or *.xml file extensions).
    /// </summary>
    /// <remarks>
    /// <para>If a crawler requests the SVG file the raw XML will be returned rather than the image to allow crawlers to better read the image.</para>
    /// <para>Adding "?raw=true" to the querystring will alos force the handler to render the raw SVG.</para>
    /// </remarks>
    public class SvgHandler : IHttpAsyncHandler
    {
        Thread _t;

        /// <inheritdoc />
        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler" /> instance.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="T:System.Web.IHttpHandler" /> instance is reusable; otherwise, false.</returns>
        public bool IsReusable => false;

        /// <inheritdoc />
        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler" /> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext" /> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        public void ProcessRequest(HttpContext context)
        {
            // Not used
        }

        /// <inheritdoc />
        /// <summary>
        /// Initiates an asynchronous call to the HTTP handler.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext" /> object that provides references to intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        /// <param name="cb">The <see cref="T:System.AsyncCallback" /> to call when the asynchronous method call is complete. If <paramref name="cb" /> is null, the delegate is not called.</param>
        /// <param name="extraData">Any extra data needed to process the request.</param>
        /// <returns>
        /// An <see cref="T:System.IAsyncResult" /> that contains information about the status of the process.
        /// </returns>
        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            var path = context.Request.PhysicalPath;

            if (!File.Exists(path))
            {
                throw new HttpException(404, "The requested file cannot be found.");
            }

            var reqState = new SvgAsyncRenderState(context, cb, extraData);
            var asyncRender = new SvgAsyncRender(reqState);
            var ts = new ThreadStart(asyncRender.RenderSvg);
            _t = new Thread(ts);
            _t.Start();

            return reqState;
        }

        /// <inheritdoc />
        /// <summary>
        /// Provides an asynchronous process End method when the process ends.
        /// </summary>
        /// <param name="result">An <see cref="T:System.IAsyncResult" /> that contains information about the status of the process.</param>
        public void EndProcessRequest(IAsyncResult result)
        {
            
        }

        /// <summary>
        /// The class to be used when 
        /// </summary>
        protected sealed class SvgAsyncRender
        {
            private SvgAsyncRenderState _state;

            public SvgAsyncRender(SvgAsyncRenderState state)
            {
                _state = state;
            }

            private void RenderRawSvg()
            {
                _state.Context.Response.ContentType = "image/svg+xml";
                _state.Context.Response.WriteFile(_state.Context.Request.PhysicalPath);
                _state.Context.Response.End();
                _state.CompleteRequest();
            }

            public void RenderSvg()
            {
                _state.Context.Response.AddFileDependency(_state.Context.Request.PhysicalPath);
                _state.Context.Response.Cache.SetLastModifiedFromFileDependencies();
                _state.Context.Response.Cache.SetETagFromFileDependencies();
                _state.Context.Response.Buffer = false;

                // Allow crawlers to see the raw XML - they can get more information from it that way
                if (_state.Context.Request.Browser.Crawler || !string.IsNullOrEmpty(_state.Context.Request.QueryString["raw"]))
                {
                    RenderRawSvg();
                }
                else
                {
                    try
                    {
                        Dictionary<string, string> entities = new Dictionary<string, string>();
                        NameValueCollection queryString = _state.Context.Request.QueryString;

                        for (int i = 0; i < queryString.Count; i++)
                        {
                            entities.Add(queryString.Keys[i], queryString[i]);
                        }

                        SvgDocument document = SvgDocument.Open<SvgDocument>(_state.Context.Request.PhysicalPath, entities);

                        using (Bitmap bitmap = document.Draw())
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                bitmap.Save(ms, ImageFormat.Png);
                                _state.Context.Response.ContentType = "image/png";
                                ms.WriteTo(_state.Context.Response.OutputStream);
                            }
                        }
                    }
                    catch (Exception exc)
                    {
                        System.Diagnostics.Trace.TraceError("An error occured while attempting to render the SVG image '" + _state.Context.Request.PhysicalPath + "': " + exc.Message);
                    }
                    finally
                    {
                        _state.Context.Response.End();
                        _state.CompleteRequest();
                    }
                }
            }
        }

        /// <summary>
        /// Represents the state of a request for SVG rendering.
        /// </summary>
        protected sealed class SvgAsyncRenderState : IAsyncResult
        {
            internal HttpContext Context;
            internal AsyncCallback Callback;
            internal object ExtraData;
            private bool _isCompleted;
            private ManualResetEvent _callCompleteEvent;

            /// <summary>
            /// Initializes a new instance of the <see cref="SvgAsyncRenderState"/> class.
            /// </summary>
            /// <param name="context">The <see cref="HttpContext"/> of the request.</param>
            /// <param name="callback">The delegate to be called when the rendering is complete.</param>
            /// <param name="extraData">The extra data.</param>
            public SvgAsyncRenderState(HttpContext context, AsyncCallback callback, object extraData)
            {
                Context = context;
                Callback = callback;
                ExtraData = extraData;
            }

            /// <summary>
            /// Indicates that the rendering is complete and the waiting thread may proceed.
            /// </summary>
            internal void CompleteRequest()
            {
                _isCompleted = true;
                lock (this)
                {
                    _callCompleteEvent.Set();
                }
                // if a callback was registered, invoke it now
                Callback?.Invoke(this);
            }

            /// <summary>
            /// Gets a user-defined object that qualifies or contains information about an asynchronous operation.
            /// </summary>
            /// <value></value>
            /// <returns>A user-defined object that qualifies or contains information about an asynchronous operation.</returns>
            public object AsyncState { get { return (ExtraData); } }
            /// <summary>
            /// Gets an indication of whether the asynchronous operation completed synchronously.
            /// </summary>
            /// <value></value>
            /// <returns>true if the asynchronous operation completed synchronously; otherwise, false.</returns>
            public bool CompletedSynchronously { get { return (false); } }
            /// <summary>
            /// Gets an indication whether the asynchronous operation has completed.
            /// </summary>
            /// <value></value>
            /// <returns>true if the operation is complete; otherwise, false.</returns>
            public bool IsCompleted { get { return (_isCompleted); } }
            /// <summary>
            /// Gets a <see cref="T:System.Threading.WaitHandle"/> that is used to wait for an asynchronous operation to complete.
            /// </summary>
            /// <value></value>
            /// <returns>A <see cref="T:System.Threading.WaitHandle"/> that is used to wait for an asynchronous operation to complete.</returns>
            public WaitHandle AsyncWaitHandle
            {
                get
                {
                    lock (this)
                    {
                        if (_callCompleteEvent == null)
                        {
                            _callCompleteEvent = new ManualResetEvent(false);
                        }

                        return _callCompleteEvent;
                    }
                }
            }
        }
    }
}