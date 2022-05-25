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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using ConcurrentCollections;

using DisCatSharp.Common.Utilities;
using DisCatSharp.Entities;
using DisCatSharp.EventArgs;

using Microsoft.Extensions.Logging;

namespace DisCatSharp.Interactivity.EventHandling
{
	/// <summary>
	/// EventWaiter is a class that serves as a layer between the InteractivityExtension
	/// and the DiscordClient to listen to an event and check for matches to a predicate.
	/// </summary>
	internal class ReactionCollector : IDisposable
	{
		DiscordClient _client;

		AsyncEvent<DiscordClient, MessageReactionAddEventArgs> _reactionAddEvent;
		AsyncEventHandler<DiscordClient, MessageReactionAddEventArgs> _reactionAddHandler;

		AsyncEvent<DiscordClient, MessageReactionRemoveEventArgs> _reactionRemoveEvent;
		AsyncEventHandler<DiscordClient, MessageReactionRemoveEventArgs> _reactionRemoveHandler;

		AsyncEvent<DiscordClient, MessageReactionsClearEventArgs> _reactionClearEvent;
		AsyncEventHandler<DiscordClient, MessageReactionsClearEventArgs> _reactionClearHandler;

		ConcurrentHashSet<ReactionCollectRequest> _requests;

		/// <summary>
		/// Creates a new EventWaiter object.
		/// </summary>
		/// <param name="client">Your DiscordClient</param>
		public ReactionCollector(DiscordClient client)
		{
			this._client = client;
			var tinfo = this._client.GetType().GetTypeInfo();

			this._requests = new ConcurrentHashSet<ReactionCollectRequest>();

			// Grabbing all three events from client
			var handler = tinfo.DeclaredFields.First(x => x.FieldType == typeof(AsyncEvent<DiscordClient, MessageReactionAddEventArgs>));

			this._reactionAddEvent = (AsyncEvent<DiscordClient, MessageReactionAddEventArgs>)handler.GetValue(this._client);
			this._reactionAddHandler = new AsyncEventHandler<DiscordClient, MessageReactionAddEventArgs>(this.HandleReactionAdd);
			this._reactionAddEvent.Register(this._reactionAddHandler);

			handler = tinfo.DeclaredFields.First(x => x.FieldType == typeof(AsyncEvent<DiscordClient, MessageReactionRemoveEventArgs>));

			this._reactionRemoveEvent = (AsyncEvent<DiscordClient, MessageReactionRemoveEventArgs>)handler.GetValue(this._client);
			this._reactionRemoveHandler = new AsyncEventHandler<DiscordClient, MessageReactionRemoveEventArgs>(this.HandleReactionRemove);
			this._reactionRemoveEvent.Register(this._reactionRemoveHandler);

			handler = tinfo.DeclaredFields.First(x => x.FieldType == typeof(AsyncEvent<DiscordClient, MessageReactionsClearEventArgs>));

			this._reactionClearEvent = (AsyncEvent<DiscordClient, MessageReactionsClearEventArgs>)handler.GetValue(this._client);
			this._reactionClearHandler = new AsyncEventHandler<DiscordClient, MessageReactionsClearEventArgs>(this.HandleReactionClear);
			this._reactionClearEvent.Register(this._reactionClearHandler);
		}

		/// <summary>
		/// Collects the async.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns>A Task.</returns>
		public async Task<ReadOnlyCollection<Reaction>> CollectAsync(ReactionCollectRequest request)
		{
			this._requests.Add(request);
			var result = (ReadOnlyCollection<Reaction>)null;

			try
			{
				await request.Tcs.Task.ConfigureAwait(false);
			}
			catch (Exception ex)
			{
				this._client.Logger.LogError(InteractivityEvents.InteractivityCollectorError, ex, "Exception occurred while collecting reactions");
			}
			finally
			{
				result = new ReadOnlyCollection<Reaction>(new HashSet<Reaction>(request.Collected).ToList());
				request.Dispose();
				this._requests.TryRemove(request);
			}
			return result;
		}

		/// <summary>
		/// Handles the reaction add.
		/// </summary>
		/// <param name="client">The client.</param>
		/// <param name="eventArgs">The event's arguments.</param>
		/// <returns>A Task.</returns>
		private Task HandleReactionAdd(DiscordClient client, MessageReactionAddEventArgs eventArgs)
		{
			// foreach request add
			foreach (var req in this._requests)
			{
				if (req.Message.Id == eventArgs.Message.Id)
				{
					if (req.Collected.Any(x => x.Emoji == eventArgs.Emoji && x.Users.Any(y => y.Id == eventArgs.User.Id)))
					{
						var reaction = req.Collected.First(x => x.Emoji == eventArgs.Emoji && x.Users.Any(y => y.Id == eventArgs.User.Id));
						req.Collected.TryRemove(reaction);
						reaction.Users.Add(eventArgs.User);
						req.Collected.Add(reaction);
					}
					else
					{
						req.Collected.Add(new Reaction()
						{
							Emoji = eventArgs.Emoji,
							Users = new ConcurrentHashSet<DiscordUser>() { eventArgs.User }
						});
					}
				}
			}
			return Task.CompletedTask;
		}

		/// <summary>
		/// Handles the reaction remove.
		/// </summary>
		/// <param name="client">The client.</param>
		/// <param name="eventArgs">The event's arguments.</param>
		/// <returns>A Task.</returns>
		private Task HandleReactionRemove(DiscordClient client, MessageReactionRemoveEventArgs eventArgs)
		{
			// foreach request remove
			foreach (var req in this._requests)
			{
				if (req.Message.Id == eventArgs.Message.Id)
				{
					if (req.Collected.Any(x => x.Emoji == eventArgs.Emoji && x.Users.Any(y => y.Id == eventArgs.User.Id)))
					{
						var reaction = req.Collected.First(x => x.Emoji == eventArgs.Emoji && x.Users.Any(y => y.Id == eventArgs.User.Id));
						req.Collected.TryRemove(reaction);
						reaction.Users.TryRemove(eventArgs.User);
						if (reaction.Users.Count > 0)
							req.Collected.Add(reaction);
					}
				}
			}
			return Task.CompletedTask;
		}

		/// <summary>
		/// Handles the reaction clear.
		/// </summary>
		/// <param name="client">The client.</param>
		/// <param name="eventArgs">The event's arguments.</param>
		/// <returns>A Task.</returns>
		private Task HandleReactionClear(DiscordClient client, MessageReactionsClearEventArgs eventArgs)
		{
			// foreach request add
			foreach (var req in this._requests)
			{
				if (req.Message.Id == eventArgs.Message.Id)
				{
					req.Collected.Clear();
				}
			}
			return Task.CompletedTask;
		}

		~ReactionCollector()
		{
			this.Dispose();
		}

		/// <summary>
		/// Disposes this EventWaiter
		/// </summary>
		public void Dispose()
		{
			this._client = null;

			this._reactionAddEvent.Unregister(this._reactionAddHandler);
			this._reactionRemoveEvent.Unregister(this._reactionRemoveHandler);
			this._reactionClearEvent.Unregister(this._reactionClearHandler);

			this._reactionAddEvent = null;
			this._reactionAddHandler = null;
			this._reactionRemoveEvent = null;
			this._reactionRemoveHandler = null;
			this._reactionClearEvent = null;
			this._reactionClearHandler = null;

			this._requests.Clear();
			this._requests = null;
			GC.SuppressFinalize(this);
		}
	}

	/// <summary>
	/// The reaction collect request.
	/// </summary>
	public class ReactionCollectRequest : IDisposable
	{
		internal TaskCompletionSource<Reaction> Tcs;
		internal CancellationTokenSource Ct;
		internal TimeSpan Timeout;
		internal DiscordMessage Message;
		internal ConcurrentHashSet<Reaction> Collected;

		/// <summary>
		/// Initializes a new instance of the <see cref="ReactionCollectRequest"/> class.
		/// </summary>
		/// <param name="msg">The msg.</param>
		/// <param name="timeout">The timeout.</param>
		public ReactionCollectRequest(DiscordMessage msg, TimeSpan timeout)
		{
			this.Message = msg;
			this.Collected = new ConcurrentHashSet<Reaction>();
			this.Timeout = timeout;
			this.Tcs = new TaskCompletionSource<Reaction>();
			this.Ct = new CancellationTokenSource(this.Timeout);
			this.Ct.Token.Register(() => this.Tcs.TrySetResult(null));
		}

		~ReactionCollectRequest()
		{
			this.Dispose();
		}

		/// <summary>
		/// Disposes the.
		/// </summary>
		public void Dispose()
		{
			GC.SuppressFinalize(this);
			this.Ct.Dispose();
			this.Tcs = null;
			this.Message = null;
			this.Collected?.Clear();
			this.Collected = null;
		}
	}

	/// <summary>
	/// The reaction.
	/// </summary>
	public class Reaction
	{
		/// <summary>
		/// Gets the emoji.
		/// </summary>
		public DiscordEmoji Emoji { get; internal set; }
		/// <summary>
		/// Gets the users.
		/// </summary>
		public ConcurrentHashSet<DiscordUser> Users { get; internal set; }
		/// <summary>
		/// Gets the total.
		/// </summary>
		public int Total => this.Users.Count;
	}
}
