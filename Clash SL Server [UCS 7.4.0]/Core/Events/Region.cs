namespace CSS.Core.Events
{
    using System;
    using System.IO;
    using System.Text;
    using MaxMind.Db;
    using MaxMind.GeoIP2;
    using MaxMind.GeoIP2.Exceptions;

    internal class Region
    {
        internal DatabaseReader Reader = null;
        internal string DbPath = "Gamefiles/database/Database.mmdb";

        public Region()
        {
            if (!Directory.Exists("Gamefiles/database/"))
                throw new Exception("Directory Gamefiles/database does not exist!");

            if (!File.Exists(DbPath))
                throw new Exception($"{DbPath} does not exist in current directory!");

            this.Reader = new DatabaseReader(DbPath, FileAccessMode.Memory);

            Logger.Say("CSS region database loaded into memory.");
        }

        internal string GetIpCountryIso(string ipAddress)
        {
            if (ipAddress == null || this.Reader == null)
                return "CSS Land";
            try
            {
                return this.Reader.City(ipAddress).Country.IsoCode;
            }
            catch (AddressNotFoundException)
            {
                return "CSS Land";
            }
        }
        internal string GetIpCountry(string ipAddress)
        {
            if (ipAddress == null || this.Reader == null)
                return "CSS Land";
            try
            {
                return this.Reader.City(ipAddress).Country.Name;
            }
            catch (AddressNotFoundException)
            {
                return "CSS Land";
            }
        }

        internal string GetFullIpData(string ipAddress)
        {
            if (ipAddress == null || this.Reader == null)
                return "CSS Land";

            try
            {
                var city = this.Reader.City(ipAddress);
                var sb = new StringBuilder();

                sb.AppendLine("IP Country ISO: " + city.Country.IsoCode);
                sb.AppendLine("IP Country Name: " + city.Country.Name);
                sb.AppendLine();
                sb.AppendLine("IP Specific Subdivision ISO: " + city.MostSpecificSubdivision.IsoCode);
                sb.AppendLine("IP Specific Subdivision Name: " + city.MostSpecificSubdivision.Name);
                sb.AppendLine();
                sb.AppendLine("IP City:" + city.City.Name);
                sb.AppendLine("IP Postal:" + city.Postal.Code);
                sb.AppendLine();
                sb.AppendLine("IP Latitude:" + city.Location.Latitude);
                sb.AppendLine("IP Longitude:" + city.Location.Longitude);

                return sb.ToString();
            }
            catch (AddressNotFoundException)
            {
                return "CSS Land";
            }
        }
    }
}
