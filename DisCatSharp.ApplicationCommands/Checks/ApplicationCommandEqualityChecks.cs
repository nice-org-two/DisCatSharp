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
using System.Linq;

using DisCatSharp.Entities;
using DisCatSharp.Enums;
using DisCatSharp.Common;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DisCatSharp.ApplicationCommands
{
	internal static class ApplicationCommandEqualityChecks
	{
		/// <summary>
		/// Whether two application commands are equal.
		/// </summary>
		/// <param name="ac1">Source command.</param>
		/// <param name="targetApplicationCommand">Command to check agains.</param>
		internal static bool IsEqualTo(this DiscordApplicationCommand ac1, DiscordApplicationCommand targetApplicationCommand)
		{
			if (targetApplicationCommand is null || ac1 is null)
				return false;

			DiscordApplicationCommand sourceApplicationCommand = new(
				ac1.Name, ac1.Description, ac1.Options,
				ac1.DefaultPermission, ac1.Type,
				ac1.NameLocalizations, ac1.DescriptionLocalizations
			);

			ApplicationCommandsExtension.ClientInternal.Logger.Log(ApplicationCommandsExtension.ApplicationCommandsLogLevel, $"[AC Change Check] Command {ac1.Name}\n\n[{JsonConvert.SerializeObject(sourceApplicationCommand)},{JsonConvert.SerializeObject(targetApplicationCommand)}]\n\n");

			return ac1.Type == targetApplicationCommand.Type && sourceApplicationCommand.SoftEqual(targetApplicationCommand, ac1.Type);
		}

		/// <summary>
		/// Checks if two <see cref="DisCatSharp.Entities.DiscordApplicationCommand"/>s are the same.
		/// Excluding id, application id and version here.
		/// </summary>
		/// <param name="source">Source application command.</param>
		/// <param name="target">Application command to check against.</param>
		/// <param name="type">The application command type.</param>
		internal static bool SoftEqual(this DiscordApplicationCommand source, DiscordApplicationCommand target, ApplicationCommandType type)
		{
			return type switch {
				ApplicationCommandType.ChatInput => DeepEqual(source, target),
				_ => (source.Name == target.Name) && (source.DefaultPermission == target.DefaultPermission)
			&& (source.Type == target.Type)
			};
		}

		internal static bool DeepEqual(DiscordApplicationCommand source, DiscordApplicationCommand target)
		{
			var rootCheck = (source.Name == target.Name) && (source.Description == target.Description) && (source.DefaultPermission == target.DefaultPermission) && (source.Type == target.Type);

			if (source.Options == null && target.Options == null)
				return rootCheck;
			else if ((source.Options != null && target.Options == null) || (source.Options == null && target.Options != null))
				return false;
			else if (source.Options.Any(o => o.Type == ApplicationCommandOptionType.SubCommandGroup) && target.Options.Any(o => o.Type == ApplicationCommandOptionType.SubCommandGroup))
			{
				List<DiscordApplicationCommandOption> minimalSourceOptions = new();
				List<DiscordApplicationCommandOption> minimalTargetOptions = new();

				foreach (var option in source.Options)
				{
					List<DiscordApplicationCommandOption> minimalSubSourceOptions = new();

					foreach (var subOption in option.Options)
					{
						List<DiscordApplicationCommandOption> minimalSubSubSourceOptions = null;

						if (subOption.Options != null)
						{
							minimalSubSubSourceOptions = new();

							foreach (var subSubOption in subOption.Options)
							{
								minimalSubSubSourceOptions.Add(new DiscordApplicationCommandOption(
									subSubOption.Name, subSubOption.Description, subSubOption.Type, subSubOption.Required ?? false,
									subSubOption.Choices, null, subSubOption.ChannelTypes, subSubOption.AutoComplete ?? false,
									subSubOption.MinimumValue, subSubOption.MaximumValue, null, null
								));
							}

							minimalSubSourceOptions.Add(new DiscordApplicationCommandOption(
								subOption.Name, subOption.Description, subOption.Type, null, null,
								minimalSubSubSourceOptions, null, null, null, null, null, null
							));
						}

					}

					minimalSourceOptions.Add(new DiscordApplicationCommandOption(
						option.Name, option.Description, option.Type, null, null,
						minimalSubSourceOptions, null, null, null, null, null, null
					));
				}

				foreach (var option in target.Options)
				{
					List<DiscordApplicationCommandOption> minimalSubTargetOptions = new();

					foreach (var subOption in option.Options)
					{
						List<DiscordApplicationCommandOption> minimalSubSubTargetOptions = null;

						if (subOption.Options != null && subOption.Options.Any())
						{
							minimalSubSubTargetOptions = new();

							foreach (var subSubOption in subOption.Options)
							{
								minimalSubSubTargetOptions.Add(new DiscordApplicationCommandOption(
									subSubOption.Name, subSubOption.Description, subSubOption.Type, subSubOption.Required ?? false,
									subSubOption.Choices, null, subSubOption.ChannelTypes, subSubOption.AutoComplete ?? false,
									subSubOption.MinimumValue, subSubOption.MaximumValue, null, null
								));
							}

							minimalSubTargetOptions.Add(new DiscordApplicationCommandOption(
								subOption.Name, subOption.Description, subOption.Type, null, null,
								minimalSubSubTargetOptions, null, null, null, null, null, null
							));
						}
					}

					minimalTargetOptions.Add(new DiscordApplicationCommandOption(
						option.Name, option.Description, option.Type, null, null,
						minimalSubTargetOptions, null, null, null, null, null, null
					));
				}

				return rootCheck && JsonConvert.SerializeObject(minimalSourceOptions) == JsonConvert.SerializeObject(minimalTargetOptions);
			}
			else if (source.Options.Any(o => o.Type == ApplicationCommandOptionType.SubCommand) && target.Options.Any(o => o.Type == ApplicationCommandOptionType.SubCommand))
			{
				List<DiscordApplicationCommandOption> minimalSourceOptions = new();
				List<DiscordApplicationCommandOption> minimalTargetOptions = new();

				foreach (var option in source.Options)
				{
					List<DiscordApplicationCommandOption> minimalSubSourceOptions =null;

					if (option.Options != null)
					{
						minimalSubSourceOptions = new();

						foreach (var subOption in option.Options)
						{
							minimalSubSourceOptions.Add(new DiscordApplicationCommandOption(
								subOption.Name, subOption.Description, subOption.Type, subOption.Required ?? false,
								subOption.Choices, null, subOption.ChannelTypes, subOption.AutoComplete ?? false,
								subOption.MinimumValue, subOption.MaximumValue, null, null
							));
						}
					}

					minimalSourceOptions.Add(new DiscordApplicationCommandOption(
						option.Name, option.Description, option.Type, null, null,
						minimalSubSourceOptions, null, null, null, null, null, null
					));
				}

				foreach (var option in target.Options)
				{
					List<DiscordApplicationCommandOption> minimalSubTargetOptions = null;

					if (option.Options != null && option.Options.Any())
					{
						minimalSubTargetOptions = new();

						foreach (var subOption in option.Options)
						{
							minimalSubTargetOptions.Add(new DiscordApplicationCommandOption(
								subOption.Name, subOption.Description, subOption.Type, subOption.Required ?? false,
								subOption.Choices, null, subOption.ChannelTypes, subOption.AutoComplete ?? false,
								subOption.MinimumValue, subOption.MaximumValue, null, null
							));
						}
					}

					minimalTargetOptions.Add(new DiscordApplicationCommandOption(
						option.Name, option.Description, option.Type, null, null,
						minimalSubTargetOptions, null, null, null, null, null, null
					));
				}

				return rootCheck && JsonConvert.SerializeObject(minimalSourceOptions) == JsonConvert.SerializeObject(minimalTargetOptions);
			}
			else
			{
				List<DiscordApplicationCommandOption> minimalSourceOptions = new();
				List<DiscordApplicationCommandOption> minimalTargetOptions = new();

				foreach (var option in source.Options)
					minimalSourceOptions.Add(new DiscordApplicationCommandOption(
						option.Name, option.Description, option.Type, option.Required ?? false,
						option.Choices, null, option.ChannelTypes, option.AutoComplete ?? false, option.MinimumValue, option.MaximumValue,
						null, null
					));

				foreach (var option in target.Options)
					minimalTargetOptions.Add(new DiscordApplicationCommandOption(
						option.Name, option.Description, option.Type, option.Required ?? false,
						option.Choices, null, option.ChannelTypes, option.AutoComplete ?? false, option.MinimumValue, option.MaximumValue,
						null, null
					));

				return rootCheck && JsonConvert.SerializeObject(minimalSourceOptions) == JsonConvert.SerializeObject(minimalTargetOptions);
			}
		}

		// TODO: Later
		//&& (source.NameLocalizations == target.NameLocalizations) && (source.DescriptionLocalizations == target.DescriptionLocalizations)
		// && (source.Permission == other.Permission) && (source.DmPermission == other.DmPermission)
	}
}
