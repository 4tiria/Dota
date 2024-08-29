using MongoDB.Bson;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace NoSql.Models;

[Table("heroInMatch")]

public class HeroInMatch
{
    public Hero Hero { get; set; }

    public string Side { get; set; }

    public int Kills { get; set; }
    public int Deaths { get; set; }
    public int Assists { get; set; }

    public int Gold { get; set; }

    public int XP { get; set; }
}