using SQLite;

namespace AirlinesApp.Models;

[Table("routes")]
public class Route
{
    [PrimaryKey, AutoIncrement]
    public int id { get; set; }
    public int route_number { get; set; }
    public string departure_airport { get; set; } = string.Empty;
    public string arrival_airport { get; set; } = string.Empty;
    public decimal ticket_price { get; set; }
    public string flight_duration { get; set; } = string.Empty;
}

[Table("aircrafts")]
public class Aircraft
{
    [PrimaryKey, AutoIncrement]
    public int id { get; set; }
    public int tail_number { get; set; }
    public string model { get; set; } = string.Empty;
    public DateTime manufacture_date { get; set; } = DateTime.Now;
    public DateTime operational_life { get; set; } = DateTime.Now.AddYears(20);
    public bool is_ready_for_flight { get; set; } = true;
    public int commander_ship_id { get; set; }
}

[Table("commander_ships")]
public class Commander
{
    [PrimaryKey, AutoIncrement]
    public int id { get; set; }
    public int personal_number { get; set; }
    public string name { get; set; } = string.Empty;
    public string address { get; set; } = string.Empty;
    public string phone { get; set; } = string.Empty;
    public string flight_hours { get; set; } = string.Empty;
}

[Table("passengers")]
public class Passenger
{
    [PrimaryKey, AutoIncrement]
    public int id { get; set; }
    public int passport_number { get; set; }
    public string full_name { get; set; } = string.Empty;
    public string address { get; set; } = string.Empty;
    public string phone { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;
}

[Table("flights")]
public class Flight
{
    [PrimaryKey, AutoIncrement]
    public int id { get; set; }
    public int flight_number { get; set; }
    public DateTime departure_datetime { get; set; } = DateTime.Now;
    public bool is_cancelled { get; set; }
    public int route_id { get; set; }
    public int aircraft_id { get; set; }
    public int passengers_id { get; set; }
}