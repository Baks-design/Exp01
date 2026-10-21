using System.Runtime.CompilerServices;
using FlaxEngine;

namespace Game.Core.Extensions;

public static class LerpExtensions
{
    const MethodImplOptions INLINE = MethodImplOptions.AggressiveInlining;

    extension(float current)
    {
        [MethodImpl(INLINE)]
        public float Lerp(float target, float deltaTime)
            => (1f - deltaTime) * current + deltaTime * target;

        [MethodImpl(INLINE)]
        public float ExpDecay(float target, float deltaTime, float decay = 16f)
            => Lerp(current, target, 1f - Mathf.Exp(-decay * deltaTime));

        /// <summary>
        /// Exponential interpolation, the multiplicative version of lerp, 
        /// useful for values such as scaling or zooming
        /// </summary>
        [MethodImpl(INLINE)]
        public float Eerp(float target, float time) =>
            time switch
            {
                0f => current,
                1f => target,
                _ => current * Mathf.Exp(Mathf.Log(target / current) * time)
            };
    }

    extension(Vector2 current)
    {
        [MethodImpl(INLINE)]
        public Vector2 ExpDecay(Vector2 target, float deltaTime, float decay = 16f)
            => Lerp(current, target, 1f - Mathf.Exp(-decay * deltaTime));

        [MethodImpl(INLINE)]
        public Vector2 Lerp(Vector2 target, float t)
            => new(
                Lerp(current.X, target.X, t),
                Lerp(current.Y, target.Y, t)
            );
    }

    extension(Vector3 current)
    {
        [MethodImpl(INLINE)]
        public Vector3 Lerp(Vector3 target, float deltaTime)
            => new(
                Lerp(current.X, target.X, deltaTime),
                Lerp(current.Y, target.Y, deltaTime),
                Lerp(current.Z, target.Z, deltaTime)
            );

        [MethodImpl(INLINE)]
        public Vector3 ExpDecay(Vector3 target, float deltaTime, float decay = 16f)
            => Lerp(current, target, 1f - Mathf.Exp(-decay * deltaTime));

        /// <inheritdoc cref="Eerp(float,float,float)"/>
        [MethodImpl(INLINE)]
        public Vector3 Eerp(Vector3 target, float time) =>
            time switch
            {
                0f => current,
                1f => target,
                _ => new Vector3(
                    Eerp(current.X, target.X, time),
                    Eerp(current.Y, target.Y, time),
                    Eerp(current.Z, target.Z, time)
                )
            };
    }
}