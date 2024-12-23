// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-18 20:12:46
// # Recently: 2024-12-22 20:12:12
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;

namespace JFramework
{
    [Serializable]
    public struct Variable<T> : IEquatable<Variable<T>>
    {
        public T origin;
        public int buffer;
        public int offset;

        public T Value
        {
            get
            {
                if (offset == 0)
                {
                    Value = origin;
                }

                if (buffer == unchecked(origin.GetHashCode() + offset))
                {
                    return origin;
                }

                Service.Event.Invoke(new VariableUpdateEvent());
                return default;
            }
            set
            {
                origin = value ?? (T)(object)string.Empty;
                offset = Service.Random.Next(1, int.MaxValue);
                buffer = unchecked(origin.GetHashCode() + offset);
            }
        }

        public Variable(T value = default)
        {
            origin = value ?? (T)(object)string.Empty;
            offset = Service.Random.Next(1, int.MaxValue);
            buffer = unchecked(origin.GetHashCode() + offset);
        }

        public static implicit operator T(Variable<T> variable)
        {
            return variable.Value;
        }

        public static implicit operator Variable<T>(T value)
        {
            return new Variable<T>(value);
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public bool Equals(Variable<T> other)
        {
            return EqualityComparer<T>.Default.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            return obj is Variable<T> other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (origin != null ? origin.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ buffer;
                hashCode = (hashCode * 397) ^ offset;
                return hashCode;
            }
        }
    }
}