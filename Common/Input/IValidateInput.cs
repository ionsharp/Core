namespace Imagin.Common.Input
{
    public interface IValidateInput
    {
        event InputValidationErrorEventHandler InputValidationError;

        bool CommitInput();
    }
}
