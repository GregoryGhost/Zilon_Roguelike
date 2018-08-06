﻿using System;
using System.Collections.Generic;

using Zilon.Core.Components;
using Zilon.Core.Schemes;

namespace Zilon.Core.Persons
{
    /// <summary>
    /// Персонаж, находящийся под управлением игрока.
    /// </summary>
    public class HumanPerson : IPerson
    {
        public int Id { get; set; }

        public string Name { get; }

        public float Hp => Scheme.Hp;

        public IEquipmentCarrier EquipmentCarrier { get; }

        public ITacticalActCarrier TacticalActCarrier { get; }

        public IEvolutionData EvolutionData { get; }

        public PersonScheme Scheme { get; }

        public ICombatStats CombatStats { get; }

        public IPropStore Inventory { get; }

        public HumanPerson(PersonScheme scheme)
        {
            if (scheme == null)
            {
                throw new ArgumentNullException(nameof(scheme));
            }

            Scheme = scheme;

            var slotCount = Scheme.SlotCount;
            EquipmentCarrier = new EquipmentCarrier(slotCount);
            EquipmentCarrier.EquipmentChanged += EquipmentCarrier_EquipmentChanged;

            TacticalActCarrier = new TacticalActCarrier();


            EvolutionData = new EvolutionData();

            CombatStats = new CombatStats() {
                //TODO Статы рассчитывать на основании схемы персонажа, перков, экипировки
                Stats =new[]{
                    new CombatStatItem {Stat = CombatStatType.Melee, Value = 10 },
                    new CombatStatItem {Stat = CombatStatType.Ballistic, Value = 10 },
                }
            };

            CalcCombatStats(CombatStats, EvolutionData);
        }

        public HumanPerson(PersonScheme scheme, Inventory inventory): this(scheme)
        {
            Inventory = inventory;
        }

        private void CalcCombatStats(ICombatStats combatStats, IEvolutionData evolutionData)
        {
            foreach (var archievedPerk in evolutionData.ArchievedPerks)
            {
                //TODO Перенести наработки по вычислению текущего апдейта перка.
            }
        }

        private void EquipmentCarrier_EquipmentChanged(object sender, EventArgs e)
        {
            TacticalActCarrier.Acts = CalcActs(EquipmentCarrier.Equipments, CombatStats);
        }

        private static ITacticalAct[] CalcActs(IEnumerable<Equipment> equipments, ICombatStats combatStats)
        {
            if (equipments == null)
            {
                throw new ArgumentNullException(nameof(equipments));
            }

            var actList = new List<ITacticalAct>();

            foreach (var equipment in equipments)
            {
                if (equipment == null)
                {
                    continue;
                }

                foreach (var actScheme in equipment.Acts)
                {
                    var equipmentPower = CalcEquipmentEfficient(equipment);
                    var act = new TacticalAct(equipmentPower, actScheme, combatStats);

                    actList.Add(act);
                }
            }

            return actList.ToArray();
        }

        private static float CalcEquipmentEfficient(Equipment equipment)
        {
            return equipment.Power * equipment.Scheme.Equip.Power;
        }

        public override string ToString()
        {
            return $"{Name}";
        }

    }
}