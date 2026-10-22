using System;
using FlaxEngine;
using Game.Core.Extensions;
using Game.Systems.Input;

namespace Game.Behaviours.Player;

public sealed class FirstPersonCameraStrategy : ICameraStrategy //FIXME:  Movement
{
    public JsonAssetReference<CameraPreset> PresetReference { get; set; }

    public void ApplyLook(
        InputService input, Actor cameraTarget, CameraData data, CameraPreset preset,
        Camera camera, PlayerCamera self)
    {
        Rotation(input, cameraTarget, data, preset);
        FOVZoom(input, data, preset, camera);
        ApplyShake(camera, preset, data);
    }

    static void Rotation(InputService input, Actor cameraTarget, CameraData data, CameraPreset preset)
    {
        var mouseDelta = new Float2(input.GetAxis(GameAxis.LookX), input.GetAxis(GameAxis.LookY));

        var ySign = preset.FPInvertY ? -1 : 1;

        var targetYaw = data.yaw + mouseDelta.X * preset.FPSensitivity;
        var targetPitch = data.pitch + mouseDelta.Y * preset.FPSensitivity * ySign;

        var t = 1 - Mathf.Exp(-preset.FPLookSmoothing * Time.DeltaTime);

        data.yaw = Mathf.LerpAngle(data.yaw, targetYaw, t);
        data.pitch = Mathf.Lerp(data.pitch, Mathf.Clamp(targetPitch, preset.FPPitch.X, preset.FPPitch.Y), t);

        cameraTarget.LocalOrientation = Quaternion.Euler(data.pitch, data.yaw, 0);
    }

    static void FOVZoom(InputService input, CameraData cameraData, CameraPreset preset, Camera camera)
    {
        if (!cameraData.initialized)
        {
            cameraData.defaultFov = camera.FieldOfView;
            cameraData.initialized = true;
        }

        var targetFov = input.GetAction(InputActions.Aim) ? preset.FPFOVZoom : cameraData.defaultFov;
        var t = preset.FPFOVLerpSpeed * Time.DeltaTime;
        camera.FieldOfView = camera.FieldOfView.Eerp(targetFov, t);
    }

    static void ApplyShake(Camera camera, CameraPreset preset, CameraData data)
    {
        if (data.shakeTimer <= 0 || data.shakeIntensity <= 0) return;

        data.shakeTimer -= Time.DeltaTime;
        var decay = Mathf.Exp(-preset.FPShakeDecay * Time.DeltaTime);
        data.shakeIntensity *= decay;

        if (data.shakeTimer <= 0 || data.shakeIntensity < 0.001f)
        {
            data.shakeIntensity = 0;
            data.shakeTimer = 0;
            return;
        }

        var t = Time.GameTime + data.shakeSeed;
        var offset = new Vector3(Mathf.Sin(t * 37.1f) * data.shakeIntensity, Mathf.Sin(t * 47.3f) * data.shakeIntensity, 0);

        camera.LocalPosition += offset * preset.FPShakeScale;
    }

    public void TriggerShake(CameraData data, float intensity, float duration)
    {
        data.shakeIntensity = Mathf.Max(data.shakeIntensity, intensity);
        data.shakeTimer = Mathf.Max(data.shakeTimer, duration);
        data.shakeSeed = (float)new Random().NextDouble() * 1000;
    }

    public void AddMovementRotation(CameraData data, float pitch, float yaw)
    {
        data.pitch += pitch;
        data.yaw += yaw;
    }
}