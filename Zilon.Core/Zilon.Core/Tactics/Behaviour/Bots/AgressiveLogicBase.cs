﻿using System;
using System.Collections.Generic;
using System.Linq;

using Zilon.Core.Persons;
using Zilon.Core.Tactics.Spatial;

namespace Zilon.Core.Tactics.Behaviour.Bots
{
    //TODO Учесть, что в один ход другой актёр может занять целевой узел.
    //TODO Учесть, что при малом расстоянии до цели нужно строить путь каждый ход
    //Иначе не получится догнать нарушителя.
    public abstract class AgressiveLogicBase : IBotLogic
    {
        private const int PursuitCounter = 3;
        //TODO Дальность видимости вынести в схему персонажа и, затем, в пересчитанном состоянии в актёра.
        private const int VisibilityRange = 5;

        protected readonly IActor Actor;
        protected readonly IMap Map;
        protected readonly IDecisionSource DecisionSource;

        private readonly IActorManager _actorList;
        private readonly ITacticalActUsageService _actService;

        private MoveTask _moveTask;
        private IdleTask _idleTask;
        private Mode _mode;
        private IAttackTarget _targetIntruder;
        private int _pursuitCounter;

        protected AgressiveLogicBase(IActor actor,
            IMap map,
            IActorManager actors,
            IDecisionSource decisionSource,
            ITacticalActUsageService actService)
        {
            Actor = actor;
            Map = map;
            DecisionSource = decisionSource;

            _actorList = actors;
            _actService = actService;
            _pursuitCounter = PursuitCounter;
        }

        public IActorTask GetCurrentTask()
        {
            // На каждом шаге осматриваем окрестности
            // на предмет нарушителей.
            var intruders = CheckForIntruders();

            var nearbyIntruder = intruders.FirstOrDefault();

            if (nearbyIntruder != null)
            {
                _mode = Mode.Pursuit;
                _targetIntruder = nearbyIntruder;
                _idleTask = null;
                ProcessIntruderDetected();
            }
            else if (_idleTask?.IsComplete == true)
            {
                _mode = Mode.Bypass;
                _targetIntruder = null;
                _idleTask = null;
            }

            switch (_mode)
            {
                case Mode.Bypass:
                    return HandleBypassMode();

                case Mode.Pursuit:
                    return HandlePersuitMode();

                case Mode.Idle:
                    return HandleIdleMode();

                default:
                    throw new InvalidOperationException($"Неизвестный режим патрулирования {_mode}");
            }
        }

        /// <summary>
        /// Метод для обработки ситуации, когда нарушитель обнаружен.
        /// </summary>
        /// <remarks>
        /// Используется логикой патруллирования для сброса текущего счётчика точки патруллирования.
        /// </remarks>
        protected abstract void ProcessIntruderDetected();

        private IActorTask HandleIdleMode()
        {
            if (_idleTask == null)
            {
                _idleTask = new IdleTask(Actor, DecisionSource);
            }
            else
            {
                // Ожидание окончено, нужно двигаться к следующей точке.
                if (_idleTask.IsComplete)
                {
                    _idleTask = null;
                    return HandleBypassMode();
                }
            }

            return _idleTask;
        }

        private IActorTask HandlePersuitMode()
        {
            var isAttackAllowed = CheckAttackAvailability(_targetIntruder);
            if (isAttackAllowed)
            {
                var attackTask = new AttackTask(Actor, _targetIntruder, _actService);
                return attackTask;
            }
            else
            {
                // Маршрут до цели обновляем каждые 3 хода.
                // Для оптимизации.
                // Эффект потери цели.

                //TODO Сделать тест аналогичный GetActorTasks_PatrolsTryToAttackEnemy_ReturnsMoveTask
                if (_pursuitCounter > 0 && _moveTask != null)
                {
                    _pursuitCounter--;
                    return _moveTask;
                }
                else
                {
                    RefreshPursuitCounter();
                    _moveTask = new MoveTask(Actor, _targetIntruder.Node, Map);
                    return _moveTask;
                }
            }
        }

        private void RefreshPursuitCounter()
        {
            _pursuitCounter = PursuitCounter;
        }

        private bool CheckAttackAvailability(IAttackTarget targetIntruder)
        {
            var actorNode = (HexNode)Actor.Node;
            var targetNode = (HexNode)targetIntruder.Node;

            if (Actor.Person.TacticalActCarrier != null)
            {
                var actCarrier = Actor.Person.TacticalActCarrier;
                var act = actCarrier.Acts.First();

                var isInDistance = act.CheckDistance(actorNode.CubeCoords, targetNode.CubeCoords);
                var targetIsOnLine = MapHelper.CheckNodeAvailability(Map, actorNode, targetNode);

                return isInDistance && targetIsOnLine;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        private IActorTask HandleBypassMode()
        {
            if (_moveTask == null)
            {
                _moveTask = CreateBypassMoveTask();
                return _moveTask;
            }
            else
            {
                if (!_moveTask.IsComplete)
                {
                    // Если команда на перемещение к целевой точке патруля не закончена,
                    // тогда продолжаем её.
                    // Предварительно проверяем, не мешает ли что-либо её продолжить выполнять.
                    if (!_moveTask.CanExecute())
                    {
                        _moveTask = CreateBypassMoveTask();
                    }

                    return _moveTask;
                }

                // Команда на перемещение к целевому узлу закончена.
                // Нужно выбрать следующую целевую точку и создать команду на простой.
                ProcessMovementComplete();

                _moveTask = null;
                _idleTask = new IdleTask(Actor, DecisionSource);
                _mode = Mode.Idle;
                return _idleTask;
            }
        }

        /// <summary>
        /// Обработка окончания передвижения.
        /// </summary>
        /// <remarks>
        /// Используется логикой патруля для установки указателя на следующий пункт маршрута.
        /// </remarks>
        protected abstract void ProcessMovementComplete();

        /// <summary>
        /// Создаёт задачу на перемещение к целевому узлу карты.
        /// </summary>
        /// <returns> Возвращает команду на перемещение. </returns>
        /// <remarks>
        /// Этот метод перегружается наследниками, задавая поведение,
        /// каким образом происходит обход карты.
        /// </remarks>
        protected abstract MoveTask CreateBypassMoveTask();

        private IActor[] CheckForIntruders()
        {
            var foundIntruders = new List<IActor>();
            foreach (var target in _actorList.Items)
            {
                if (target.Owner == Actor.Owner)
                {
                    continue;
                }

                if (target.Person.Survival.IsDead)
                {
                    continue;
                }

                var isVisible = CheckTargetVisible(Actor, target);
                if (!isVisible)
                {
                    continue;
                }

                foundIntruders.Add(target);
            }

            return foundIntruders.ToArray();
        }

        private bool CheckTargetVisible(IActor actor, IAttackTarget target)
        {
            var actorNode = (HexNode)actor.Node;
            var targetNode = (HexNode)target.Node;
            var distance = actorNode.CubeCoords.DistanceTo(targetNode.CubeCoords);

            var isVisible = distance <= VisibilityRange;
            return isVisible;
        }

        private enum Mode
        {
            /// <summary>
            /// Обход ключевых точек.
            /// </summary>
            Bypass,

            /// <summary>
            /// Преследование нарушителя.
            /// </summary>
            Pursuit,

            /// <summary>
            /// Задержка перед следующим решением.
            /// </summary>
            Idle
        }
    }
}
