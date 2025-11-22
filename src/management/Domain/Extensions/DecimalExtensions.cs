namespace CascVel.Modules.Evaluations.Management.Domain.Extensions;

static class DecimalExtensions
{
    extension(decimal value)
    {
        public ushort BasisPoints()
        {
            if (value < 1m || value > 100m)
            {
                throw new InvalidDataException("Basis points must be between 1.00 and 100.00");
            }
            return (ushort)(value * 100);
        }
    }
}

