using System;
using System.Collections.Generic;



namespace MultisiteEventing.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Asset>? Assets { get; set; }
        public List<Event>? Events { get; set; }
    }

    public class Asset
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string UnitOfMeasure { get; set; }
    }

    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Store> Stores { get; set; }
        public List<Asset> Assets { get; set; }
        public List<Client> ComplimentaryClients { get; set; }
    }
}
