using Words.Domain.Enums;

namespace Application.Services.UserWordActions;
public static class RecordTypesCollection
{
    public static Dictionary<string, int> RecordTypes =
        new Dictionary<string, int>()
        {
            { "word-count", (int)RecordTypesEnum.Words },
            { "repeats", (int)RecordTypesEnum.Repeats },
            { null!, (int)RecordTypesEnum.Words }
        };

}
