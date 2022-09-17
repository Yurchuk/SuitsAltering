using SuitsAltering.BL.Results;
using SuitsAltering.DAL.Enums;

namespace SuitsAltering.BL.Errors
{
    public static class AlteringErrors
    {
        public static Error InvalidClothingType => new Error("A0001", "Wrong clothing type");
        public static Error InvalidAlteringLengthRange => new Error("A0002", "Wrong altering length, it should be between -5 and 5 cm");
        public static Error BothAdjustmentsCannotBeZero => new Error("A0003", "Both adjustments cannot be 0 cm");
        public static Error AlteringNotFound => new Error("A0004", "Altering not found");
        public static Error CannotBeUpdatedWithThisStatus(string currentStatus, string statusToUpdate) => new Error("A0005", $"Cannot update to status - {statusToUpdate} because current status is {currentStatus}");
        public static Error CanBeUpdatedOnlyToStatusStartedOrDone => new Error("A0006", $"Status can be updated only to statuses: {AlteringStatus.Started} and {AlteringStatus.Done}");
    }
}
