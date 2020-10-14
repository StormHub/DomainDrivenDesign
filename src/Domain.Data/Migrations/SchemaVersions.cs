using System;
using System.ComponentModel.DataAnnotations;

// ReSharper disable UnusedAutoPropertyAccessor.Local
namespace Domain.Data.Migrations
{
    public sealed class SchemaVersions
    {
        [Required]
        public int Id { get; private set; }

        [Required]
        public string ScriptName { get; private set; }

        [Required]
        public DateTime Applied { get; private set; }
    }
}