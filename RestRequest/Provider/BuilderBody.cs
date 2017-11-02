﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using RestRequest.interfaces;

namespace RestRequest.Provider
{
	public class BuilderBody : BuilderBase, IBuilder
	{
		public BuilderBody(string url, HttpMethod method) : base(url, method)
		{
		}
		public IBuilderNoneBody Body(object parameters)
		{
			var body = new JsonBody();
			body.AddParameter(parameters);
			RequestBody = body;
			return this;
		}

		public IBuilderNoneBody Form(object parameters)
		{
			var body = new FormBody();
			body.AddParameter(parameters);
			RequestBody = body;
			return this;
		}

		public IBuilderNoneBody Form(Dictionary<string, string> parameters)
		{
			var body = new FormBody();
			body.AddParameter(parameters);
			RequestBody = body;
			return this;
		}

		public IBuilderNoneBody Form(IEnumerable<NamedFileStream> files, object parameters)
		{
			var body = new MultipartBody();
			body.AddParameters(parameters);
			body.AddFiles(files);
			RequestBody = body;
			return this;
		}

		public IBuilderNoneBody Form(IEnumerable<NamedFileStream> files)
		{
			var body = new MultipartBody();
			body.AddFiles(files);
			RequestBody = body;
			return this;
		}

		public IBuilderNoneBody Form(IEnumerable<NamedFileStream> files, Dictionary<string, string> parameters)
		{
			var body = new MultipartBody();
			body.AddParameters(parameters);
			body.AddFiles(files);
			RequestBody = body;
			return this;
		}

		public IActionCallback OnSuccess(Action<HttpStatusCode, string> action)
		{
			SuccessAction = action;
			return this;
		}

		public IActionCallback OnFail(Action<WebException> action)
		{
			FailAction = action;
			return this;
		}

		public void Start()
		{
			var builder = new BuilderRequest(this);
			builder.CreateRequest();
			builder.BuildRequestAndCallback();
		}

		public IBuilderNoneBody ContentType(string contenttype)
		{
			RequestBody?.SetContentType(contenttype);
			return this;
		}


		public ResponseResult<string> ResponseString()
		{
			var builder = new BuilderRequest(this);
			builder.CreateRequest();
			builder.BuildRequest();
			var res = builder.GetResponse();

			return new ResponseResult<string>
			{
				Succeed = res.Success,
				StatusCode = res.StatusCode,
				Content = res.ResponseContent
			};

		}

		public ResponseResult<T> RresponseValue<T>()
		{
			var builder = new BuilderRequest(this);
			builder.CreateRequest();
			builder.BuildRequest();
			var res = builder.GetResponse();
			return new ResponseResult<T>
			{
				Succeed = res.Success,
				StatusCode = res.StatusCode,
				Content = JsonConvert.DeserializeObject<T>(res.ResponseContent)
			};
		}
	}
}