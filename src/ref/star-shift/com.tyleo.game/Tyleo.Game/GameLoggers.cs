#nullable enable

using Tyleo.Logging;

namespace Tyleo.Game
{
    /// <summary>
    /// Holds the loggers from this module.
    /// </summary>
    public sealed record GameLoggers(
        GameLoggers.PhysicsLoggers Physics,
        GameLoggers.UpdateLoggers Update,
        ILogger Config,
        ILogger DebugAssertions,
        ILogger Loggers,
        ILogger Math,
        ILogger RuntimeInitialization
    )
    {
        internal static GameLoggers FromLoggerNode(ILoggerFactoryNode node)
        {
            var tyleoNode = node.CreateChildFilterNode(
                "Tyleo.Game"
            );

            return new(
                Physics: PhysicsLoggers.FromLoggerNode(tyleoNode),
                Update: UpdateLoggers.FromLoggerNode(tyleoNode),

                DebugAssertions: tyleoNode.CreateChildLogger(nameof(DebugAssertions)),
                Config: tyleoNode.CreateChildLogger(nameof(Config)),
                Loggers: tyleoNode.CreateChildLogger(nameof(Loggers)),
                Math: tyleoNode.CreateChildLogger(nameof(Math)),
                RuntimeInitialization: tyleoNode.CreateChildLogger(nameof(RuntimeInitialization))
            );
        }

        public sealed record PhysicsLoggers(
            PhysicsLoggers.TriggerLoggers Trigger
        )
        {
            internal static PhysicsLoggers FromLoggerNode(ILoggerFactoryNode node)
            {
                var physicsNode = node.CreateChildFilterNode(
                    nameof(Physics)
                );

                return new(
                    Trigger: TriggerLoggers.FromLoggerNode(physicsNode)
                );
            }

            public sealed record TriggerLoggers(
                ILogger OnEnter,
                ILogger OnExit,
                ILogger OnStay
            )
            {
                internal static TriggerLoggers FromLoggerNode(ILoggerFactoryNode node)
                {
                    var triggersNode = node.CreateChildFilterNode(
                        nameof(Trigger)
                    );

                    return new(
                        OnEnter: triggersNode.CreateChildLogger(nameof(OnEnter)),
                        OnExit: triggersNode.CreateChildLogger(nameof(OnExit)),
                        OnStay: triggersNode.CreateChildLogger(nameof(OnStay))
                    );
                }
            }
        }


        public sealed record UpdateLoggers(
            ILogger Fixed,
            ILogger Runtime,
            ILogger RuntimeOnly,
            ILogger Editor,
            ILogger EditorAndRuntime,
            ILogger EditorOnly
        )
        {
            internal static UpdateLoggers FromLoggerNode(ILoggerFactoryNode node)
            {
                var updateNode = node.CreateChildFilterNode(
                    nameof(Update)
                );

                return new(
                    Fixed: updateNode.CreateChildLogger(nameof(Fixed)),
                    Runtime: updateNode.CreateChildLogger(nameof(Runtime)),
                    RuntimeOnly: updateNode.CreateChildLogger(nameof(RuntimeOnly)),
                    Editor: updateNode.CreateChildLogger(nameof(Editor)),
                    EditorAndRuntime: updateNode.CreateChildLogger(nameof(EditorAndRuntime)),
                    EditorOnly: updateNode.CreateChildLogger(nameof(EditorOnly))
                );
            }
        }
    }
}