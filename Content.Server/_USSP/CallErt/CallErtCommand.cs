using Robust.Shared.Console;
using Content.Shared.Administration;
using Content.Server.Administration;
using Robust.Shared.Prototypes;
using System.Linq;
using Robust.Shared.Audio;
using Robust.Shared.Player;
using Robust.Shared.Audio.Systems;
using Content.Server._USSP.CallErt;
using Content.Server.Chat.Systems;


namespace Content.Server._USSP.CallErt;

[AdminCommand(AdminFlags.Admin)]
public sealed class CallErt : LocalizedCommands
{
    public string Description => Loc.GetString("CallErtcommand-desc");
    public string Help => Loc.GetString("CallErtcommand-help");
    [Dependency] private readonly IEntityManager _entity = default!;
    [Dependency] private readonly IPrototypeManager _prototype = default!;

    public override string Command => "CallErt";

    public override CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        if (args.Length == 1)
        {
            var options = _prototype
                .EnumeratePrototypes<CallErtPresetPrototype>()
                .Select(p => new CompletionOption(p.ID, p.Desc));

            return CompletionResult.FromHintOptions(options.OrderBy(x => x.Value, StringComparer.Ordinal).ToArray(), Loc.GetString("CallErtcommand-id-preset"));
        }

        return CompletionResult.Empty;
    }

    public override void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        var chatSystem = IoCManager.Resolve<IEntitySystemManager>().GetEntitySystem<ChatSystem>();

        if (args.Length == 0)
        {
            shell.WriteError(Loc.GetString("CallErtcommand-error-args0"));
            _entity.System<SharedAudioSystem>().PlayGlobal("/Audio/_USSP/CallErt/noert.ogg", Filter.Broadcast(), true, AudioParams.Default.WithVolume(-2f));
            return;
        }
        if (args.Length > 1)
        {
            shell.WriteError(Loc.GetString("CallErtcommand-error-args1"));
            return;
        }
        var ertSpawnSystem = _entity.System<CallErtSystem>();
        var protoId = args[0];
        var prototypeManager = _prototype;
        if (!prototypeManager.TryIndex<CallErtPresetPrototype>(protoId, out var proto))
        {
            shell.WriteError(Loc.GetString("CallErtcommand-error-preset-not-found", ("protoid", protoId)));
            return;
        }
        if (ertSpawnSystem.SpawnErt(proto))
        {
            _entity.System<SharedAudioSystem>().PlayGlobal("/Audio/_USSP/CallErt/yesert.ogg", Filter.Broadcast(), true, AudioParams.Default.WithVolume(-5f));
            shell.WriteLine(Loc.GetString("CallErtcommand-preset-loaded", ("protoid", protoId)));
            return;
        }
        else
        {
            shell.WriteError(Loc.GetString("CallErtcommand-error-when-load-grid"));
            return;
        }
    }
}
