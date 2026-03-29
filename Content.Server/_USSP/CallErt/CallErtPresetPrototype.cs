using Robust.Shared.Prototypes;

namespace Content.Server._USSP.CallErt;

[Serializable, Prototype("CallErt")]
public sealed partial class CallErtPresetPrototype : IPrototype
{
    [IdDataField] public string ID { get; set; } = default!;

    [DataField("path")] public string Path { get; set; } = string.Empty;

    [DataField("desc")] public string Desc { get; set; } = string.Empty;
}