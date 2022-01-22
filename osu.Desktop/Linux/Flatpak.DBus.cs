using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Tmds.DBus;

[assembly: InternalsVisibleTo(Tmds.DBus.Connection.DynamicAssemblyName)]
namespace Flatpak.DBus
{
    [DBusInterface("org.freedesktop.portal.Flatpak")]
    interface IFlatpak : IDBusObject
    {
        Task<uint> SpawnAsync(byte[] CwdPath, byte[][] Argv, IDictionary<uint, CloseSafeHandle> Fds, IDictionary<string, string> Envs, uint Flags, IDictionary<string, object> Options);
        Task SpawnSignalAsync(uint Pid, uint Signal, bool ToProcessGroup);
        Task<ObjectPath> CreateUpdateMonitorAsync(IDictionary<string, object> Options);
        Task<IDisposable> WatchSpawnStartedAsync(Action<(uint pid, uint relpid)> handler, Action<Exception> onError = null);
        Task<IDisposable> WatchSpawnExitedAsync(Action<(uint pid, uint exitStatus)> handler, Action<Exception> onError = null);
        Task<T> GetAsync<T>(string prop);
        Task<FlatpakProperties> GetAllAsync();
        Task SetAsync(string prop, object val);
        Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
    }

    [Dictionary]
    class FlatpakProperties
    {
        private uint _version = default(uint);
        public uint Version
        {
            get
            {
                return _version;
            }

            set
            {
                _version = (value);
            }
        }

        private uint _supports = default(uint);
        public uint Supports
        {
            get
            {
                return _supports;
            }

            set
            {
                _supports = (value);
            }
        }
    }

    static class FlatpakExtensions
    {
        public static Task<uint> GetVersionAsync(this IFlatpak o) => o.GetAsync<uint>("version");
        public static Task<uint> GetSupportsAsync(this IFlatpak o) => o.GetAsync<uint>("supports");
    }
}