using GoNatureAR.Sensors;
using System;

namespace GoNatureAR
{
    /// <summary>
    /// A generic enum that can only take three different type of enums.
    /// 1. ThermalComfortMeasure
    /// 2. AirQualityMeasure
    /// 3. NoiseMeasure
    /// </summary>
    [Serializable]
    public class EnumKey
    {
        private object enumValue;

        public EnumKey(object enumValue)
        {
            if (!(enumValue is ThermalComfortMeasurements || enumValue is AirQualityMeasurements || enumValue is NoiseMeasurements))
                throw new ArgumentException("Invalid enum type");

            this.enumValue = enumValue;
        }

        public object EnumValue => enumValue;

        public Enum GetEnumValue()
        {
            if (enumValue is Enum)
                return (Enum)enumValue;
            else
                throw new InvalidOperationException("Invalid enum type");
        }

        public override bool Equals(object obj)
        {
            return obj is EnumKey key && Equals(enumValue, key.enumValue);
        }

        public override int GetHashCode()
        {
            return enumValue.GetHashCode();
        }
    }
}
