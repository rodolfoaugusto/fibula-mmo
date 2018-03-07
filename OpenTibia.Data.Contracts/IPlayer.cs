﻿namespace OpenTibia.Data.Contracts
{
    public interface ICipPlayer
    {
        short Player_Id { get; set; }
        string Charname { get; set; }
        int Account_Id { get; set; }
        int Account_Nr { get; set; }
        int Creation { get; set; }
        int Lastlogin { get; set; }
        byte Gender { get; set; }
        byte Online { get; set; }
        string Vocation { get; set; }
        byte Hideprofile { get; set; }
        int Playerdelete { get; set; }
        short Level { get; set; }
        string Residence { get; set; }
        string Oldname { get; set; }
        string Comment { get; set; }
        string CharIp { get; set; }
    }
}
