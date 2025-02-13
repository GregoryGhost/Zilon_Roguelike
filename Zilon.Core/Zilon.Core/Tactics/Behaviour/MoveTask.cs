﻿using System;
using System.Collections.Generic;
using System.Linq;

using Zilon.Core.Tactics.Spatial;
using Zilon.Core.Tactics.Spatial.PathFinding;

namespace Zilon.Core.Tactics.Behaviour
{
    public class MoveTask : ActorTaskBase
    {
        private readonly IMap _map;
        private readonly List<IMapNode> _path;

        public IMapNode TargetNode { get; }

        public override void Execute()
        {
            if (IsComplete)
            {
                return;
            }

            if (!_path.Any())
            {
                if (TargetNode != Actor.Node)
                {
                    throw new TaskException("Актёр не достиг целевого узла при окончании пути.");
                }

                _isComplete = true;
                return;
            }

            var nextNode = _path[0];

            if (!_map.IsPositionAvailableFor(nextNode, Actor))
            {
                throw new InvalidOperationException($"Попытка переместиться в заблокированную ячейку {nextNode}.");
            }

            _map.ReleaseNode(Actor.Node, Actor);
            Actor.MoveToNode(nextNode);
            _map.HoldNode(nextNode, Actor);

            _path.RemoveAt(0);

            if (!_path.Any())
            {
                _isComplete = true;
            }
        }

        public bool CanExecute()
        {
            if (!_path.Any())
            {
                return false;
            }

            var nextNode = _path[0];

            return _map.IsPositionAvailableFor(nextNode, Actor);
        }

        public MoveTask(IActor actor, IMapNode targetNode, IMap map) : base(actor)
        {
            TargetNode = targetNode ?? throw new ArgumentNullException(nameof(targetNode));
            _map = map ?? throw new ArgumentNullException(nameof(map));

            if (actor.Node == targetNode)
            {
                // Это может произойти, если источник команд выбрал целевую точку ту же, что и сам актёр
                // в результате рандома.
                _isComplete = true;

                _path = new List<IMapNode>(0);
            }
            else
            {
                _path = new List<IMapNode>();

                CreatePath();

                if (!_path.Any())
                {
                    _isComplete = true;
                }
            }
        }

        private void CreatePath()
        {
            var context = new PathFindingContext(Actor)
            {
                TargetNode = TargetNode
            };

            var startNode = Actor.Node;
            var finishNode = TargetNode;

            _path.Clear();

            _map.FindPath(startNode, finishNode, context, _path);
        }

        public override string ToString()
        {
            return $"{Actor} ({TargetNode})";
        }
    }
}