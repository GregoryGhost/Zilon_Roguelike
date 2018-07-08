﻿using Zilon.Core.Schemes;

namespace Zilon.Core.Persons
{
    public class PersonService
    {
        private readonly ISchemeService _schemeService;

        public PersonService(ISchemeService schemeService)
        {
            _schemeService = schemeService;
        }

        public IPerson Create()
        {
            var person = new Person();
            return person;
        }

        public void SetEquipment(IPerson person, Equipment equipment, int slotIndex)
        {
            person.EquipmentCarrier.SetEquipment(equipment, slotIndex);
        }
    }
}