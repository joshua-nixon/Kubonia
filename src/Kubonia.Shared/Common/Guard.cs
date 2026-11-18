using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Kubonia.Shared.Common
{
    public static class Guard
    {
        public static void IsNotNull<T>([NotNull] T? value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            ArgumentNullException.ThrowIfNull(value, paramName);
        }
    }
}
