/*
 * drive_monitor.exe written by Nicholas Flesch
 * nicholas.flesch@outlook.com
 * last modified: 15 October 2020
 * reports on free/used space of local computers fixed harddrives
 * can display report in console or send an email if configured.
 * can email an low free space alert if email settings are configured.
 * requires .NET Core 3.1.
*/

using System;
using System.Collections.Generic;

namespace drive_monitor
{
    class Program
    {
        static void Main(string[] args)
        {
            Options options = new Options(args);
        }
    }
}