﻿using CsvHelper;
using SatellEarthAPI.Application.Common.Interfaces;
using SatellEarthAPI.Application.TodoLists.Queries.ExportTodos;
using SatellEarthAPI.Infrastructure.Files.Maps;
using System.Globalization;

namespace SatellEarthAPI.Infrastructure.Files
{
    public class CsvFileBuilder : ICsvFileBuilder
    {
        public byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records)
        {
            using var memoryStream = new MemoryStream();
            using (var streamWriter = new StreamWriter(memoryStream))
            {
                using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

                csvWriter.Configuration.RegisterClassMap<TodoItemRecordMap>();
                csvWriter.WriteRecords(records);
            }

            return memoryStream.ToArray();
        }
    }
}