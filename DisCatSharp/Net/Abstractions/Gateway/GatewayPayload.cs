// This file is part of the DisCatSharp project, based off DSharpPlus.
//
// Copyright (c) 2021-2022 AITSYS
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using Newtonsoft.Json;

namespace DisCatSharp.Net.Abstractions
{
	/// <summary>
	/// Represents a websocket payload exchanged between Discord and the client.
	/// </summary>
	internal sealed class GatewayPayload
	{
		/// <summary>
		/// Gets or sets the OP code of the payload.
		/// </summary>
		[JsonProperty("op")]
		public GatewayOpCode OpCode { get; set; }

		/// <summary>
		/// Gets or sets the data of the payload.
		/// </summary>
		[JsonProperty("d")]
		public object Data { get; set; }

		/// <summary>
		/// Gets or sets the sequence number of the payload. Only present for OP 0.
		/// </summary>
		[JsonProperty("s", NullValueHandling = NullValueHandling.Ignore)]
		public int? Sequence { get; set; }

		/// <summary>
		/// Gets or sets the event name of the payload. Only present for OP 0.
		/// </summary>
		[JsonProperty("t", NullValueHandling = NullValueHandling.Ignore)]
		public string EventName { get; set; }
	}
}
