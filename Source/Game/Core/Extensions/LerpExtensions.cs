using System.Runtime.CompilerServices;
using FlaxEngine;

namespace Game.Core.Extensions;

public static class LerpExtensions
{
    const MethodImplOptions INLINE = MethodImplOptions.AggressiveInlining;

    extension(float current)
    {
        [MethodImpl(INLINE)]
        public float Lerp(float target, float t) => (1f - t) * current + t * target;

        [MethodImpl(INLINE)]
        public float ExpDecay(float target, float deltaTime, float decay = 16f)
            => current.Lerp(target, 1f - Mathf.Exp(-decay * deltaTime));

        [MethodImpl(INLINE)]
        public float Eerp(float target, float time)
        {
            switch (time)
            {
                case 0f: return current;
                case 1f: return target;
            }
            if (current == 0f || target / current <= 0f) return current;
            return current * Mathf.Exp(Mathf.Log(target / current) * time);
        }
    }

    extension(Vector2 current)
    {
        [MethodImpl(INLINE)]
        public Vector2 Lerp(Vector2 target, float t)
            => new(current.X.Lerp(target.X, t), current.Y.Lerp(target.Y, t));

        [MethodImpl(INLINE)]
        public Vector2 ExpDecay(Vector2 target, float deltaTime, float decay = 16f)
            => current.Lerp(target, 1f - Mathf.Exp(-decay * deltaTime));
    }

    extension(Vector3 current)
    {
        [MethodImpl(INLINE)]
        public Vector3 Lerp(Vector3 target, float t)
            => new(current.X.Lerp(target.X, t), current.Y.Lerp(target.Y, t), current.Z.Lerp(target.Z, t));

        [MethodImpl(INLINE)]
        public Vector3 ExpDecay(Vector3 target, float deltaTime, float decay = 16f)
            => current.Lerp(target, 1f - Mathf.Exp(-decay * deltaTime));

        [MethodImpl(INLINE)]
        public Vector3 Eerp(Vector3 target, float time) => time switch
        {
            0f => current,
            1f => target,
            _ => new Vector3(current.X.Eerp(target.X, time), current.Y.Eerp(target.Y, time), current.Z.Eerp(target.Z, time)),
        };
    }
}