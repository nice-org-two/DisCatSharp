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
using System.Linq;

namespace DisCatSharp.Entities
{
	/// <summary>
	/// Constructs an interaction modal response.
	/// </summary>
	public sealed class DiscordInteractionModalBuilder
	{
		/// <summary>
		/// Title of modal.
		/// </summary>
		public string Title
		{
			get => this._title;
			set
			{
				if (value != null && value.Length > 128)
					throw new ArgumentException("Title length cannot exceed 128 characters.", nameof(value));
				this._title = value;
			}
		}
		private string _title;

		/// <summary>
		/// Custom id of modal.
		/// </summary>
		public string CustomId { get; set; }

		/// <summary>
		/// Components to send on this interaction response.
		/// </summary>
		public IReadOnlyList<DiscordActionRowComponent> ModalComponents => this._components;
		private readonly List<DiscordActionRowComponent> _components = new();

		/// <summary>
		/// Constructs a new empty interaction modal builder.
		/// </summary>
		public DiscordInteractionModalBuilder() { }

		public DiscordInteractionModalBuilder WithTitle(string title)
		{
			this.Title = title;
			return this;
		}

		public DiscordInteractionModalBuilder WithCustomId(string customId)
		{
			this.CustomId = customId;
			return this;
		}

		/// <summary>
		/// Appends a collection of components to the builder. Each call will append to a new row.
		/// </summary>
		/// <param name="components">The components to append. Up to five.</param>
		/// <returns>The current builder to chain calls with.</returns>
		/// <exception cref="System.ArgumentException">Thrown when passing more than 5 components.</exception>
		public DiscordInteractionModalBuilder AddModalComponents(params DiscordTextComponent[] components)
			=> this.AddModalComponents((IEnumerable<DiscordTextComponent>)components);

		/// <summary>
		/// Appends a text component to the builder.
		/// </summary>
		/// <param name="component">The component to append.</param>
		/// <returns>The current builder to chain calls with.</returns>
		public DiscordInteractionModalBuilder AddTextComponent(DiscordTextComponent component)
		{
			List<DiscordTextComponent> comp = new(1)
			{
				component
			};

			return this.AddModalComponents(comp);
		}

		/// <summary>
		/// Appends several rows of components to the message
		/// </summary>
		/// <param name="components">The rows of components to add, holding up to five each.</param>
		/// <returns></returns>
		public DiscordInteractionModalBuilder AddModalComponents(IEnumerable<DiscordActionRowComponent> components)
		{
			var ara = components.ToArray();

			if (ara.Length + this._components.Count > 5)
				throw new ArgumentException("ActionRow count exceeds maximum of five.");

			foreach (var ar in ara)
				this._components.Add(ar);

			return this;
		}

		/// <summary>
		/// Appends a collection of components to the builder. Each call will append to a new row.
		/// If you add a <see cref="DiscordTextComponent"></see> you can only add one.
		/// </summary>
		/// <param name="components">The components to append. Up to five.</param>
		/// <returns>The current builder to chain calls with.</returns>
		/// <exception cref="System.ArgumentException">Thrown when passing more than 5 components.</exception>
		public DiscordInteractionModalBuilder AddModalComponents(IEnumerable<DiscordTextComponent> components)
		{
			var compArr = components.ToArray();
			var count = compArr.Length;

			if (count > 5)
				throw new ArgumentException("Cannot add more than 5 components per action row!");

			if (components.Where(c => c.Type == Enums.ComponentType.InputText).Any() && count < 1)
				throw new ArgumentException("Cannot add more than 1 text components per action row!");

			var arc = new DiscordActionRowComponent(compArr);
			this._components.Add(arc);
			return this;
		}

		/// <summary>
		/// Clears all message components on this builder.
		/// </summary>
		public void ClearComponents()
			=> this._components.Clear();

		/// <summary>
		/// Allows for clearing the Interaction Response Builder so that it can be used again to send a new response.
		/// </summary>
		public void Clear()
		{
			this._components.Clear();
			this.Title = null;
			this.CustomId = null;
		}
	}
}
