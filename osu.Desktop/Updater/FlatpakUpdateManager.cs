// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using System;
using osu.Framework.Logging;
using System.Threading.Tasks;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Platform;
using osu.Game.Online.API;
using osu.Game.Overlays.Notifications;
using Flatpak.DBus;
using Tmds.DBus;

namespace osu.Game.Updater
{
    /// <summary>
    /// An update manager that shows notifications if a newer release is detected.
    /// This is a case where updates are handled externally by a package manager or other means, so no action is performed on clicking the notification.
    /// </summary>

    public class FlatpakUpdateManager : osu.Game.Updater.UpdateManager
    {
        private string version;
        private static readonly Logger logger = Logger.GetLogger("updater");

        [BackgroundDependencyLoader]
        private void load(OsuGameBase game)
        {
            version = game.Version;
        }

        protected override async Task<bool> PerformUpdateCheck()
        {
            try
            {
                Logger.Log("Starting updater", LoggingTarget.Runtime, LogLevel.Important);
                var sessionConnection = Connection.Session;
                Logger.Log("sessionConnection created", LoggingTarget.Runtime, LogLevel.Important);
                var flatpak = sessionConnection.CreateProxy<IFlatpak>("org.freedesktop.portal.Flatpak",
                                                                               "/org/freedesktop/portal/Flatpak");
                Logger.Log("flatpak proxy created", LoggingTarget.Runtime, LogLevel.Important);
                var props = await flatpak.GetAllAsync().ConfigureAwait(false);
                Logger.Log("props got", LoggingTarget.Runtime, LogLevel.Important);
                string myVersion = props.Version.ToString();

                Notifications.Post(new SimpleNotification
                {
                    Text = $"A newer release of osu! has been found ({version} → {myVersion}).\n\n"
                               + "Check with your package manager / provider to bring osu! up-to-date!",
                    Icon = FontAwesome.Solid.Upload,
                });
                return true;
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString(), LoggingTarget.Runtime, LogLevel.Important);
                // we shouldn't crash on a web failure. or any failure for the matter.
                return true;
            }

            //return false;
        }
    }
}
