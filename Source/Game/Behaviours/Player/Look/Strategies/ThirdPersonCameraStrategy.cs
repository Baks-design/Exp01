using System;
using FlaxEngine;
using Game.Core.Extensions;
using Game.Systems.Input;

namespace Game.Behaviours.Player;

public sealed class ThirdPersonCameraStrategy : ICameraStrategy<ThirdPersonCameraPreset>  //FIXME:  Movement
{
    public JsonAssetReference<ThirdPersonCameraPreset> PresetReference { get; set; }

    public void ApplyLook(
        InputService input, Actor cameraTarget, CameraData data, ThirdPersonCameraPreset preset,
        Camera camera, PlayerCamera self)
    {
        Rotation(input, cameraTarget, data, preset, self);
        ApplySpringArm(camera, cameraTarget, data, preset);
        FOVZoom(input, data, preset, camera);
        ApplyShake(camera, preset, data);
    }

    static void Rotation(
        InputService input, Actor cameraTarget, CameraData data,
        ThirdPersonCameraPreset preset, PlayerCamera self)
    {
        if (self.lockOnTarget != null)
        {
            var toTarget = (self.lockOnTarget.Position - cameraTarget.Position).Normalized;
            float targetYaw = Mathf.Atan2(toTarget.X, toTarget.Z) * Mathf.RadiansToDegrees;
            float targetPitch = Mathf.Asin(Mathf.Clamp(-toTarget.Y, -1, 1)) * Mathf.RadiansToDegrees;

            var t = 1 - Mathf.Exp(-preset.lockOnLerpSpeed * Time.DeltaTime);
            data.yaw = Mathf.LerpAngle(data.yaw, targetYaw, t);
            data.pitch = Mathf.Lerp(data.pitch, Mathf.Clamp(targetPitch, preset.minMaxPitch.X, preset.minMaxPitch.Y), t);
        }
        else
        {
            var mouseDelta = new Float2(input.GetAxis(GameAxis.LookX), input.GetAxis(GameAxis.LookY));
            data.pitch = Mathf.Clamp(data.pitch + mouseDelta.Y * preset.sensitivity, preset.minMaxPitch.X, preset.minMaxPitch.Y);
            data.yaw += mouseDelta.X * preset.sensitivity;
        }

        cameraTarget.LocalOrientation = Quaternion.Euler(data.pitch, data.yaw, 0);
    }

    static void ApplySpringArm(Camera camera, Actor cameraTarget, CameraData data, ThirdPersonCameraPreset preset)
    {
        var targetPos = cameraTarget.Position;
        var lookRot = Quaternion.Euler(data.pitch, data.yaw, 0);
        var desiredDir = Vector3.Transform(Vector3.Backward, lookRot).Normalized;
        var desiredDistance = preset.distance;

        Vector3 basePosition;
        if (Physics.RayCast(targetPos, desiredDir, out var hit, desiredDistance, preset.cameraCollisionLayers, false))
        {
            var safeDistance = Mathf.Max(hit.Distance - preset.collisionBias - camera.NearPlane, preset.minDistance);
            basePosition = targetPos + desiredDir * safeDistance;
        }
        else
            basePosition = targetPos + desiredDir * desiredDistance;

        camera.Position = basePosition;
        camera.Orientation = Quaternion.LookRotation(-desiredDir, Vector3.Up);
    }

    static void FOVZoom(InputService input, CameraData cameraData, ThirdPersonCameraPreset preset, Camera camera)
    {
        if (!cameraData.initialized)
        {
            cameraData.defaultFov = camera.FieldOfView;
            cameraData.initialized = true;
        }

        var targetFov = input.GetAction(InputActions.Aim) ? preset.fOVZoom : cameraData.defaultFov;
        var t = preset.fOVLerpSpeed * Time.DeltaTime;
        camera.FieldOfView = camera.FieldOfView.Eerp(targetFov, t);
    }

    static void ApplyShake(Camera camera, ThirdPersonCameraPreset preset, CameraData data)
    {
        if (data.shakeTimer <= 0 || data.shakeIntensity <= 0) return;

        data.shakeTimer -= Time.DeltaTime;
        data.shakeIntensity *= Mathf.Exp(-preset.shakeDecay * Time.DeltaTime);

        if (data.shakeTimer <= 0 || data.shakeIntensity < 0.001f)
        {
            data.shakeIntensity = 0;
            data.shakeTimer = 0;
            return;
        }

        var t = Time.GameTime + data.shakeSeed;
        var offset = new Vector3(Mathf.Sin(t * 37.1f) * data.shakeIntensity, Mathf.Sin(t * 47.3f) * data.shakeIntensity, 0);

        camera.Position = data.basePosition + offset * preset.shakeScale;
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