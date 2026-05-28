namespace Application.Exceptions;

public sealed class ApplicationValidationException : Exception
{
    public ApplicationValidationException(IEnumerable<string> errors)
        : base("One or more validation failures have occurred.")
    {
        ArgumentNullException.ThrowIfNull(errors);

        Errors = errors.ToArray();
    }

    public IReadOnlyCollection<string> Errors { get; }
}
