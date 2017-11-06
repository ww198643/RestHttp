﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using RestRequest.interfaces;

namespace RestRequest.Provider
{
	internal class TextBody : IBody
	{
		private string Content { get; }
		private Stream BodyStream { get; set; }

		private string ContentType { get; set; } = "application/text";

		public TextBody(string content)
		{
			Content = content;
		}
		public void SetContentType(string contenttype)
		{
			ContentType = contenttype;
		}

		public string GetContentType()
		{
			return ContentType;
		}

		public Stream GetBody()
		{
			BodyStream = new MemoryStream();
			var bytes = Encoding.UTF8.GetBytes(Content);
			BodyStream.Write(bytes, 0, bytes.Length);
			return BodyStream;
		}
	}
}