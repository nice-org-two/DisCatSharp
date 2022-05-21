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

namespace DisCatSharp.ApplicationCommands
{
	/// <summary>
	/// Marks this class a slash command group
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class SlashCommandGroupAttribute : Attribute
	{
		/// <summary>
		/// Gets the name of this slash command group
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets the description of this slash command group
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Gets the needed permission of this slash command group
		/// </summary>
		public Permissions? DefaultMemberPermissions { get; set; }

		/// <summary>
		/// Gets the dm permission of this slash command group
		/// </summary>
		public bool? DmPermission { get; set; }
		
		/// <summary>
		/// Gets whether this command is marked as NSFW
		/// </summary>
		public bool IsNsfw { get; set; }

		/// <summary>
		/// Marks this class as a slash command group
		/// </summary>
		/// <param name="name">The name of this slash command group.</param>
		/// <param name="description">The description of this slash command group.</param>
		/// <param name="isNsfw">Whether this command is marked as NSFW.</param>
		public SlashCommandGroupAttribute(string name, string description, bool isNsfw = false)
		{
			this.Name = name.ToLower();
			this.Description = description;
			this.DefaultMemberPermissions = null;
			this.DmPermission = null;
			this.IsNsfw = isNsfw;
		}

		/// <summary>
		/// Marks this method as a slash command group
		/// </summary>
		/// <param name="name">The name of this slash command.</param>
		/// <param name="description">The description of this slash command.</param>
		/// <param name="defaultMemberPermissions">The default member permissions.</param>
		/// <param name="isNsfw">Whether this command is marked as NSFW.</param>
		public SlashCommandGroupAttribute(string name, string description, long defaultMemberPermissions, bool isNsfw = false)
		{
			this.Name = name.ToLower();
			this.Description = description;
			this.DefaultMemberPermissions = (Permissions)defaultMemberPermissions;
			this.DmPermission = null;
			this.IsNsfw = isNsfw;
		}

		/// <summary>
		/// Marks this method as a slash command group
		/// </summary>
		/// <param name="name">The name of this slash command.</param>
		/// <param name="description">The description of this slash command.</param>
		/// <param name="dmPermission">The dm permission.</param>
		/// <param name="isNsfw">Whether this command is marked as NSFW.</param>
		public SlashCommandGroupAttribute(string name, string description, bool dmPermission, bool isNsfw = false)
		{
			this.Name = name.ToLower();
			this.Description = description;
			this.DefaultMemberPermissions = null;
			this.DmPermission = dmPermission;
			this.IsNsfw = isNsfw;
		}

		/// <summary>
		/// Marks this method as a slash command group
		/// </summary>
		/// <param name="name">The name of this slash command.</param>
		/// <param name="description">The description of this slash command.</param>
		/// <param name="defaultMemberPermissions">The default member permissions.</param>
		/// <param name="dmPermission">The dm permission.</param>
		/// <param name="isNsfw">Whether this command is marked as NSFW.</param>
		public SlashCommandGroupAttribute(string name, string description, long defaultMemberPermissions, bool dmPermission, bool isNsfw = false)
		{
			this.Name = name.ToLower();
			this.Description = description;
			this.DefaultMemberPermissions = (Permissions)defaultMemberPermissions;
			this.DmPermission = dmPermission;
			this.IsNsfw = isNsfw;
		}
	}
}
