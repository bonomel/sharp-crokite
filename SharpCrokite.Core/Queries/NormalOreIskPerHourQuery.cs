using SharpCrokite.Core.Models;
using SharpCrokite.DataAccess.Models;
using SharpCrokite.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpCrokite.Core.Queries
{
    public class NormalOreIskPerHourQuery
    {
        private HarvestableRepository harvestableRepository;
        private MaterialRepository materialRepository;

        public NormalOreIskPerHourQuery(HarvestableRepository harvestableRepository, MaterialRepository materialRepository)
        {
            this.harvestableRepository = harvestableRepository;
            this.materialRepository = materialRepository;
        }

        internal IEnumerable<NormalOreIskPerHour> Execute()
        {
            List<NormalOreIskPerHour> normalOreIskPerHourCollection = new();

            IEnumerable<Harvestable> harvestableModels = harvestableRepository.Find(h =>
            h.Type == "Veldspar" ||
            h.Type == "Scordite" ||
            h.Type == "Pyroxeres" ||
            h.Type == "Plagioclase");

            foreach(Harvestable harvestableModel in harvestableModels)
            {
                normalOreIskPerHourCollection.Add(new()
                {
                    Icon = harvestableModel.Icon,
                    Name = harvestableModel.Name,
                    Description = harvestableModel.Description,
                    Tritanium = 0,
                    Pyerite = 0,
                    Isogen = 0,
                    Megacyte = 0,
                    Mexallon = 0,
                    Nocxium = 0,
                    Zydrine = 0,
                    MaterialIskPerHour = "todo",
                    CompressedIskPerHour = "todo"
                });
            }

            return normalOreIskPerHourCollection;
        }
    }
}
