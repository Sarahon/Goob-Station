// SPDX-FileCopyrightText: 2022 20kdc <asdd2808@gmail.com>
// SPDX-FileCopyrightText: 2024 Piras314 <p1r4s@proton.me>
// SPDX-FileCopyrightText: 2025 Aiden <28298836+Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.Network;

namespace Content.Client.Launcher;

/// <summary>
/// So apparently the way that disconnect information is shipped around is really indirect.
/// But honestly, given that content might have additional flags (i.e. hide disconnect button for bans)?
/// This is responsible for collecting any extended disconnect information.
/// </summary>
public sealed class ExtendedDisconnectInformationManager
{
    [Dependency] private readonly IClientNetManager _clientNetManager = default!;

    private NetDisconnectedArgs? _lastNetDisconnectedArgs = null;

    public NetDisconnectedArgs? LastNetDisconnectedArgs
    {
        get => _lastNetDisconnectedArgs;
        private set
        {
            _lastNetDisconnectedArgs = value;
            LastNetDisconnectedArgsChanged?.Invoke(value);
        }
    }

    // BE CAREFUL!
    // This may fire at an arbitrary time before or after whatever code that needs it.
    public event Action<NetDisconnectedArgs?>? LastNetDisconnectedArgsChanged;

    public void Initialize()
    {
        _clientNetManager.Disconnect += OnNetDisconnect;
    }

    private void OnNetDisconnect(object? sender, NetDisconnectedArgs args)
    {
        LastNetDisconnectedArgs = args;
    }
}
