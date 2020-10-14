using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Domain.Infrastructure
{
    public abstract class ValueObject : IEquatable<ValueObject>
    {
        List<PropertyInfo> properties;
        List<FieldInfo> fields;

        public static bool operator ==(ValueObject obj1, ValueObject obj2)
        {
            if (obj1 is null)
            {
                return obj2 is null;
            }

            return obj1.Equals(obj2);
        }

        public static bool operator !=(ValueObject obj1, ValueObject obj2) => !(obj1 == obj2);

        public bool Equals(ValueObject obj) => Equals(obj as object);

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            return GetProperties().All(p => PropertiesAreEqual(obj, p))
                && GetFields().All(f => FieldsAreEqual(obj, f));
        }

        bool PropertiesAreEqual(object obj, PropertyInfo p) => p.GetValue(this, null) == p.GetValue(obj, null);

        bool FieldsAreEqual(object obj, FieldInfo f) => Equals(f.GetValue(this), f.GetValue(obj));

        IEnumerable<PropertyInfo> GetProperties() => properties ??= GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(p => !Attribute.IsDefined(p, typeof(IgnoreMemberAttribute))).ToList();

        IEnumerable<FieldInfo> GetFields() => fields ??= GetType()
            .GetFields(BindingFlags.Instance | BindingFlags.Public)
            .Where(p => !Attribute.IsDefined(p, typeof(IgnoreMemberAttribute)))
            .ToList();

        public override int GetHashCode()
        {
            unchecked //allow overflow
            {
                int hash = 17;
                foreach (PropertyInfo prop in GetProperties())
                {
                    object value = prop.GetValue(this, null);
                    hash = HashValue(hash, value);
                }

                foreach (FieldInfo field in GetFields())
                {
                    object value = field.GetValue(this);
                    hash = HashValue(hash, value);
                }

                return hash;
            }
        }

        static int HashValue(int seed, object value)
        {
            int currentHash = value?.GetHashCode() ?? 0;
            return seed * 23 + currentHash;
        }
    }
}