using System;
using FlaxEngine;
using FlaxEngine.Json;
using Game.Systems.Serialization;

namespace Game.Behaviours.Player;

public sealed class PlayerData : Script, ISaveable
{
    [Serializable]
    public sealed class State
    {
        public Vector3 Position;
    }

    [ReadOnly] public PlayerData playerData;
    State pendingState;

    public string SaveKey => "player";

    public override void OnStart()
    {
        playerData = Actor.GetScript<PlayerData>();
        if (playerData == null)
        {
            Debug.LogError("PlayerSaveRegistrar: PlayerData not found on this actor.");
            return;
        }

        var manager = SerializerManager.Instance;
        if (manager == null)
        {
            Debug.LogError("PlayerSaveRegistrar: SerializerManager plugin not available.");
            return;
        }
        manager.Serializer.Register(playerData);
    }

    public override void OnUpdate()
    {
        if (pendingState == null) return;

        Actor.Position = pendingState.Position;
        pendingState = null;
    }

    public override void OnDestroy()
       => SerializerManager.Instance?.Serializer.Unregister(playerData?.SaveKey);

    public string CaptureState()
    {
        var s = new State
        {
            Position = Actor.Position,
        };
        return JsonSerializer.Serialize(s);
    }

    public void RestoreState(string state)
    {
        if (string.IsNullOrEmpty(state)) return;

        try
        {
            var s = JsonSerializer.Deserialize<State>(state);
            if (s == null) return;
            pendingState = s;
        }
        catch (Exception ex)
        {
            Debug.LogError($"PlayerData.RestoreState failed: {ex}");
        }
    }
}