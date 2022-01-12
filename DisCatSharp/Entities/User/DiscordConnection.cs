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

using System.Collections.Generic;

using Newtonsoft.Json;

namespace DisCatSharp.Entities
{
	/// <summary>
	/// Gets a Discord connection to a 3rd party service.
	/// </summary>
	public class DiscordConnection
	{
		/// <summary>
		/// Gets the id of the connection account
		/// </summary>
		[JsonProperty("id")]
		public string Id { get; internal set; }

		/// <summary>
		/// Gets the username of the connection account.
		/// </summary>
		[JsonProperty("name")]
		public string Name { get; internal set; }

		/// <summary>
		/// Gets the service of the connection (twitch, youtube, steam, twitter, facebook, spotify, leagueoflegends, reddit)
		/// </summary>
		[JsonProperty("type")]
		public string Type { get; internal set; }

		/// <summary>
		/// Gets whether the connection is revoked.
		/// </summary>
		[JsonProperty("revoked")]
		public bool IsRevoked { get; internal set; }

		/// <summary>
		/// Gets a collection of partial server integrations.
		/// </summary>
		[JsonProperty("integrations", NullValueHandling = NullValueHandling.Ignore)]
		public IReadOnlyList<DiscordIntegration> Integrations { get; internal set; }

		/// <summary>
		/// Gets whether the connection is verified.
		/// </summary>
		[JsonProperty("verified", NullValueHandling = NullValueHandling.Ignore)]
		public bool? Verified { get; internal set; }

		/// <summary>
		/// Gets whether the connection will show a activity.
		/// </summary>
		[JsonProperty("show_activity", NullValueHandling = NullValueHandling.Ignore)]
		public bool? ShowActivity { get; internal set; }

		/// <summary>
		/// Whether the connection will sync friends.
		/// </summary>
		[JsonProperty("friend_sync", NullValueHandling = NullValueHandling.Ignore)]
		public bool? FriendSync { get; internal set; }

		/// <summary>
		/// Gets the visibility of the connection.
		/// </summary>
		[JsonProperty("visibility", NullValueHandling = NullValueHandling.Ignore)]
		public long? Visibility { get; internal set; }

		/// <summary>
		/// Gets the client instance this object is tied to.
		/// </summary>
		[JsonIgnore]
		internal BaseDiscordClient Discord { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="DiscordConnection"/> class.
		/// </summary>
		internal DiscordConnection()
		{ }
	}
}
