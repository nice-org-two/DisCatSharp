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

namespace DisCatSharp
{
	/// <summary>
	/// Represents additional details of a users account.
	/// </summary>
	[Flags]
	public enum UserFlags
	{
		/// <summary>
		/// The user has no flags.
		/// </summary>
		None = 0,

		/// <summary>
		/// The user is a Discord employee.
		/// </summary>
		Staff = 1 << 0,

		/// <summary>
		/// The user is a Discord partner.
		/// </summary>
		Partner = 1 << 1,

		/// <summary>
		/// The user has the HypeSquad badge.
		/// </summary>
		HypeSquad = 1 << 2,

		/// <summary>
		/// The user reached the first bug hunter tier.
		/// </summary>
		BugHunterLevelOne = 1 << 3,

		/// <summary>
		/// The user has SMS recovery for 2FA enabled.
		/// </summary>
		MfaSms = 1 << 4,

		/// <summary>
		/// The user is marked as dismissed Nitro promotion
		/// </summary>
		PremiumPromoDismissed = 1 << 5,

		/// <summary>
		/// The user is a member of house bravery.
		/// </summary>
		HouseBravery = 1 << 6,

		/// <summary>
		/// The user is a member of house brilliance.
		/// </summary>
		HouseBrilliance = 1 << 7,

		/// <summary>
		/// The user is a member of house balance.
		/// </summary>
		HouseBalance = 1 << 8,

		/// <summary>
		/// The user has the early supporter badge.
		/// </summary>
		PremiumEarlySupporter = 1 << 9,

		/// <summary>
		/// User is a <see cref="Entities.DiscordTeam"/>.
		/// </summary>
		TeamPseudoUser = 1 << 10,

		/// <summary>
		/// Relates to partner/verification applications.
		/// </summary>
		PartnerOrVerificationApplication = 1 << 11,

		/// <summary>
		/// Whether the user is an official system user.
		/// </summary>
		System = 1 << 12,

		/// <summary>
		/// Whether the user has unread system messages.
		/// </summary>
		HasUnreadUrgentMessages = 1 << 13,

		/// <summary>
		/// The user reached the second bug hunter tier.
		/// </summary>
		BugHunterLevelTwo = 1 << 14,

		/// <summary>
		/// The user has a pending deletion for being underage in DOB prompt.
		/// </summary>
		UnderageDeleted = 1 << 15,

		/// <summary>
		/// The user is a verified bot.
		/// </summary>
		VerifiedBot = 1 << 16,

		/// <summary>
		/// The user is a verified bot developer.
		/// </summary>
		VerifiedDeveloper = 1 << 17,

		/// <summary>
		/// The user is a discord certified moderator.
		/// </summary>
		CertifiedModerator = 1 << 18,

		/// <summary>
		/// The user is a bot and has set an interactions endpoint url.
		/// </summary>
		BotHttpInteractions = 1 << 19,

		/// <summary>
		/// The user is disabled for being a spammer.
		/// </summary>
		Spammer = 1 << 20
	}
}
