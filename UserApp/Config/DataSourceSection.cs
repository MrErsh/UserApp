using System.Configuration;

namespace UserApp.Config
{
    /// <summary>
    /// Секция, определяющая поставщика данных.
    /// </summary>
    public class DataSourceSection : ConfigurationSection
    {
        /// <summary>
        /// SQLServer, XML, INMEMORY.
        /// </summary>
        [ConfigurationProperty("type", IsRequired = true)]
        public string Type
        {
            get { return ((string) (base["type"])); }
            set { base["type"] = value; }
        }

        /// <summary>
        /// Строка соединения при Type "SQLServer".
        /// </summary>
        [ConfigurationProperty("connStr", IsRequired = false)]
        public string ConnectionString
        {
            get { return ((string) (base["connStr"])); }
            set { base["connStr"] = value; }
        }

        /// <summary>
        /// Путь к файлу при Type "XML".
        /// </summary>
        [ConfigurationProperty("path", IsRequired = false)]
        public string Path
        {
            get { return ((string) (base["path"])); }
            set { base["path"] = value; }
        }
    }
}
