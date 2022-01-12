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

using System.Threading.Tasks;

namespace DisCatSharp.ApplicationCommands
{
	/// <summary>
	/// Represents a base class for application command modules
	/// </summary>
	public abstract class ApplicationCommandsModule
	{
		/// <summary>
		/// Called before the execution of a slash command in the module.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <returns> Whether or not to execute the slash command.</returns>
		public virtual Task<bool> BeforeSlashExecutionAsync(InteractionContext ctx)
			=> Task.FromResult(true);

		/// <summary>
		/// Called after the execution of a slash command in the module.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <returns></returns>
		public virtual Task AfterSlashExecutionAsync(InteractionContext ctx)
			=> Task.CompletedTask;

		/// <summary>
		/// Called before the execution of a context menu in the module.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <returns> Whether or not to execute the slash command. </returns>
		public virtual Task<bool> BeforeContextMenuExecutionAsync(ContextMenuContext ctx)
			=> Task.FromResult(true);

		/// <summary>
		/// Called after the execution of a context menu in the module.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <returns></returns>
		public virtual Task AfterContextMenuExecutionAsync(ContextMenuContext ctx)
			=> Task.CompletedTask;
	}
}
