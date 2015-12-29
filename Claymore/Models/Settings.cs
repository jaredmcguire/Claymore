namespace Claymore.Models
{
    public interface ISettings
    {
        string ConnectionString { get; set; }
        string OutputPath { get; set; }
        bool ScriptTables { get; set; }
        bool ScriptViews { get; set; }
        bool ScriptSprocs { get; set; }
        bool ScriptFunctions { get; set; }
        bool ScriptTriggers { get; set; }
        bool ScriptSchemas { get; set; }
        bool ScriptUsers { get; set; }
    }

    public class Settings : ISettings
    {
        public string ConnectionString { get; set; }
        public string OutputPath { get; set; }
        public bool ScriptTables { get; set; }
        public bool ScriptViews { get; set; }
        public bool ScriptSprocs { get; set; }
        public bool ScriptFunctions { get; set; }
        public bool ScriptTriggers { get; set; }
        public bool ScriptSchemas { get; set; }
        public bool ScriptUsers { get; set; }
    }
}