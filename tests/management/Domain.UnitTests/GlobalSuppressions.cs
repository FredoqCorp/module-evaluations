using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Style", "IDE0290:Use primary constructor", Justification = "Test code")]
[assembly: SuppressMessage("Security", "CA5394:Do not use insecure randomness", Justification = "Test code uses Random.Shared for non-security purposes")]
[assembly: SuppressMessage("Security", "SCS0005:Weak random number generator", Justification = "Test code uses Random.Shared for non-security purposes")]
[assembly: SuppressMessage("Style", "CS1591:Missing XML comment for publicly visible type or member", Justification = "Test methods do not require XML documentation")]
