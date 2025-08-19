namespace FluentBuilderGeneratorTests.DTO
{
    public record RecordWithPrimaryConstructor(string Test, int Num)
    {
        public string Normal { get; set; } = string.Empty;

        public string Data { get; } = Test + Num;
    }
}