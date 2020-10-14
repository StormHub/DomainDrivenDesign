using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Design", "CA1812:Not used", Justification = "DI Automatic registered")]
[assembly: SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "Domain marker interface")]
[assembly: SuppressMessage("Design", "CA1030:Use events where appropriate", Justification = "Domain events")]
[assembly: SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "Domain events")]
