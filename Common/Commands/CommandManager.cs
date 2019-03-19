﻿using System;
using System.Collections.Generic;
using System.Linq;
using Common.Interfaces;

namespace Common.Commands
{
    public class CommandManager
    {
        public static Dictionary<string, HandleCommand> CommandHandlers;

        public delegate void HandleCommand(IWorldManager manager, string[] args);

        static CommandManager()
        {
            CommandHandlers = new Dictionary<string, HandleCommand>(StringComparer.OrdinalIgnoreCase);

            DefineCommand("gps", Commands.Gps);
            DefineCommand("help", Commands.Help);
            DefineCommand("speed", Commands.Speed);
            DefineCommand("go", Commands.Go);
            DefineCommand("nudge", Commands.Nudge);
            DefineCommand("morph", Commands.Morph);
            DefineCommand("demorph", Commands.Demorph);
        }

        public static void DefineCommand(string command, HandleCommand handler) => CommandHandlers[command.ToLower()] = handler;

        public static bool InvokeHandler(string command, IWorldManager manager)
        {
            if (string.IsNullOrEmpty(command))
                return false;

            if (command[0] != '.')
                return false;

            string[] lines = command.Split(' ');
            return InvokeHandler(lines[0], manager, lines.Skip(1).ToArray());
        }

        public static bool InvokeHandler(string command, IWorldManager manager, params string[] args)
        {
            command = command.TrimStart('.').Trim(); // Remove command "." prefix and format

            if (CommandHandlers.TryGetValue(command, out var handle))
            {
                handle.Invoke(manager, args);
                return true;
            }

            return false;
        }
    }
}
