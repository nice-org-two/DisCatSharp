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

namespace DisCatSharp.CommandsNext.Attributes
{
	/// <summary>
	/// Defines that a command is only usable within a guild which has enabled the member verification gate.
	/// </summary>
	public sealed class RequireMemberVerificationGateAttribute : CheckBaseAttribute
	{
		/// <summary>
		/// Defines that this command is only usable within guild which has enabled the member verification gate.
		/// </summary>
		public RequireMemberVerificationGateAttribute()
		{ }

		/// <summary>
		/// Executes the a check.
		/// </summary>
		/// <param name="ctx">The command context.</param>
		/// <param name="help">If true, help - returns true.</param>
		public override Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help) => Task.FromResult(ctx.Guild != null && ctx.Guild.HasMemberVerificationGate);
	}
}
